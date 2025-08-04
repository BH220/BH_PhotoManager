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
            groupBox1 = new GroupBox();
            lstTarget = new ListBox();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
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
            btnPathOpen = new Button();
            btnPathSetting = new Button();
            label4 = new Label();
            txtOutputPath = new TextBox();
            label2 = new Label();
            label1 = new Label();
            txtHeight = new TextBox();
            txtWidth = new TextBox();
            contextMenuStrip1.SuspendLayout();
            groupBox1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
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
            btnAddFile.Click += btnAddFile_Click;
            // 
            // btnAddFolder
            // 
            btnAddFolder.Name = "btnAddFolder";
            btnAddFolder.Size = new Size(155, 22);
            btnAddFolder.Text = "폴더 추가";
            btnAddFolder.Click += btnAddFolder_Click;
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
            btnSelect.Click += btnSelect_Click;
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
            btnDelete.Click += btnDelete_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(lstTarget);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(5, 5);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(494, 254);
            groupBox1.TabIndex = 9;
            groupBox1.TabStop = false;
            groupBox1.Text = " 변환 대상 목록 ( 마우스 우클릭 하여 메뉴 표시, 파일 드래그 앤 드랍 하여 추가 ) ";
            // 
            // lstTarget
            // 
            lstTarget.AllowDrop = true;
            lstTarget.ContextMenuStrip = contextMenuStrip1;
            lstTarget.Dock = DockStyle.Fill;
            lstTarget.DrawMode = DrawMode.OwnerDrawFixed;
            lstTarget.FormattingEnabled = true;
            lstTarget.ItemHeight = 15;
            lstTarget.Location = new Point(3, 19);
            lstTarget.Name = "lstTarget";
            lstTarget.SelectionMode = SelectionMode.MultiExtended;
            lstTarget.Size = new Size(488, 232);
            lstTarget.TabIndex = 7;
            lstTarget.DrawItem += lstTarget_DrawItem;
            lstTarget.DragDrop += lstTarget_DragDrop;
            lstTarget.DragEnter += lstTarget_DragEnter;
            lstTarget.KeyDown += lstTarget_KeyDown;
            lstTarget.MouseDown += lstTarget_MouseDown;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = DockStyle.Bottom;
            tabControl1.Location = new Point(5, 259);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(494, 137);
            tabControl1.TabIndex = 10;
            // 
            // tabPage1
            // 
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
            tabPage1.Size = new Size(486, 109);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "변환";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnReset
            // 
            btnReset.Location = new Point(274, 80);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(75, 23);
            btnReset.TabIndex = 20;
            btnReset.Text = "초기화";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click;
            // 
            // btnResult
            // 
            btnResult.Location = new Point(193, 80);
            btnResult.Name = "btnResult";
            btnResult.Size = new Size(75, 23);
            btnResult.TabIndex = 19;
            btnResult.Text = "결과 보기";
            btnResult.UseVisualStyleBackColor = true;
            btnResult.Click += btnResult_Click;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(112, 80);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(75, 23);
            btnStart.TabIndex = 18;
            btnStart.Text = "변환 시작";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(397, 11);
            label7.Name = "label7";
            label7.Size = new Size(12, 15);
            label7.TabIndex = 17;
            label7.Text = "/";
            label7.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtFail
            // 
            txtFail.Location = new Point(415, 7);
            txtFail.Name = "txtFail";
            txtFail.ReadOnly = true;
            txtFail.Size = new Size(65, 23);
            txtFail.TabIndex = 16;
            txtFail.TextAlign = HorizontalAlignment.Right;
            txtFail.Click += txtFail_Click;
            // 
            // label5
            // 
            label5.Location = new Point(231, 7);
            label5.Name = "label5";
            label5.Size = new Size(89, 23);
            label5.TabIndex = 15;
            label5.Text = "성공/실패";
            label5.TextAlign = ContentAlignment.MiddleRight;
            // 
            // txtSuccess
            // 
            txtSuccess.Location = new Point(326, 7);
            txtSuccess.Name = "txtSuccess";
            txtSuccess.ReadOnly = true;
            txtSuccess.Size = new Size(65, 23);
            txtSuccess.TabIndex = 14;
            txtSuccess.TextAlign = HorizontalAlignment.Right;
            txtSuccess.TextChanged += txtSuccess_TextChanged;
            // 
            // progressBar1
            // 
            progressBar1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            progressBar1.Location = new Point(112, 42);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(368, 23);
            progressBar1.TabIndex = 13;
            // 
            // lbPercent
            // 
            lbPercent.Location = new Point(6, 42);
            lbPercent.Name = "lbPercent";
            lbPercent.Size = new Size(100, 23);
            lbPercent.TabIndex = 12;
            lbPercent.Text = "진행률 (0%)";
            lbPercent.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            label6.Location = new Point(6, 6);
            label6.Name = "label6";
            label6.Size = new Size(100, 23);
            label6.TabIndex = 9;
            label6.Text = "총 대상 수";
            label6.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtTotal
            // 
            txtTotal.Location = new Point(112, 6);
            txtTotal.Name = "txtTotal";
            txtTotal.ReadOnly = true;
            txtTotal.Size = new Size(65, 23);
            txtTotal.TabIndex = 8;
            txtTotal.TextAlign = HorizontalAlignment.Right;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(btnPathOpen);
            tabPage2.Controls.Add(btnPathSetting);
            tabPage2.Controls.Add(label4);
            tabPage2.Controls.Add(txtOutputPath);
            tabPage2.Controls.Add(label2);
            tabPage2.Controls.Add(label1);
            tabPage2.Controls.Add(txtHeight);
            tabPage2.Controls.Add(txtWidth);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(486, 109);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "설정";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnPathOpen
            // 
            btnPathOpen.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPathOpen.Location = new Point(373, 42);
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
            btnPathSetting.Location = new Point(428, 42);
            btnPathSetting.Name = "btnPathSetting";
            btnPathSetting.Size = new Size(49, 23);
            btnPathSetting.TabIndex = 8;
            btnPathSetting.Text = "설정";
            btnPathSetting.UseVisualStyleBackColor = true;
            btnPathSetting.Click += btnPathSetting_Click;
            // 
            // label4
            // 
            label4.Location = new Point(6, 42);
            label4.Name = "label4";
            label4.Size = new Size(100, 23);
            label4.TabIndex = 7;
            label4.Text = "출력 경로";
            label4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtOutputPath
            // 
            txtOutputPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtOutputPath.Location = new Point(112, 42);
            txtOutputPath.Name = "txtOutputPath";
            txtOutputPath.ReadOnly = true;
            txtOutputPath.Size = new Size(255, 23);
            txtOutputPath.TabIndex = 6;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(165, 10);
            label2.Name = "label2";
            label2.Size = new Size(13, 15);
            label2.TabIndex = 4;
            label2.Text = "x";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            label1.Location = new Point(6, 6);
            label1.Name = "label1";
            label1.Size = new Size(100, 23);
            label1.TabIndex = 2;
            label1.Text = "기준 해상도";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtHeight
            // 
            txtHeight.Location = new Point(184, 6);
            txtHeight.Name = "txtHeight";
            txtHeight.Size = new Size(47, 23);
            txtHeight.TabIndex = 1;
            txtHeight.Text = "1080";
            txtHeight.TextAlign = HorizontalAlignment.Center;
            txtHeight.KeyPress += txtNum_KeyPress;
            // 
            // txtWidth
            // 
            txtWidth.Location = new Point(112, 6);
            txtWidth.Name = "txtWidth";
            txtWidth.Size = new Size(47, 23);
            txtWidth.TabIndex = 0;
            txtWidth.Text = "1920";
            txtWidth.TextAlign = HorizontalAlignment.Center;
            txtWidth.KeyPress += txtNum_KeyPress;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(504, 401);
            Controls.Add(groupBox1);
            Controls.Add(tabControl1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(520, 440);
            Name = "frmMain";
            Padding = new Padding(5);
            Text = "이미지 변환";
            contextMenuStrip1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
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
        private GroupBox groupBox1;
        private ListBox lstTarget;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Label label4;
        private TextBox txtOutputPath;
        private Label label2;
        private Label label1;
        private TextBox txtHeight;
        private TextBox txtWidth;
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
    }
}
