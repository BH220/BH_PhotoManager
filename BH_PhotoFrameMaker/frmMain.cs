using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using BH_WaitingPopupWinform; 
using System.Diagnostics;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Image = SixLabors.ImageSharp.Image;
using SixLabors.ImageSharp.Formats.Jpeg;

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
                    CopyToOrigin(lstTarget.Items[idx].ToString());
                    lstTarget.Invoke(() =>
                    {
                        lstTarget.Items.RemoveAt(idx);
                    });
                }
            });
            WaitingPopupManager.CloseForm();
        }

        private void CopyToOrigin(string path)
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
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

        private void lstTarget_DragDrop(object sender, DragEventArgs e)
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
                btnStart.Enabled = true;
            }
        }

        private void ConvertFile(List<string> files)
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
                        MoveFile(file, pathDuplication, fileName, ext);
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
                dialog.Description = "전자 액자용 사진 폴더 경로를 선택하세요";
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
                dialog.Description = "사진 모음 폴더 루트 경로를 선택하세요";
                dialog.UseDescriptionForTitle = true;
                dialog.ShowNewFolderButton = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    bool isOk = false;
                    string selectedPath = dialog.SelectedPath;
                    string[] dics = Directory.GetDirectories(selectedPath);
                    foreach (string dic in dics)
                    {
                        //바로 하위 폴더의 목록이 년도인게 있는지 체크 0000 ~ 2100 사이 인지 

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
            if(btnStart.Enabled == false)
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
