using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STOM.API;
namespace STOM.Model
{
    delegate bool UpdateUI(MotionAxis axis); 
    class MotionAxis
    {
        private int _num = 0;
        /// <summary>
        /// 当前轴号
        /// </summary>
        public int Num 
        {
            get { return _num; }
            set { _num = value; }
        }
        private string _descript ;
        /// <summary>
        /// 轴描述
        /// </summary>
        public string Descript
        {
            get { return _descript; }
            set { _descript = value; }
        }
        private bool _axisEnable = false;
        /// <summary>
        /// 轴的使能信号
        /// </summary>
        public bool AxisEnable
        {
            get { return _axisEnable; }
            set { _axisEnable = value; }
        }


        private double _minSpeed = 0.0;
        /// <summary>
        /// 轴的启动速度
        /// </summary>
        public double MinSpeed
        {
            get { return _minSpeed; }
            set { _minSpeed = value; }
        }
        private double _maxSpeed = 0.0;
        /// <summary>
        /// 轴的最大速度
        /// </summary>
        public double MaxSpeed
        {
            get { return _maxSpeed; }
            set { _maxSpeed = value; }
        }
        private bool _positiveEL = false;
        /// <summary>
        /// 轴正限位信号
        /// </summary>
        public bool PositiveEL
        {
            get { return _positiveEL; }
            set { _positiveEL = value; }
        }

        private bool _negativeEL = false;
        /// <summary>
        /// 轴负限位信号
        /// </summary>
        public bool NegativeEL
        {
            get { return _negativeEL; }
            set { _negativeEL = value; }
        }

        private bool _emgStatus = false;
        /// <summary>
        /// 急停信号
        /// </summary>
        public bool EmgStatus
        {
            get { return _emgStatus; }
            set { _emgStatus = value; }
        }
        private bool _emgEnable = false; 
        /// <summary>
        /// 急停使能
        /// </summary>
        public bool EmgEnable
        {
            get { return _emgEnable; }
            set { _emgEnable = value; }
        }
        private bool _almStatus = false;
        /// <summary>
        /// 轴报警状态
        /// </summary>
        public bool ALMStatus
        {
            get { return _almStatus; }
            set { _almStatus = value; }
        }

        private bool _oriStatus = false;
        /// <summary>
        /// 轴的原点信号
        /// </summary>
        public bool OriStatus
        {
            get { return _oriStatus; }
            set { _oriStatus = value; }
        }

        private double _millimeters;
        /// <summary>
        /// 电机转一圈,轴运动多少个毫米
        /// </summary>
        public double Millimeters
        {
            get { return _millimeters; }
            set { _millimeters = value; }
        }
        private double _pluse;
        /// <summary>
        /// 电机转一圈,驱动器发送多少脉冲
        /// </summary>
        public double Pluse
        {
            get { return _pluse; }
            set { _pluse = value; }
        }
        private bool _homeDirection;
        /// <summary>
        /// 回原方向true 正方向, false 反方向
        /// </summary>
        public bool HomeDirection
        {
            get { return _homeDirection; }
            set { _homeDirection = value; }
        }
        private bool _homeModel;
        /// <summary>
        /// 回原点模式
        /// </summary>
        public bool HomeModel
        {
            get { return _homeModel; }
            set { _homeModel = value; }
        }

        private int _pluseOutModel;
        /// <summary>
        /// 驱动器输出模式
        /// </summary>
        public int PluseOutModel
        {
            get { return _pluseOutModel; }
            set { _pluseOutModel = value; }
        }

        private double _curPosition;
        /// <summary>
        /// 轴当前所在位置mm  
        /// </summary>
        public double CurPosition
        {
            get { return _curPosition; }
            set { _curPosition = value; }
        }

        private bool _moveEnable;
        /// <summary>
        /// 运动使能,防止轴在正常工作的时候操作员误点击了按钮导致轴的运动被打断的防呆机制
        /// </summary>
        public bool MoveEnable
        {
            get { return _moveEnable; }
            set { _moveEnable = value; }
        }


        private double _moveLength;
        /// <summary>
        /// 轴要移动的长度mm
        /// </summary>
        public double MoveLength
        {
            get { return _moveLength; }
            set { _moveLength = value; }
        }

        private int _moveModel;
        /// <summary>
        /// 当前轴的运动模式 1:连续运动, 0:定长运动, 2:定点运动
        /// </summary>
        public int MoveModel
        {
            get { return _moveModel; }
            set { _moveModel = value; }
        }


        private bool _powerEffective;
        /// <summary>
        /// 轴电平有效信号 true 高电平有效, false 低电平有效
        /// </summary>
        public bool PowerEffective
        {
            get { return _powerEffective; }
            set { _powerEffective = value; }
        }

        public event UpdateUI Updater;
        /// <summary>
        /// 更新数据到界面,调用委托,使得绑定的数据不会出错
        /// </summary>
        public bool UpdateCurUI()
        {
            return Updater(this);
        }

        public bool UpdateCurModel(UserComponents.MotionCardAxis axis)
        {
            bool flag = false;
            Main diag = new Main();
            diag.UpdateMotionAxis(axis.AxisNum,
                axis.AxisDescript,
                Convert.ToInt32(axis.AxisMoveEnable),
                axis.AxisMinSpeed.ToString(),
                axis.AxisMaxSpeed.ToString(),
                Convert.ToInt32(axis.AxisEnable),
                Convert.ToInt32(axis.AxisPositiveEL),
                Convert.ToInt32(axis.AxisNegativeEL),
                Convert.ToInt32(axis.AxisEMG),
                Convert.ToInt32(axis.AxisALM),
                axis.AxisCurPosition.ToString(),
                axis.AxisMoveLength.ToString(),
                Convert.ToInt32(axis.AxisMoveModel),
                this.Num,
                this.Descript);

            this.Num = axis.AxisNum;
            this.Descript = axis.AxisDescript;
            this.MoveEnable = axis.AxisMoveEnable;
            this.MinSpeed = axis.AxisMinSpeed;
            this.MaxSpeed = axis.AxisMaxSpeed;
            this.AxisEnable = axis.AxisEnable;
            this.PositiveEL = axis.AxisPositiveEL;
            this.NegativeEL = axis.AxisNegativeEL;
            this.EmgStatus = axis.AxisEMG;
            this.ALMStatus = axis.AxisALM;
            this.CurPosition = axis.AxisCurPosition;
            this.MoveLength = axis.AxisMoveLength;
            this.MoveModel = axis.AxisMoveModel;
            return flag;
        }
    }
}
