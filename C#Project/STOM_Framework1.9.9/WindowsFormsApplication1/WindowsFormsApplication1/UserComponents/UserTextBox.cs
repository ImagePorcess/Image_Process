using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace STOM.UserComponents
{
    public enum boxStyle
    {
        IntNum = 0,
        DoubleNum,
        Str,
        IPStr
    };
    public partial class UserTextBox : UserControl
    {
        public UserTextBox()
        {
            InitializeComponent();           
        }

        public event UpdateTextBoxAttribute TextBoxUpdate;

        private boxStyle _testStyle = boxStyle.Str;
        [Category("Custom"), Description("输入框类型")]
        public boxStyle TextStyle
        {
            get { return _testStyle; }
            set
            {
                _testStyle = value;

                switch (TextStyle)
                {
                    case boxStyle.DoubleNum:
                        OldValue = "0.0"; break;
                    case boxStyle.IntNum:
                        OldValue = "0"; break;
                    case boxStyle.IPStr:
                        OldValue = "0.0.0.0"; break;
                    case boxStyle.Str:
                    default:
                        OldValue = "Null"; break;
                }
            }
        }

        private string _value = "Null";
        [Category("Custom"), Description("输入")]
        public string OldValue
        {
            get { return _value; }
            set { _value = value; this.textBox.Text = OldValue; }
        }

        private string _newValue = "0";

        public string NewValue
        {
            get { return _newValue; }
            set { _newValue = value; }
        }


        private string _descript = "";
        [Category("Custom"), Description("描述")]
        public string Descript
        {
            get { return _descript; }
            set { _descript = value; }
        }

        

        Regex ipRegex = new Regex(@"[0-9]{1,3}[.][0-9]{1,3}[.][0-9]{1,3}[.][0-9]{1,3}");

        /// <summary>
        /// 根据输入的设置控制输入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_KeyPress(object sender, KeyPressEventArgs e) 
        {
            if ( TextStyle == boxStyle.IPStr )
            {
                if (this.textBox.TextLength == 0 && e.KeyChar == '.')
                {
                    e.Handled = true;
                }
                else if (this.textBox.Text.Count(p => p == '.') >= 3 && e.KeyChar == '.')
                {
                    e.Handled = true;
                }
                else if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b' && e.KeyChar != '.')
                {
                    e.Handled = true;
                }
            }
            else if (TextStyle == boxStyle.DoubleNum)
            {
                if (this.textBox.Text.Length == 0 && e.KeyChar == '.')
                {
                    e.Handled = true;
                }
                else if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b' && e.KeyChar != '.')
                {
                    e.Handled = true;
                }
            }
            else if (TextStyle == boxStyle.IntNum)
            {
                if ((e.KeyChar > '9' || e.KeyChar < '0') && e.KeyChar != '\b')
                {
                    e.Handled = true;
                }   
            }
                    
        }  

        private void textBox_TextChanged(object sender, EventArgs e)
        {
          
            if( !textBox.Text.Equals(""))
            {
                if (TextStyle == boxStyle.IPStr )
                {
                    if (ipRegex.IsMatch(OldValue.Trim()))
                    {
                        this.NewValue = this.textBox.Text;
                        if (TextBoxUpdate != null)
                        {
                            this.TextBoxUpdate(this);
                        }
                    }
                }              
                else
                {
                    this.NewValue = this.textBox.Text;
                    if (TextBoxUpdate != null)
                    {
                        this.TextBoxUpdate(this);
                    }
                }              
            }                
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            bool flag = true;
            if (this.textBox.Text.Trim().Equals(""))
            {
                MessageBox.Show("内容为空,请输入数据!");
                flag = false;
            }

            if( TextStyle == boxStyle.IPStr )
            {
                if( !ipRegex.IsMatch(this.textBox.Text)  )
                {
                    MessageBox.Show("IP格式不正确,请输入数据!");
                    flag = false;
                }
                else
                {
                    if (this.textBox.Text.Split('.')[3].Length > 3)
                    {
                        MessageBox.Show("IP格式不正确,请输入数据!");
                        flag = false;
                    }                      
                }
            }

            if( !flag )
            {
                
                this.textBox.Focus();
            }

        } 
    }
}
