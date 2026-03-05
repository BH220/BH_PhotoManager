using BH_WaitingPopupWinform; 
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp.Processing;
using System.Diagnostics;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Image = SixLabors.ImageSharp.Image;

namespace BH_PhotoFrameMaker
{
    public partial class frmMain : Form
    {
        CancellationTokenSource cts = null;
        private readonly HashSet<string> imageExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg",".jpeg",".png",".gif",".bmp",".tif",".tiff"
        };
        List<string> lstConvertTarget = new List<string>();
        List<string> lstFailTarget = new List<string>();

        private string pathOrigin
        {
            get
            {
                string path = Path.Combine(SettingHelper.Instance.RootPath, "origin");
                if (Directory.Exists(path) == false)
                    Directory.CreateDirectory(path);
                return path;
            }
        }
        private string pathFail
        {
            get
            {
                string path = Path.Combine(SettingHelper.Instance.RootPath, "fail");
                if (Directory.Exists(path) == false)
                    Directory.CreateDirectory(path);
                return path;
            }
        }
        private string pathConvert
        {
            get
            {
                string path = Path.Combine(SettingHelper.Instance.RootPath, "convert");
                if (Directory.Exists(path) == false)
                    Directory.CreateDirectory(path);
                return path;
            }
        }
        private string pathDuplication
        {
            get
            {
                string path = Path.Combine(SettingHelper.Instance.RootPath, "duplication");
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                return path;
            }
        }

