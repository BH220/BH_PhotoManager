
using ImageMagick;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace BH_PhotoFrameMaker
{
    public partial class frmMain : Form
    {
        List<string> targetExtension = new List<string>();
        List<ProcessData> lstTargetData = new List<ProcessData>();
        CancellationTokenSource cts = null;

        public frmMain()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            targetExtension.Add(".jpg");
            targetExtension.Add(".jpeg");
            targetExtension.Add(".png");
            targetExtension.Add(".mp4");
            targetExtension.Add(".gif");
            CreateOutputFolder();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
        }

        private void txtNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 숫자(0~9) 또는 백스페이스만 허용
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // 입력 무시
            }
        }

        private void btnPathOpen_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(txtOutputPath.Text) == false)
            {
                Directory.CreateDirectory(txtOutputPath.Text); // 폴더가 없으면 생성
            }
            if (string.IsNullOrEmpty(txtOutputPath.Text) == false)
            {
                Process.Start("explorer.exe", txtOutputPath.Text);
            }
        }

        private void btnPathSetting_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowser = new FolderBrowserDialog())
            {
                if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    txtOutputPath.Text = folderBrowser.SelectedPath + "\\"; // 선택한 경로를 텍스트 박스에 설정
                }
            }
        }

        private void txtFail_Click(object sender, EventArgs e)
        {

        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(txtOutputPath.Text) == false)
            {
                Directory.CreateDirectory(txtOutputPath.Text); // 폴더가 없으면 생성
            }
            SetEnabled(false);
            cts = new CancellationTokenSource();
            await Task.Run(() =>
            {
                ConvertImage();
            });
        }

        private void SetEnabled(bool enabled)
        {
            lstTarget.Enabled =
            btnStart.Enabled =
            btnPathOpen.Enabled =
            btnPathSetting.Enabled =
            txtWidth.Enabled =
            txtHeight.Enabled =
            contextMenuStrip1.Enabled =
            btnSelect.Enabled = enabled;
        }

        private void ConvertImage()
        {
            Thread.Sleep(3000);
            int success = 0;
            int fail = 0;
            int percent = 0;
            foreach (string file in lstTarget.Items)
            {
                this.Invoke(new Action(() =>
                {
                    percent = (int)((success + fail + 1) * 100 / lstTarget.Items.Count);
                    if (percent >= 100) percent = 100;
                    else if (percent <= 0) percent = 0;
                    progressBar1.Value = percent;
                    lbPercent.Text = $"진행률 ({percent}%)";
                }));
                if (cts.IsCancellationRequested)
                {
                    break; // 취소 요청이 있으면 중단
                }
                ProcessData data = new ProcessData(file);
                try
                {
                    string now = DateTime.Now.ToString("yyMMdd_HHmmss_fff");
                    string outputFile = txtOutputPath.Text + $"{now}.jpg";
                    FileInfo fifo = new FileInfo(file);
                    ResizeAndCompress(file, outputFile, fifo.Length);
                    data.IsSuccess = true;
                    success++;
                }
                catch (Exception ex)
                {
                    data.IsSuccess = false;
                    fail++;
                    data.ErrorMessage = ex.Message;
                }
                finally
                {
                    this.Invoke(new Action(() =>
                    {
                        txtSuccess.Text = success.ToString("#,#");
                        txtFail.Text = fail.ToString("#,#");
                    }));
                    lstTargetData.Add(data);
                }
            }
            this.Invoke(new Action(() =>
            {
                SetEnabled(true);
            }));
        }

        public void ResizeAndCompress(string inputPath, string outputPath, long originSize)
        {
            uint maxWidth = Convert.ToUInt32(txtWidth.Text);
            uint maxHeight = Convert.ToUInt32(txtHeight.Text);
            uint quality = 70; // JPEG 품질 설정 (70~85 권장)
            using (var image = new MagickImage(inputPath))
            {
                image.AutoOrient(); // 이미지 방향 자동 조정 
                // 원본 해상도 저장
                int originalWidth = (int)image.Width;
                int originalHeight = (int)image.Height;

                // 조건: 원본 해상도가 더 클 경우 리사이즈
                if (originalWidth > maxWidth || originalHeight > maxHeight)
                {
                    var geometry = new MagickGeometry((uint)maxWidth, (uint)maxHeight)
                    {
                        IgnoreAspectRatio = false // 비율 유지
                    };
                    image.Resize(geometry);
                }

                // 메타데이터 제거 (용량 절감)
                image.Strip();

                // JPEG 압축 설정
                image.Format = MagickFormat.Jpeg;
                image.Quality = quality;                  // 70~85 권장
                //image.Interlace = Interlace.Plane;        // progressive 저장 (웹 최적화)

                // 저장
                image.Write(outputPath);
            }
            FileInfo outputFileInfo = new FileInfo(outputPath);
            if(originSize<outputFileInfo.Length)
            {
                // 원본 크기보다 커지면 삭제
                File.Delete(outputPath);
                File.Copy(inputPath, outputPath);
            }
        }

        private void btnAddFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog frm = new OpenFileDialog())
            {
                frm.Filter = "이미지 및 동영상 (*.jpg;*.jpeg;*.png;*.gif;*.mp4)|*.jpg;*.jpeg;*.png;*.gif;*.mp4";
                frm.Multiselect = true; // 다중 선택 허용
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    AddFiles(frm.FileNames);
                }
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (lstTarget.SelectedIndices.Count == lstTarget.Items.Count)
                lstTarget.ClearSelected(); // 전체 선택 해제
            else
            {
                for (int i = 0; i < lstTarget.Items.Count; i++)
                {
                    lstTarget.SetSelected(i, true);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            for (int i = lstTarget.SelectedIndices.Count - 1; i >= 0; i--)
            {
                lstTarget.Items.RemoveAt(lstTarget.SelectedIndices[i]);
            }
            txtTotal.Text = lstTarget.Items.Count.ToString("#,#");
        }

        private void btnAddFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog frm = new FolderBrowserDialog())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    // 모든 파일 경로 가져오기 (하위 폴더 포함)
                    AddFolder(frm.SelectedPath);
                }
            }
        }

        private void AddFolder(string selectedPath)
        {
            string[] allFiles = Directory.GetFiles(selectedPath, "*.*", SearchOption.AllDirectories);
            AddFiles(allFiles);
        }

        private void lstTarget_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void lstTarget_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                List<string> targetFiles = new List<string>();

                foreach (string f in files)
                {
                    if (string.IsNullOrEmpty(Path.GetExtension(f)))
                        AddFolder(f);
                    else
                        targetFiles.Add(f);
                }
                if (targetFiles.Count > 0)
                    AddFiles(targetFiles.ToArray());
            }
        }

        private void AddFiles(string[] files)
        {
            foreach (string file in files)
            {
                if (targetExtension.Contains(Path.GetExtension(file).ToLower()))
                {
                    if (!lstTarget.Items.Contains(file))
                    {
                        lstTarget.Items.Add(file); // 파일 경로 추가
                    }
                }
            }
            txtTotal.Text = lstTarget.Items.Count.ToString("#,#");
        }

        private void lstTarget_MouseDown(object sender, MouseEventArgs e)
        {
            int index = lstTarget.IndexFromPoint(e.Location);

            // 인덱스가 -1이면 빈 공간 클릭한 것
            if (index == ListBox.NoMatches)
            {
                lstTarget.ClearSelected();
            }
        }

        private void lstTarget_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            // 홀수/짝수에 따라 배경색 설정
            bool isEven = (e.Index % 2 == 0);
            Color backColor = isEven ? Color.White : Color.Gainsboro;

            // 선택된 경우는 선택 색상 사용
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                backColor = SystemColors.Highlight;
            }

            using (SolidBrush backgroundBrush = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(backgroundBrush, e.Bounds);
            }

            // 텍스트 그리기
            string text = lstTarget.Items[e.Index].ToString();
            Color textColor = ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                ? SystemColors.HighlightText
                : lstTarget.ForeColor;

            TextRenderer.DrawText(
                e.Graphics,
                text,
                e.Font,
                e.Bounds,
                textColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
        }

        private void lstTarget_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                btnDelete_Click(sender, e);
            }
            else if (e.KeyCode == Keys.A && e.Control)
            {
                for (int i = 0; i < lstTarget.Items.Count; i++)
                {
                    lstTarget.SetSelected(i, true);
                }
            }
        }

        private void txtSuccess_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnResult_Click(object sender, EventArgs e)
        {
            if (lstTargetData.Count > 0)
            {
                frmError frm = new frmError(lstTargetData);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("처리 결과가 없습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            lstTarget.Items.Clear();
            lstTargetData.Clear();
            txtTotal.Text = "";
            txtSuccess.Text = "";
            txtFail.Text = "";
            progressBar1.Value = 0;
            CreateOutputFolder();
        }

        private void CreateOutputFolder()
        {
            txtOutputPath.Text = Application.StartupPath + "photo_frame\\" + DateTime.Now.ToString("yyMMdd_HHmmss") + "\\";
        }
    }
}
