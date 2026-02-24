namespace BH_PhotoFrameMaker
{
    partial class frmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            contextMenuStrip1 = new ContextMenuStrip(components);
            btnAddFile = new ToolStripMenuItem();
            btnAddFolder = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            btnSelect = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            btnDelete = new ToolStripMenuItem();
            tabControl1 = new TabControl();
            tabPage3 = new TabPage();
            groupBox1 = new GroupBox();
            lstTarget = new ListBox();
            panel1 = new Panel();
            lbOriginPath = new LinkLabel();
            btnItemDeleteAll = new Button();
            button1 = new Button();
            btnItemDelete = new Button();
            btnConvertCopy = new Button();
            label8 = new Label();
            label3 = new Label();
            tabPage1 = new TabPage();
            btnCheck = new Button();
            btnReset = new Button();
            btnResult = new Button();
            btnStart = new Button();
            label7 = new Label();
            txtFail = new TextBox();
            label5 = new Label();
            txtSuccess = new TextBox();
            progressBar1 = new ProgressBar();
            lbPercent = new Label();
            label6 = new Label();
            txtTotal = new TextBox();
            tabPage2 = new TabPage();
            rbManual = new RadioButton();
            rbAuto = new RadioButton();
            nudHeight = new NumericUpDown();
            nudWidth = new NumericUpDown();
            btnPathOpen = new Button();
            btnPathSetting = new Button();
            label4 = new Label();
            txtOutputPath = new TextBox();
            label2 = new Label();
            label9 = new Label();
            label1 = new Label();
            contextMenuStrip1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage3.SuspendLayout();
            groupBox1.SuspendLayout();
            panel1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudHeight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudWidth).BeginInit();
            SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { btnAddFile, btnAddFolder, toolStripSeparator1, btnSelect, toolStripSeparator2, btnDelete });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(156, 104);
            // 
            // btnAddFile
            // 
            btnAddFile.Name = "btnAddFile";
            btnAddFile.Size = new Size(155, 22);
            btnAddFile.Text = "파일 추가";
            // 
            // btnAddFolder
            // 
            btnAddFolder.Name = "btnAddFolder";
            btnAddFolder.Size = new Size(155, 22);
            btnAddFolder.Text = "폴더 추가";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(152, 6);
            // 
            // btnSelect
            // 
            btnSelect.Name = "btnSelect";
            btnSelect.Size = new Size(155, 22);
            btnSelect.Text = "전체 선택/해제";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(152, 6);
            // 
            // btnDelete
            // 
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(155, 22);
            btnDelete.Text = "삭제";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(5, 5);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(494, 391);
            tabControl1.TabIndex = 10;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(groupBox1);
            tabPage3.Controls.Add(panel1);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(486, 363);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "1. 전자액자용 이미지 모으기";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(lstTarget);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(480, 257);
            groupBox1.TabIndex = 9;
            groupBox1.TabStop = false;
            groupBox1.Text = " 변환 대상 파일을 드래그 앤 드랍하여 추가 ";
            // 
            // lstTarget
            // 
            lstTarget.AllowDrop = true;
            lstTarget.Dock = DockStyle.Fill;
            lstTarget.FormattingEnabled = true;
            lstTarget.ItemHeight = 15;
            lstTarget.Location = new Point(3, 19);
            lstTarget.Name = "lstTarget";
            lstTarget.SelectionMode = SelectionMode.MultiExtended;
            lstTarget.Size = new Size(474, 235);
            lstTarget.TabIndex = 7;
            lstTarget.DragDrop += lstTarget_DragDrop;
            lstTarget.DragEnter += lstTarget_DragEnter;
            // 
            // panel1
            // 
            panel1.Controls.Add(lbOriginPath);
            panel1.Controls.Add(btnItemDeleteAll);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(btnItemDelete);
            panel1.Controls.Add(btnConvertCopy);
            panel1.Controls.Add(label8);
            panel1.Controls.Add(label3);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(3, 260);
            panel1.Name = "panel1";
            panel1.Size = new Size(480, 100);
            panel1.TabIndex = 12;
            // 
            // lbOriginPath
            // 
            lbOriginPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            lbOriginPath.Location = new Point(36, 60);
            lbOriginPath.Name = "lbOriginPath";
            lbOriginPath.Size = new Size(441, 15);
            lbOriginPath.TabIndex = 12;
            lbOriginPath.LinkClicked += lbOriginPath_LinkClicked;
            // 
            // btnItemDeleteAll
            // 
            btnItemDeleteAll.Location = new Point(237, 3);
            btnItemDeleteAll.Name = "btnItemDeleteAll";
            btnItemDeleteAll.Size = new Size(111, 32);
            btnItemDeleteAll.TabIndex = 11;
            btnItemDeleteAll.Text = "모든 항목 삭제";
            btnItemDeleteAll.UseVisualStyleBackColor = true;
            btnItemDeleteAll.Click += btnItemDeleteAll_Click;
            // 
            // button1
            // 
            button1.Location = new Point(237, 6);
            button1.Name = "button1";
            button1.Size = new Size(111, 32);
            button1.TabIndex = 11;
            button1.Text = "선택 항목 삭제";
            button1.UseVisualStyleBackColor = true;
            button1.Click += btnItemDelete_Click;
            // 
            // btnItemDelete
            // 
            btnItemDelete.Location = new Point(120, 3);
            btnItemDelete.Name = "btnItemDelete";
            btnItemDelete.Size = new Size(111, 32);
            btnItemDelete.TabIndex = 11;
            btnItemDelete.Text = "선택 항목 삭제";
            btnItemDelete.UseVisualStyleBackColor = true;
            btnItemDelete.Click += btnItemDelete_Click;
            // 
            // btnConvertCopy
            // 
            btnConvertCopy.Location = new Point(3, 3);
            btnConvertCopy.Name = "btnConvertCopy";
            btnConvertCopy.Size = new Size(111, 32);
            btnConvertCopy.TabIndex = 11;
            btnConvertCopy.Text = "변환 하여 복사";
            btnConvertCopy.UseVisualStyleBackColor = true;
            btnConvertCopy.Click += btnConvertCopy_Click;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("맑은 고딕", 9F);
            label8.Location = new Point(3, 60);
            label8.Name = "label8";
            label8.Size = new Size(34, 15);
            label8.TabIndex = 10;
            label8.Text = "경로:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("맑은 고딕", 9F);
            label3.Location = new Point(3, 38);
            label3.Name = "label3";
            label3.Size = new Size(429, 15);
            label3.TabIndex = 10;
            label3.Text = "변환된 파일은 아래 경로에 [ yyyy-MM-dd_원본파일명 ] 의 형태로 저장됩니다.";
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(btnCheck);
            tabPage1.Controls.Add(btnReset);
            tabPage1.Controls.Add(btnResult);
            tabPage1.Controls.Add(btnStart);
            tabPage1.Controls.Add(label7);
            tabPage1.Controls.Add(txtFail);
            tabPage1.Controls.Add(label5);
            tabPage1.Controls.Add(txtSuccess);
            tabPage1.Controls.Add(progressBar1);
            tabPage1.Controls.Add(lbPercent);
            tabPage1.Controls.Add(label6);
            tabPage1.Controls.Add(txtTotal);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(486, 363);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "2. 액자용으로 컨버팅";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnCheck
            // 
            btnCheck.Location = new Point(6, 6);
            btnCheck.Name = "btnCheck";
            btnCheck.Size = new Size(100, 32);
            btnCheck.TabIndex = 21;
            btnCheck.Text = "전환 대상 확인";
            btnCheck.UseVisualStyleBackColor = true;
            btnCheck.Click += btnCheck_Click;
            // 
            // btnReset
            // 
            btnReset.Location = new Point(112, 6);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(89, 32);
            btnReset.TabIndex = 20;
            btnReset.Text = "초기화";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click;
            // 
            // btnResult
            // 
            btnResult.Location = new Point(207, 122);
            btnResult.Name = "btnResult";
            btnResult.Size = new Size(89, 32);
            btnResult.TabIndex = 19;
            btnResult.Text = "결과 보기";
            btnResult.UseVisualStyleBackColor = true;
            btnResult.Click += btnResult_Click;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(112, 122);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(89, 32);
            btnStart.TabIndex = 18;
            btnStart.Text = "변환 시작";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(397, 53);
            label7.Name = "label7";
            label7.Size = new Size(12, 15);
            label7.TabIndex = 17;
            label7.Text = "/";
            label7.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtFail
            // 
            txtFail.Location = new Point(415, 49);
            txtFail.Name = "txtFail";
            txtFail.ReadOnly = true;
            txtFail.Size = new Size(65, 23);
            txtFail.TabIndex = 16;
            txtFail.TextAlign = HorizontalAlignment.Right;
            // 
            // label5
            // 
            label5.Location = new Point(231, 49);
            label5.Name = "label5";
            label5.Size = new Size(89, 23);
            label5.TabIndex = 15;
            label5.Text = "성공/실패";
            label5.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtSuccess
            // 
            txtSuccess.Location = new Point(326, 49);
            txtSuccess.Name = "txtSuccess";
            txtSuccess.ReadOnly = true;
            txtSuccess.Size = new Size(65, 23);
            txtSuccess.TabIndex = 14;
            txtSuccess.TextAlign = HorizontalAlignment.Right;
            // 
            // progressBar1
            // 
            progressBar1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            progressBar1.Location = new Point(112, 84);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(368, 23);
            progressBar1.TabIndex = 13;
            // 
            // lbPercent
            // 
            lbPercent.Location = new Point(6, 84);
            lbPercent.Name = "lbPercent";
            lbPercent.Size = new Size(100, 23);
            lbPercent.TabIndex = 12;
            lbPercent.Text = "진행률 (0%)";
            lbPercent.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            label6.Location = new Point(6, 48);
            label6.Name = "label6";
            label6.Size = new Size(100, 23);
            label6.TabIndex = 9;
            label6.Text = "총 대상 수";
            label6.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtTotal
            // 
            txtTotal.Location = new Point(112, 48);
            txtTotal.Name = "txtTotal";
            txtTotal.ReadOnly = true;
            txtTotal.Size = new Size(65, 23);
            txtTotal.TabIndex = 8;
            txtTotal.TextAlign = HorizontalAlignment.Right;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(rbManual);
            tabPage2.Controls.Add(rbAuto);
            tabPage2.Controls.Add(nudHeight);
            tabPage2.Controls.Add(nudWidth);
            tabPage2.Controls.Add(btnPathOpen);
            tabPage2.Controls.Add(btnPathSetting);
            tabPage2.Controls.Add(label4);
            tabPage2.Controls.Add(txtOutputPath);
            tabPage2.Controls.Add(label2);
            tabPage2.Controls.Add(label9);
            tabPage2.Controls.Add(label1);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(486, 363);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "설정";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // rbManual
            // 
            rbManual.AutoSize = true;
            rbManual.Location = new Point(102, 184);
            rbManual.Name = "rbManual";
            rbManual.Size = new Size(73, 19);
            rbManual.TabIndex = 11;
            rbManual.Text = "수동변환";
            rbManual.UseVisualStyleBackColor = true;
            rbManual.CheckedChanged += rb_CheckedChanged;
            // 
            // rbAuto
            // 
            rbAuto.AutoSize = true;
            rbAuto.Checked = true;
            rbAuto.Location = new Point(23, 184);
            rbAuto.Name = "rbAuto";
            rbAuto.Size = new Size(73, 19);
            rbAuto.TabIndex = 11;
            rbAuto.TabStop = true;
            rbAuto.Text = "자동변환";
            rbAuto.UseVisualStyleBackColor = true;
            rbAuto.CheckedChanged += rb_CheckedChanged;
            // 
            // nudHeight
            // 
            nudHeight.Location = new Point(99, 103);
            nudHeight.Maximum = new decimal(new int[] { 2160, 0, 0, 0 });
            nudHeight.Minimum = new decimal(new int[] { 480, 0, 0, 0 });
            nudHeight.Name = "nudHeight";
            nudHeight.Size = new Size(51, 23);
            nudHeight.TabIndex = 10;
            nudHeight.TextAlign = HorizontalAlignment.Center;
            nudHeight.Value = new decimal(new int[] { 1080, 0, 0, 0 });
            nudHeight.ValueChanged += WidthHeight_ValueChanged;
            // 
            // nudWidth
            // 
            nudWidth.Location = new Point(23, 103);
            nudWidth.Maximum = new decimal(new int[] { 3840, 0, 0, 0 });
            nudWidth.Minimum = new decimal(new int[] { 720, 0, 0, 0 });
            nudWidth.Name = "nudWidth";
            nudWidth.Size = new Size(51, 23);
            nudWidth.TabIndex = 10;
            nudWidth.TextAlign = HorizontalAlignment.Center;
            nudWidth.Value = new decimal(new int[] { 1920, 0, 0, 0 });
            nudWidth.ValueChanged += WidthHeight_ValueChanged;
            // 
            // btnPathOpen
            // 
            btnPathOpen.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPathOpen.Location = new Point(23, 29);
            btnPathOpen.Name = "btnPathOpen";
            btnPathOpen.Size = new Size(49, 23);
            btnPathOpen.TabIndex = 9;
            btnPathOpen.Text = "열기";
            btnPathOpen.UseVisualStyleBackColor = true;
            btnPathOpen.Click += btnPathOpen_Click;
            // 
            // btnPathSetting
            // 
            btnPathSetting.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPathSetting.Location = new Point(78, 29);
            btnPathSetting.Name = "btnPathSetting";
            btnPathSetting.Size = new Size(49, 23);
            btnPathSetting.TabIndex = 8;
            btnPathSetting.Text = "설정";
            btnPathSetting.UseVisualStyleBackColor = true;
            btnPathSetting.Click += btnPathSetting_Click;
            // 
            // label4
            // 
            label4.Location = new Point(6, 3);
            label4.Name = "label4";
            label4.Size = new Size(172, 23);
            label4.TabIndex = 7;
            label4.Text = "■ 전자 액자용 사진 폴더 경로";
            label4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtOutputPath
            // 
            txtOutputPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtOutputPath.Location = new Point(133, 29);
            txtOutputPath.Name = "txtOutputPath";
            txtOutputPath.ReadOnly = true;
            txtOutputPath.Size = new Size(347, 23);
            txtOutputPath.TabIndex = 6;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(80, 105);
            label2.Name = "label2";
            label2.Size = new Size(13, 15);
            label2.TabIndex = 4;
            label2.Text = "x";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            label9.Location = new Point(6, 158);
            label9.Name = "label9";
            label9.Size = new Size(379, 23);
            label9.TabIndex = 2;
            label9.Text = "■ 전자액자용 이미지 모음에서 드래그앤 드랍시 자동 변환 처리";
            label9.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            label1.Location = new Point(6, 77);
            label1.Name = "label1";
            label1.Size = new Size(100, 23);
            label1.TabIndex = 2;
            label1.Text = "■ 기준 해상도";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(504, 401);
            Controls.Add(tabControl1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(520, 440);
            Name = "frmMain";
            Padding = new Padding(5);
            Text = "이미지 변환";
            contextMenuStrip1.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage3.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudHeight).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudWidth).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem btnAddFile;
        private ToolStripMenuItem btnAddFolder;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem btnSelect;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem btnDelete;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Label label4;
        private TextBox txtOutputPath;
        private Label label2;
        private Label label1;
        private Button btnPathSetting;
        private Button btnPathOpen;
        private Label lbPercent;
        private Label label6;
        private TextBox txtTotal;
        private Label label7;
        private TextBox txtFail;
        private Label label5;
        private TextBox txtSuccess;
        private ProgressBar progressBar1;
        private Button btnStart;
        private Button btnResult;
        private Button btnReset;
        private TabPage tabPage3;
        private Button btnCheck;
        private Panel panel1;
        private Button btnConvertCopy;
        private Label label3;
        private GroupBox groupBox1;
        private ListBox lstTarget;
        private LinkLabel lbOriginPath;
        private Label label8;
        private NumericUpDown nudHeight;
        private NumericUpDown nudWidth;
        private RadioButton rbManual;
        private RadioButton rbAuto;
        private Label label9;
        private Button btnItemDelete;
        private Button btnItemDeleteAll;
        private Button button1;
    }
}
