using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using STOM.API;

namespace STOM.UserComponents
{
    public delegate bool UpdateIOModel(MotionCardIO io);
    public partial class MotionCardIO : UserControl
    {
        public MotionCardIO()
        {
            InitializeComponent();
        }

        private ushort _ioPoint = 0;
        [Category("Custom"), Description("IOPoint")]
        public ushort IoPoint
        {
            get { return _ioPoint; }
            set { _ioPoint = value; this.txtBox_IOPoint.Text = IoPoint.ToString(); }
        }
        private bool _ioStatus = false;
        [Category("Custom"), Description("IOStatus")]
        public bool IoStatus
        {
            get { return _ioStatus; }
            set { _ioStatus = value; this.panel_IOStatus.BackColor = (IoStatus == true ? Color.Red : Color.Lime); }
        }

        private string _ioName = "Name:";
        [Category("Custom"), Description("IOName")]
        public string IoName
        {
            get { return _ioName; }
            set { _ioName = value; this.lbl_IOName.Text = IoName; }
        }

        private bool _ioStyle;
        [Category("Custom"), Description("style = true 表示输入, style = false表示输出")]
        public bool IoStyle
        {
            get { return _ioStyle; }
            set { _ioStyle = value; }
        }
        

        private void panel_IOStatus_MouseClick(object sender, MouseEventArgs e)
        {
            if ( IoStyle == false )
            {
                this.IoStatus = !this.IoStatus;
            }
            IOModelUpdater(this);
           
        }

        /// <summary>
        /// 定义委托用来把UI的数据传递给model
        /// </summary>
        public event UpdateIOModel IOModelUpdater;

        /// <summary>
        /// 注册Model到控件上,通过委托实现Model变化修改UI的数据
        /// </summary>
        public void RegisterIOToModel()
        {
            foreach( Model.MotionIO io in Control.MotionControl.ioList )
            {
                if (io.IoDescript.Equals(this.IoName) && this.IoStyle == io.IoStyle)
                {
                    io.IOUpdater += new Model.UpdateIOUI(LoadIOToModel);
                    this.IOModelUpdater += new UpdateIOModel(io.IOModelUpdate);
                    break;
                }
            }
        }

        /// <summary>
        /// 根据IO的类型和描述加载对应的界面参数
        /// </summary>
        /// <param name="io">IO类</param>
        /// <returns></returns>
        private bool LoadIOToModel(Model.MotionIO io)
        {
            bool flag = false;
            if (this.IoStyle == io.IoStyle && this.IoName == io.IoDescript)
            {
                this.IoPoint = io.IoPointNum;
                this.IoStatus = io.IoStatus;
                flag = true;
            }
            return flag;
        }

        private void txtBox_IOPoint_TextChanged(object sender, EventArgs e)
        {
            this.IoPoint = Convert.ToUInt16(this.txtBox_IOPoint.Text);
            if( IOModelUpdater != null)
            {
                IOModelUpdater(this);
            }
            
        }
        

    }
}
