namespace BH_PhotoFrameMaker
{
    partial class frmError
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            dataGridView1 = new DataGridView();
            colIsSuccess = new DataGridViewTextBoxColumn();
            colFileName = new DataGridViewTextBoxColumn();
            colFilePath = new DataGridViewTextBoxColumn();
            colErrorMessage = new DataGridViewTextBoxColumn();
            panel1 = new Panel();
            rbSuccess = new RadioButton();
            rbFail = new RadioButton();
            label1 = new Label();
            rbAll = new RadioButton();
            flowLayoutPanel1 = new FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { colIsSuccess, colFileName, colFilePath, colErrorMessage });
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 32);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.Size = new Size(784, 329);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellFormatting += dataGridView1_CellFormatting;
            dataGridView1.RowPostPaint += dataGridView1_RowPostPaint;
            // 
            // colIsSuccess
            // 
            colIsSuccess.DataPropertyName = "IsSuccess";
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            colIsSuccess.DefaultCellStyle = dataGridViewCellStyle1;
            colIsSuccess.HeaderText = "구분";
            colIsSuccess.Name = "colIsSuccess";
            colIsSuccess.ReadOnly = true;
            colIsSuccess.Width = 60;
            // 
            // colFileName
            // 
            colFileName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colFileName.DataPropertyName = "FileName";
            colFileName.HeaderText = "파일명";
            colFileName.Name = "colFileName";
            colFileName.ReadOnly = true;
            // 
            // colFilePath
            // 
            colFilePath.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colFilePath.DataPropertyName = "FilePath";
            colFilePath.HeaderText = "전체 경로";
            colFilePath.Name = "colFilePath";
            colFilePath.ReadOnly = true;
            // 
            // colErrorMessage
            // 
            colErrorMessage.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colErrorMessage.DataPropertyName = "ErrorMessage";
            colErrorMessage.HeaderText = "오류 내용";
            colErrorMessage.Name = "colErrorMessage";
            colErrorMessage.ReadOnly = true;
            // 
            // panel1
            // 
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(784, 32);
            panel1.TabIndex = 1;
            // 
            // rbSuccess
            // 
            rbSuccess.AutoSize = true;
            rbSuccess.Dock = DockStyle.Left;
            rbSuccess.Location = new Point(130, 6);
            rbSuccess.Name = "rbSuccess";
            rbSuccess.Size = new Size(49, 19);
            rbSuccess.TabIndex = 3;
            rbSuccess.Text = "성공";
            rbSuccess.UseVisualStyleBackColor = true;
            rbSuccess.CheckedChanged += rbSuccess_CheckedChanged;
            // 
            // rbFail
            // 
            rbFail.AutoSize = true;
            rbFail.Dock = DockStyle.Left;
            rbFail.Location = new Point(185, 6);
            rbFail.Name = "rbFail";
            rbFail.Size = new Size(49, 19);
            rbFail.TabIndex = 2;
            rbFail.Text = "실패";
            rbFail.UseVisualStyleBackColor = true;
            rbFail.CheckedChanged += rbFail_CheckedChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Left;
            label1.Location = new Point(3, 3);
            label1.Name = "label1";
            label1.Size = new Size(66, 25);
            label1.TabIndex = 1;
            label1.Text = "표시 구분: ";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // rbAll
            // 
            rbAll.AutoSize = true;
            rbAll.Checked = true;
            rbAll.Dock = DockStyle.Left;
            rbAll.Location = new Point(75, 6);
            rbAll.Name = "rbAll";
            rbAll.Size = new Size(49, 19);
            rbAll.TabIndex = 0;
            rbAll.TabStop = true;
            rbAll.Text = "전체";
            rbAll.UseVisualStyleBackColor = true;
            rbAll.CheckedChanged += rbAll_CheckedChanged;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(label1);
            flowLayoutPanel1.Controls.Add(rbAll);
            flowLayoutPanel1.Controls.Add(rbSuccess);
            flowLayoutPanel1.Controls.Add(rbFail);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(0, 3, 0, 0);
            flowLayoutPanel1.Size = new Size(784, 32);
            flowLayoutPanel1.TabIndex = 4;
            // 
            // frmError
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 361);
            Controls.Add(dataGridView1);
            Controls.Add(panel1);
            MinimumSize = new Size(800, 400);
            Name = "frmError";
            Text = "오류 뷰어";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel1.ResumeLayout(false);
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView dataGridView1;
        private Panel panel1;
        private RadioButton rbSuccess;
        private RadioButton rbFail;
        private Label label1;
        private RadioButton rbAll;
        private DataGridViewTextBoxColumn colIsSuccess;
        private DataGridViewTextBoxColumn colFileName;
        private DataGridViewTextBoxColumn colFilePath;
        private DataGridViewTextBoxColumn colErrorMessage;
        private FlowLayoutPanel flowLayoutPanel1;
    }
}