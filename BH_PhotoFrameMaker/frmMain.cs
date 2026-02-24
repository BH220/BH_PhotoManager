
using BH_WaitingPopupWinform;
using ImageMagick;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace BH_PhotoFrameMaker
{
    public partial class frmMain : Form
    {
        CancellationTokenSource cts = null;
        string[] allowedExtensions = { ".png", ".jpg", ".jpeg", ".gif" };

        public frmMain()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
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
        private void btnConvertCopy_Click(object sender, EventArgs e)
        {
            WaitingPopupManager.ShowForm(this, "변환 중입니다.", "잠시만 기다려주세요.");
            string rootOrigin = Path.Combine(SettingHelper.Instance.RootPath, "origin");
            if (Directory.Exists(rootOrigin) == false)
                Directory.CreateDirectory(rootOrigin);

            for (int idx = lstTarget.Items.Count - 1; idx >= 0; idx--)
            {
                Thread.Sleep(1000);
                lstTarget.Items.RemoveAt(idx);
            }
            WaitingPopupManager.CloseForm();
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

                    if (allowedExtensions.Contains(extension))
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
        private void btnCheck_Click(object sender, EventArgs e)
        {

        }

        private void btnReset_Click(object sender, EventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {

        }

        private void btnResult_Click(object sender, EventArgs e)
        {

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
                dialog.Description = "폴더를 선택하세요.";
                dialog.UseDescriptionForTitle = true;
                dialog.ShowNewFolderButton = true;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = dialog.SelectedPath;

                    txtOutputPath.Text = selectedPath;
                    lbOriginPath.Text = selectedPath;
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
            SettingHelper.Instance.ConvertWidth = (int)nudWidth.Value;
            SettingHelper.Instance.ConvertHeight = (int)nudHeight.Value;
            SettingHelper.Instance.IsAutoConvert = rbAuto.Checked;
            SettingHelper.Instance.Save();
        }
        #endregion

        #region 공통
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
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

        }
        #endregion

    }
}
