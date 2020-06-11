using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STOM.API;
namespace STOM.Model
{
    /// <summary>
    /// 定义一个委托用来注册IO界面刷新
    /// </summary>
    /// <param name="io"></param>
    /// <returns></returns>
    delegate bool UpdateIOUI(MotionIO io);
    class MotionIO
    {
        private bool _ioStyle;
        /// <summary>
        /// 点位类型 true 是输入,false是输出
        /// </summary>
        public bool IoStyle
        {
            get { return _ioStyle; }
            set { _ioStyle = value; }
        }
        private ushort _ioPointNum;
        /// <summary>
        /// 点位编号
        /// </summary>
        public ushort IoPointNum
        {
            get { return _ioPointNum; }
            set { _ioPointNum = value; }
        }
        private string _ioDescript;
        /// <summary>
        /// 点位描述例如：复位
        /// </summary>
        public string IoDescript
        {
            get { return _ioDescript; }
            set { _ioDescript = value; }
        }
        private bool _ioStatus;
        /// <summary>
        /// 当前点位状态
        /// </summary>
        public bool IoStatus
        {
            get { return _ioStatus; }
            set { _ioStatus = value; }
        }

        public event UpdateIOUI IOUpdater;
        /// <summary>
        /// 执行UI刷新
        /// </summary>
        public void IoUpdateUI()
        {
            if ( !IOUpdater(this) )
            {
                System.Windows.Forms.MessageBox.Show(this.IoDescript + "数据加载到界面失败");
            }
            
        }

        /// <summary>
        /// 根据UI界面的数据修改数据源
        /// </summary>
        /// <param name="io"></param>
        /// <returns></returns>
        public bool IOModelUpdate(UserComponents.MotionCardIO io)
        {
            bool flag = false;
            Main diag = new Main();
            diag.UpdateMotionIO(Convert.ToInt16(io.IoPoint),Convert.ToInt16(io.IoStatus), Convert.ToInt16(this.IoStyle),Convert.ToInt16(this.IoPointNum));
            this.IoPointNum = io.IoPoint;
            this.IoStatus = io.IoStatus;
            return flag;
        }

        public void GetStatus()
        {
            
        }
    }
}
