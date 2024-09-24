using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BH_PhotoManager
{
    public partial class ctlMediaInfo : UserControl
    {
        public delegate void HideClickEventHandler();
        public event HideClickEventHandler HideClicEvent;

        public ctlMediaInfo()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if(HideClicEvent != null)
            {
                HideClicEvent();
            }
        }
    }
}