        public frmMain()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            PreSetSetting();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            PathCheck();
        }

        #region 액자용으로 변환 복사 관련
        private void btnItemDelete_Click(object sender, EventArgs e)
        {
            while (lstTarget.SelectedItems.Count > 0)
            {
                lstTarget.Items.Remove(lstTarget.SelectedItems[0]);
            }
        }

        private async void btnConvertCopy_Click(object sender, EventArgs e)
        {
            WaitingPopupManager.ShowForm(this, "변환 중입니다.", "잠시만 기다려주세요.");
            await Task.Run(() =>
            {
                string rootOrigin = Path.Combine(SettingHelper.Instance.RootPath, "origin");
                if (Directory.Exists(rootOrigin) == false)
                    Directory.CreateDirectory(rootOrigin);

                for (int idx = lstTarget.Items.Count - 1; idx >= 0; idx--)
                {
                    bool result = CopyToOrigin(lstTarget.Items[idx].ToString());
                    if (result)
                    {
                        lstTarget.Invoke(() =>
                        {
                            lstTarget.Items.RemoveAt(idx);
                        });
                    }
                }
            });
            WaitingPopupManager.CloseForm();
        }

        private bool CopyToOrigin(string path)
        {
            bool result = false;
            try
            {
                string ymd = $"20{path.Replace(SettingHelper.Instance.PhotoRootPath, "").Substring(6, 6)}";
                string newFile = $"{SettingHelper.Instance.RootPath}\\origin\\{ymd.Substring(0, 4)}-{ymd.Substring(4, 2)}-{ymd.Substring(6, 2)}_{Path.GetFileName(path)}";
                if (File.Exists(newFile) == false)
                {
                    File.Copy(path, newFile);
                }
                result = true;
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        private void lbOriginPath_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (string.IsNullOrEmpty(lbOriginPath.Text) == false)
            {
                Process.Start("explorer.exe", lbOriginPath.Text);
            }
        }

        private void lstTarget_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private async void lstTarget_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data == null || !e.Data.GetDataPresent(DataFormats.FileDrop))
                return;

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            foreach (var file in files)
            {
                if (File.Exists(file))
                {
                    string extension = Path.GetExtension(file).ToLower();

                    if (imageExtensions.Contains(extension))
                    {
                        if (!lstTarget.Items.Contains(file)) // 중복 방지
                        {
                            lstTarget.Items.Add(file);
                        }
                    }
                }
            }
            if (SettingHelper.Instance.IsAutoConvert && lstTarget.Items.Count > 0)
            {
                Application.DoEvents();
                await Task.Delay(200);
                btnConvertCopy_Click(null, null);
            }
        }

        private void btnItemDeleteAll_Click(object sender, EventArgs e)
        {
            while (lstTarget.Items.Count > 0)
            {
                lstTarget.Items.Remove(lstTarget.Items[0]);
            }
        }
        #endregion

        #region 액자용으로 컨버팅 관련
        private void btnReset_Click(object sender, EventArgs e)
        {
            List<Control> controls = new List<Control>() {
                txtTotal,
                txtSuccess,
                txtFail
            };

            foreach (var control in controls)
            {
                control.Text = "";
            }
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            lstConvertTarget = new List<string>();
            lstFailTarget = new List<string>();
            lstConvertTarget.AddRange(Directory.GetFiles(pathOrigin).ToList());
            lstFailTarget.AddRange(Directory.GetFiles(pathFail).ToList());
            txtTotal.Text = (lstConvertTarget.Count + lstFailTarget.Count).ToString("#,0");
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            btnCheck_Click(null, null);
            btnCheck.Enabled =
            btnReset.Enabled =
            groupBox2.Enabled =
            btnStart.Enabled = false;
            try
            {
                List<string> lstTarget = GetImageFiles(pathFail);
                List<string> originFiles = GetImageFiles(pathOrigin);
                lstTarget.AddRange(originFiles);

                int total = lstTarget.Count;

                InitUI(total);
                await Task.Run(() => ConvertFile(lstTarget));
            }
            finally
            {
                btnCheck.Enabled =
                btnReset.Enabled =
                groupBox2.Enabled =
                btnStart.Enabled = true;
            }
        }

        private void ConvertFile(List<string> files, bool updateUi = true)
        {
            int cnt_success = 0;
            int cnt_fail = 0;

            string fileName = "";
            string ext = "";
            string convert = "";
            int total = files.Count;

            JpegEncoder jpegEncoder = new JpegEncoder
            {
                Quality = 75,
                ColorType = JpegEncodingColor.YCbCrRatio420
            };

            foreach (string file in files)
            {
                try
                {
                    fileName = Path.GetFileNameWithoutExtension(file);
                    ext = Path.GetExtension(file);

                    convert = Path.Combine(pathConvert, $"{fileName}_convert.jpg");

                    if (File.Exists(convert))
                    {
                        cnt_success++;
                        continue;
                    }

                    using (Image image = Image.Load(file))
                    {
                        image.Mutate(x =>
                        {
                            x.AutoOrient();

                            double ratioX = (double)SettingHelper.Instance.ConvertWidth / image.Width;
                            double ratioY = (double)SettingHelper.Instance.ConvertHeight / image.Height;
                            double ratio = Math.Min(ratioX, ratioY);

                            int newWidth = (int)(image.Width * ratio);
                            int newHeight = (int)(image.Height * ratio);

                            x.Resize(newWidth, newHeight, KnownResamplers.Bicubic);
                        });

                        image.Metadata.ExifProfile = null;

                        image.Save(convert, jpegEncoder);
                    }
                    cnt_success++;
                }
                catch
                {
                    MoveFile(file, pathFail, fileName, ext);
                    cnt_fail++;
                }
                finally
                {
                    if(updateUi)
                        UpdateUI(cnt_success, cnt_fail, total);
                }
            }
        }

        private void UpdateUI(int cnt_success, int cnt_fail, int total)
        {
            string success = cnt_success.ToString("#,0");
            if (txtSuccess.Text != success)
            {
                txtSuccess.Invoke(() =>
                {
                    txtSuccess.Text = success;
                });
            }
            string fail = cnt_fail.ToString("#,0");
            if (txtFail.Text != fail)
            {
                txtFail.Invoke(() =>
                {
                    txtFail.Text = fail;
                });
            }
            int process = (int)(((double)(cnt_success + cnt_fail) / (double)total) * (double)100);
            if (progressBar1.Value != process)
            {
                progressBar1.Invoke(() =>
                {
                    progressBar1.Value = cnt_success + cnt_fail;
                });
                lbPercent.Invoke(() =>
                {
                    lbPercent.Text = $"진행률 ({process}%)";
                });
            }
        }

        private void MoveFile(string fullPath, string path, string name, string ext)
        {
            string df = Path.Combine(path, name);
            int idx = 0;
            while (true)
            {
                var nf = Path.Combine(df, string.Join("_", idx++), ext);
                if (File.Exists(nf) == false)
                {
                    File.Move(fullPath, nf);
                    break;
                }
            }
        }

        private void InitUI(int total)
        {
            Invoke(() =>
            {
                txtSuccess.Text = "0";
                txtFail.Text = "0";

                progressBar1.Value = 0;
                progressBar1.Maximum = total;
            });
        }

        private List<string> GetImageFiles(string path)
        {
            return Directory.EnumerateFiles(path)
                .Where(x => imageExtensions.Contains(Path.GetExtension(x)))
                .ToList();
        }

        private void picLeft_Click(object sender, EventArgs e)
        {
            rbLeft.Checked = true;
        }

        private void picDown_Click(object sender, EventArgs e)
        {
            rbBottom.Checked = true;
        }

        private void picRight_Click(object sender, EventArgs e)
        {
            rbRight.Checked = true;
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data == null || !e.Data.GetDataPresent(DataFormats.FileDrop))
                return;

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            foreach (var file in files)
            {
                if (File.Exists(file))
                {
                    string extension = Path.GetExtension(file).ToLower();

                    if (imageExtensions.Contains(extension))
                    {
                        if (!listRotate.Items.Contains(file)) // 중복 방지
                        {
                            listRotate.Items.Add(file);
                        }
                    }
                }
            }
            if (chkAutoRotate.Checked && listRotate.Items.Count > 0)
            {
                btnRotate_Click(null, null);
            }
        }

        private void listBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private async void btnRotate_Click(object sender, EventArgs e)
        {
            if (rbLeft.Checked || rbRight.Checked || rbBottom.Checked)
            {
                Application.DoEvents();
                await Task.Delay(200);
                WaitingPopupManager.ShowForm(this, "변환 중입니다.", "잠시만 기다려주세요.");
                await Task.Run(() =>
                {
                    List<string> originNames = Directory.GetFiles(pathOrigin).Select(x => Path.GetFileName(x)).ToList();
                    for (int idx = listRotate.Items.Count - 1; idx >= 0; idx--)
                    {
                        bool result = ImageRotate(originNames, listRotate.Items[idx].ToString(), rbLeft.Checked ? RotateMode.Rotate270 : rbRight.Checked ? RotateMode.Rotate90 : RotateMode.Rotate180);
                        if (result)
                        {
                            listRotate.Invoke(() =>
                            {
                                listRotate.Items.RemoveAt(idx);
                            });
                        }
                    }
                });
                WaitingPopupManager.CloseForm();
            }
            else
            {
                MessageBox.Show("회전 방향을 선택하세요.", "회전 방향 미선택", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private bool ImageRotate(List<string> originList, string convetedFile, RotateMode rotateMode)
        {
            bool result = false;
            try
            {
                if(rotateMode == RotateMode.Rotate90)
                    rotateMode = RotateMode.Rotate270;
                else if (rotateMode == RotateMode.Rotate180)
                    rotateMode = RotateMode.Rotate180;
                else if (rotateMode == RotateMode.Rotate270)
                    rotateMode = RotateMode.Rotate90;
                string convetedFileTemp = convetedFile + "_temp";
                string convertedFileName = Path.GetFileNameWithoutExtension(convetedFile).Replace("_convert","");
                string originFile = originList.First(x => x.StartsWith(convertedFileName));
                File.Move(convetedFile, convetedFileTemp);
                string originFullPath = Path.Combine(pathOrigin, originFile);
                string tempFile = originFullPath + ".tmp" + Path.GetExtension(originFullPath);
                
                using (Image image = Image.Load(originFullPath))
                {
                    image.Mutate(x =>
                    {
                        x.Rotate(rotateMode);
                    });

                    var exif = image.Metadata.ExifProfile;
                    if (exif != null)
                    {
                        exif.SetValue(ExifTag.Orientation, (ushort)1);
                    }

                    image.Save(tempFile);
                }

                File.Delete(originFullPath);
                File.Move(tempFile, originFullPath);

                ConvertFile(new List<string>() { originFullPath }, false);
                File.Delete(convetedFileTemp);
                result = true;
            }
            catch(Exception ex)
            {

            }
            return result;
        }

        #endregion

        #region 설정 관련
        private void btnPathOpen_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtOutputPath.Text) == false)
                Process.Start("explorer.exe", txtOutputPath.Text);
        }

        private void btnPathSetting_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "전자 액자용 사진 폴더 경로를 선택하세요 [ EX) G:\\사진\\3. 전자 액자 전용 사진 ]";
                dialog.UseDescriptionForTitle = true;
                dialog.ShowNewFolderButton = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = dialog.SelectedPath;
                    txtOutputPath.Text = selectedPath;
                    lbOriginPath.Text = selectedPath;
                    //하위에 origin, convert, fail 폴더 없으면 만들어
                    List<string> lstDic = new List<string>();
                    lstDic.Add(Path.Combine(selectedPath, "origin"));
                    lstDic.Add(Path.Combine(selectedPath, "fail"));
                    lstDic.Add(Path.Combine(selectedPath, "convert"));
                    lstDic.Add(Path.Combine(selectedPath, "duplication"));
                    lstDic.ForEach(dic =>
                    {
                        if (Directory.Exists(dic) == false)
                            Directory.CreateDirectory(dic);
                    });
                    SettingSave();
                }
            }
        }

        private void WidthHeight_ValueChanged(object sender, EventArgs e)
        {
            SettingSave();
        }

        private void rb_CheckedChanged(object sender, EventArgs e)
        {
            SettingSave();
        }

        private void SettingSave()
        {
            SettingHelper.Instance.RootPath = txtOutputPath.Text;
            SettingHelper.Instance.PhotoRootPath = txtPhotoAllRoot.Text;
            SettingHelper.Instance.ConvertWidth = (int)nudWidth.Value;
            SettingHelper.Instance.ConvertHeight = (int)nudHeight.Value;
            SettingHelper.Instance.IsAutoConvert = rbAuto.Checked;
            SettingHelper.Instance.Save();
        }

        private void btnPhotoPathSetting_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "사진 모음 폴더 루트 경로를 선택하세요 [ EX) G:\\사진\\1. 사진모음 ]";
                dialog.UseDescriptionForTitle = true;
                dialog.ShowNewFolderButton = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    bool isOk = false;
                    string selectedPath = dialog.SelectedPath;
                    string[] dics = Directory.GetDirectories(selectedPath);
                    foreach (string dic in dics)
                    {
                        string d = Path.GetFileName(dic);
                        if (!string.IsNullOrWhiteSpace(d) && d.Length == 4 && int.TryParse(d, out int number) && number >= 2000 && number <= 2100)
                        {
                            isOk = true;
                            break;
                        }
                    }

                    if (isOk)
                    {
                        txtPhotoAllRoot.Text = selectedPath;
                        SettingSave();
                    }
                }
            }
        }

        private void btnPhotoPathOpen_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPhotoAllRoot.Text) == false)
                Process.Start("explorer.exe", txtPhotoAllRoot.Text);
        }
        #endregion

        #region 공통
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (btnStart.Enabled == false)
            {
                tabControl1.SelectedTab = tabPage1;
                tabControl1.Focus();
                return;
            }
            if (tabControl1.SelectedTab != tabPage2)
            {
                PathCheck();
            }
        }

        private void PathCheck()
        {
            if (string.IsNullOrEmpty(SettingHelper.Instance.RootPath))
            {
                tabControl1.SelectedTab = tabPage2;
                MessageBox.Show("전자 액자용 사진 폴더 경로가 비어 있습니다. 반드시 설정해야 하는 값입니다.");
                btnPathSetting_Click(null, null);
            }
            else if (string.IsNullOrEmpty(SettingHelper.Instance.PhotoRootPath))
            {
                tabControl1.SelectedTab = tabPage2;
                MessageBox.Show("사진 모음 폴더 루트 경로가 비어 있습니다. 반드시 설정해야 하는 값입니다.");
                btnPhotoPathSetting_Click(null, null);
            }
        }

        private void PreSetSetting()
        {
            txtPhotoAllRoot.Text = SettingHelper.Instance.PhotoRootPath;
            txtOutputPath.Text = SettingHelper.Instance.RootPath;
            nudHeight.Value = SettingHelper.Instance.ConvertHeight;
            nudWidth.Value = SettingHelper.Instance.ConvertWidth;
            if (SettingHelper.Instance.IsAutoConvert)
                rbAuto.Checked = true;
            else
                rbManual.Checked = true;
            lbOriginPath.Text = SettingHelper.Instance.RootPath;
        }
        #endregion

    }
}
