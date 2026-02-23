using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BH_PhotoFrameMaker
{
    public partial class frmError : Form
    {
        List<ProcessData> processDatas = null;
        public frmError()
        {
            InitializeComponent();
        }

        public frmError(List<ProcessData> processDatas)
        {
            InitializeComponent();
            this.processDatas = processDatas;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView1.GridColor = Color.DarkGray;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            dataGridView1.DataSource = processDatas;
            colIsSuccess.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;    
            dataGridView1.Refresh();
            rbAll.Text = $"전체 ({processDatas.Count.ToString("#,#")})";
            rbSuccess.Text = $"성공 ({processDatas.Where(x => x.IsSuccess).Count().ToString("#,#")})";
            rbFail.Text = $"실패 ({processDatas.Where(x => x.IsSuccess == false).Count().ToString("#,#")})";
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            // 현재 행 번호 문자열 (1부터 시작)
            string rowNumber = (e.RowIndex + 1).ToString();

            // 폰트 기준으로 텍스트 크기 측정
            SizeF textSize = e.Graphics.MeasureString(
                rowNumber,
                dataGridView1.RowHeadersDefaultCellStyle.Font);

            int padding = 20; // 여백 추가
            int requiredWidth = (int)Math.Ceiling(textSize.Width + padding);

            // 현재 RowHeadersWidth보다 작으면 갱신
            if (dataGridView1.RowHeadersWidth < requiredWidth)
            {
                dataGridView1.RowHeadersWidth = requiredWidth;
            }

            // 행 번호 그리기
            Rectangle headerBounds = new Rectangle(
                e.RowBounds.Left,
                e.RowBounds.Top,
                dataGridView1.RowHeadersWidth,
                e.RowBounds.Height);

            using (Brush brush = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString(
                    rowNumber,
                    dataGridView1.RowHeadersDefaultCellStyle.Font,
                    brush,
                    headerBounds,
                    new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    });
            }
        }

        private void rbAll_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = processDatas;
            dataGridView1.Refresh();
        }

        private void rbSuccess_CheckedChanged(object sender, EventArgs e)
        {
            List<ProcessData> filterData = processDatas.Where(x => x.IsSuccess).ToList();
            dataGridView1.DataSource = filterData;
            dataGridView1.Refresh();
        }

        private void rbFail_CheckedChanged(object sender, EventArgs e)
        {
            List<ProcessData> filterData = processDatas.Where(x => x.IsSuccess == false).ToList();
            dataGridView1.DataSource = filterData;
            dataGridView1.Refresh();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "colIsSuccess") // 열 이름 확인
            {
                if (e.Value != null)
                {
                    string status = e.Value.ToString().ToLower();

                    if (status == "false")
                    {
                        e.CellStyle.ForeColor = Color.Red;
                        e.Value = "실패";
                    }
                    else if(status == "true")
                    {
                        e.Value = "성공";
                    }
                }
            }
        }
    }
}
