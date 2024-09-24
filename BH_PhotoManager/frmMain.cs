using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BH_PhotoManager
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ctlMediaInfo1.HideClicEvent += CtlMediaInfo1_HideClicEvent; ;
        }

        private void CtlMediaInfo1_HideClicEvent()
        {
            ctlMediaInfo1.Visible = false;
            viewMediaInfo.Text = "상세 정보 표시";
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            LoadMedia();
        }

        private void LoadMedia()
        {
            TreeNode tnArchive = new TreeNode("Archive");
            string[] archive = Directory.GetDirectories(Application.StartupPath + @"\media\archive");
            foreach (string dic in archive)
            {
                tnArchive.Nodes.Add(getAllArchiveTreeNode(dic));
            }
            
            TreeNode tnFrame = new TreeNode("Frame");
            TreeNode tnPrint = new TreeNode("Print");
            treeView1.Nodes.Add(tnArchive);
            treeView1.Nodes.Add(tnFrame);
            treeView1.Nodes.Add(tnPrint);
        }

        TreeNode getAllArchiveTreeNode(string path)
        {
            TreeNode result = new TreeNode(Path.GetFileNameWithoutExtension(path));
            string[] dics = Directory.GetDirectories(path);
            foreach (string dic in dics)
            {
                result.Nodes.Add(getAllArchiveTreeNode(dic));
            }

            return result;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void viewMediaInfo_Click(object sender, EventArgs e)
        {
            if(ctlMediaInfo1.Visible)
            {
                ctlMediaInfo1.Visible = false;
                viewMediaInfo.Text = "상세 정보 표시";
            }
            else
            {
                ctlMediaInfo1.Visible = true;
                viewMediaInfo.Text = "상세 정보 숨김";
            }
        }
    }
}
