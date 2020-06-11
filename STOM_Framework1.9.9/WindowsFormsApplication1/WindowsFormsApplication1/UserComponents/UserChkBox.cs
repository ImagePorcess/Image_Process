using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STOM.UserComponents
{
    public partial class UserChkBox : UserControl
    {
        public UserChkBox()
        {
            InitializeComponent();
        }

        private string _descript = "";
        [Category("Custom"), Description("控件对应的数据库属性")]
        public string Descript
        {
            get { return _descript; }
            set { _descript = value; }
        }

        private string _chkBoxText = "";
        [Category("Custom"),Description("chkBox的描述信息")]
        public string ChkBoxText
        {
            get { return _chkBoxText; }
            set { _chkBoxText = value; this.checkBox.Text = ChkBoxText; }
        }

        private bool _chkChecked = false;
        /// <summary>
        /// 保持chebox的选中状态
        /// </summary>
        public bool ChkChecked
        {
            get { _chkChecked = this.checkBox.Checked; return _chkChecked; }
            set 
            { 
                _chkChecked = value;
                if(_chkChecked)
                {
                    this.checkBox.CheckState = System.Windows.Forms.CheckState.Checked;
                }
                else
                {
                    this.checkBox.CheckState = System.Windows.Forms.CheckState.Unchecked;
                }
            }
        }

        public event UpdateChkBoxAttribute ChkBoxUpdater;

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if( ChkBoxUpdater!= null )
            {
                ChkBoxUpdater(this);
            }
        }



    }
}
