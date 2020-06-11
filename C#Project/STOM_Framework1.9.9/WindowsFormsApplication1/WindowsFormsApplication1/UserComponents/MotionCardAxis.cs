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
    public delegate bool UpdateModel(MotionCardAxis ui);
    public partial class MotionCardAxis : UserControl
    {
        public MotionCardAxis()
        {
            InitializeComponent();
        }

        private int _axisIndex = -1;
        /// <summary>
        /// 当前轴在axislist中的序号
        /// </summary>
        public int AxisIndex
        {
            get { return _axisIndex; }
            set { _axisIndex = value; }
        }

        /// <summary>
        /// 运动轴描述
        /// </summary>
        private string _axisDescript = "运动轴";
        [Category("Custom"), Description("运动轴描述")]
        public string AxisDescript
        {
            get { return _axisDescript; }
            set { _axisDescript = value; this.groupBox1.Text = _axisDescript; }
        }
        

        private int _axisNum = 0;
        [Category("Custom"), Description("AxisNum")]
        public int AxisNum
        {
            get { return _axisNum; }
            set { _axisNum = value; this.txtBox_AxisNum.Text = AxisNum.ToString(); }
        }

        private double _axisMinSpeed = 0;
        [Category("Custom"), Description("MinSpeed")]
        public double AxisMinSpeed
        {
            get { return _axisMinSpeed; }
            set { _axisMinSpeed = value; this.txtBox_MinSpeed.Text = AxisMinSpeed.ToString(); }
        }

        private double _axisMaxSpeed = 0;
        [Category("Custom"), Description("MaxSpeed")]
        public double AxisMaxSpeed
        {
            get { return _axisMaxSpeed; }
            set { _axisMaxSpeed = value; this.txtBox_MaxSpeed.Text = AxisMaxSpeed.ToString(); }
        }

        private bool _axisEnable = false;
        /// <summary>
        /// 轴的使能信号
        /// </summary>
        public bool AxisEnable
        {
            get { return _axisEnable; }
            set { _axisEnable = value; this.panel_ENABLE.BackColor = (AxisEnable == false ? Color.Lime : Color.Red); }
        }


        private double _axisCurPosition = 0;
        /// <summary>
        /// 轴当前的位置mm
        /// </summary>
        public double AxisCurPosition
        {
            get { return _axisCurPosition; }
            set { _axisCurPosition = value; this.txtBox_AxisPos.Text = AxisCurPosition.ToString(); }
        }

        private double _axisMoveLength = 0;
        /// <summary>
        /// 设置要移动的距离mm
        /// </summary>
        public double AxisMoveLength
        {
            get { return _axisMoveLength; }
            set { _axisMoveLength = value; this.txtBox_Move_Length.Text = AxisMoveLength.ToString(); }
        }

        private bool _axisMoveEnable = false;
        /// <summary>
        /// 轴在正常工作时,运动防呆机制
        /// </summary>
        public bool AxisMoveEnable
        {
            get { return _axisMoveEnable; }
            set { _axisMoveEnable = value; }
        }

        private bool _axisPositiveEL = false;
        /// <summary>
        /// 轴的正限位信号
        /// </summary>
        public bool AxisPositiveEL
        {
            get { return _axisPositiveEL; }
            set { _axisPositiveEL = value; this.panel_EL_POSITIVE.BackColor = (AxisPositiveEL == false ? Color.Lime : Color.Red); }
        }

        private bool _axisNegativeEL = false;
        /// <summary>
        /// 轴的负限位信号
        /// </summary>
        public bool AxisNegativeEL
        {
            get { return _axisNegativeEL; }
            set { _axisNegativeEL = value; this.panel_EL_NEGATIVE.BackColor = (AxisNegativeEL == false ? Color.Lime : Color.Red); }
        }

        private bool _axisALM = false;
        /// <summary>
        /// 轴的报警信号
        /// </summary>
        public bool AxisALM
        {
            get { return _axisALM; }
            set { _axisALM = value; this.panel_ALARM.BackColor = (AxisALM == false ? Color.Lime : Color.Red); }
        }

        private bool _axisEMG = false;
        /// <summary>
        /// 轴的急停信号
        /// </summary>
        public bool AxisEMG
        {
            get { return _axisEMG; }
            set { _axisEMG = value; this.panel_EMG.BackColor = (AxisALM == false ? Color.Lime : Color.Red); }
        }

        private bool _axisEMGEnable = true;
        /// <summary>
        /// 轴是否对急停信号响应
        /// </summary>
        public bool AxisEMGEnable
        {
            get { return _axisEMGEnable; }
            set { _axisEMGEnable = value; }
        }

        private int _axisMoveModel = 0;
        /// <summary>
        /// 轴的运动模式,1是连续运动, 0是定长运动(也就是相对位移), 2是定点运动(绝对位移)
        /// </summary>
        public int AxisMoveModel
        {
            get { return _axisMoveModel; }
            set
            { 
                _axisMoveModel = value; 
                if( _axisMoveModel == 0)
                {
                    this.radioBtn_PositionMove.Checked = false;
                    this.radioBtn_LengthMove.Checked = true;
                    this.radioBtn_ContinuousMove.Checked = false;
                }
                else if ( _axisMoveModel == 1)
                {
                    this.radioBtn_PositionMove.Checked = false;
                    this.radioBtn_LengthMove.Checked = false;
                    this.radioBtn_ContinuousMove.Checked = true;
                }
                else if( _axisMoveModel == 2)
                {
                    this.radioBtn_PositionMove.Checked = true;
                    this.radioBtn_LengthMove.Checked = false;
                    this.radioBtn_ContinuousMove.Checked = false;
                }
            }
        }

        private bool _axisOriStatus = false;
        /// <summary>
        /// 原点状态
        /// </summary>
        public bool AxisOriStatus
        {
            get { return _axisOriStatus; }
            set { _axisOriStatus = value; this.panel_ORI.BackColor = (AxisOriStatus == false ? Color.Lime : Color.Red); }
        }

        private bool load;
        /// <summary>
        /// 作为UI加载完成的标识,当UI第一次加载的时候不要启动数据库更新操作,防止出现再
        /// 未完成数据加载到界面时数据库数据被界面的原始数据刷新
        /// </summary>
        public bool LoadStatus
        {
            get { return load; }
            set { load = value; }
        }


        public event UpdateModel UIUpdater;  

        /// <summary>
        /// 根据轴的描述信息匹配轴的model,注册委托事件
        /// </summary>
        public void RegisterAxisToModel()
        {
            
            foreach (Model.MotionAxis axis in Control.MotionControl.axisList)
            {
                if (axis.Descript.Equals(AxisDescript))
                {
                    this.AxisIndex = Control.MotionControl.axisList.IndexOf(axis);
                    axis.Updater += new Model.UpdateUI(LoadModelToUI);
                    this.UIUpdater += new UpdateModel(axis.UpdateCurModel);
                    break;
                }
            }
        }
        /// <summary>
        /// 把model里面的数据加载到控件上
        /// </summary>
        /// <returns></returns>
        private bool  LoadModelToUI(Model.MotionAxis axis)
        {
            bool flag = false;

            if (axis.Descript.Equals(this.AxisDescript))
            {
                this.AxisNum = axis.Num;
                this.AxisALM = axis.ALMStatus;
                this.AxisEnable = axis.AxisEnable;
                this.AxisEMG = axis.EmgStatus;
                this.AxisEMGEnable = axis.EmgEnable;
                this.AxisMaxSpeed = axis.MaxSpeed;
                this.AxisMinSpeed = axis.MinSpeed;
                this.AxisMoveEnable = axis.MoveEnable;
                this.AxisMoveLength = axis.MoveLength;
                this.AxisMoveModel = axis.MoveModel;
                this.AxisNegativeEL = axis.NegativeEL;
                this.AxisPositiveEL = axis.PositiveEL;
                this.AxisCurPosition = axis.CurPosition;
                flag = true;
            }

            return flag;
        }

        /// <summary>
        /// 用来限制用户输入,只能输入数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBox_AxisNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            if ((e.KeyChar < '0' || e.KeyChar > '9' ) && e.KeyChar != '\b')
            {
                e.Handled = true;
            }

        }

        /// <summary>
        /// 限制用户输入只能输入double
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBox_MinSpeed_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.txtBox_AxisNum.TextLength == 0 && e.KeyChar == '.')
            {
                e.Handled = true;
            }
            else if (this.txtBox_AxisNum.Text.Contains(".") && e.KeyChar == '.')
            {
                e.Handled = true;
            }
            else if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b' && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 限制用户输入,只能输入double
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBox_MaxSpeed_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.txtBox_MaxSpeed.TextLength == 0 && e.KeyChar == '.')
            {
                e.Handled = true;
            }
            else if (this.txtBox_MaxSpeed.Text.Contains(".") && e.KeyChar == '.')
            {
                e.Handled = true;
            }
            else if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b' && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 限制用户输入,只能输入double
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBox_Move_Length_KeyPress(object sender, KeyPressEventArgs e)
        {
            if( this.txtBox_Move_Length.Text.Length == 0 && e.KeyChar == '.')
            {
                e.Handled = true;
            }
            else if(this.txtBox_Move_Length.Text.Contains(".") && e.KeyChar == '.')
            {
                e.Handled = true;
            }
            else
            if ( (e.KeyChar < '0' || e.KeyChar > '9' ) && e.KeyChar != '\b' && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }
      
        

        /// <summary>
        /// 修改轴的编号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBox_AxisNum_TextChanged(object sender, EventArgs e)
        {
            if (this.txtBox_AxisNum.Text.Trim().Equals("")) // 为了允许先清空数据后输入新数据
            {
                return;
            }
            this.AxisNum = Convert.ToInt32(this.txtBox_AxisNum.Text);
        }

        /// <summary>
        /// 防呆,如果没有设置轴编号,那么默认为之前设置的参数值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBox_AxisNum_Leave(object sender, EventArgs e)
        {
            if (txtBox_AxisNum.Text.Trim().Equals(""))
            {
                txtBox_AxisNum.Text = Convert.ToString(this.AxisNum);
            }

            if (UIUpdater != null)
            {
                UIUpdater(this);
            }
        }

        /// <summary>
        /// 设置初始速度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBox_MinSpeed_TextChanged(object sender, EventArgs e)
        {
           if( this.txtBox_MinSpeed.Text.Trim().Equals(""))
           {
               return;
           }
           this.AxisMinSpeed = Convert.ToDouble(this.txtBox_MinSpeed.Text.Trim());
        }

        /// <summary>
        /// 更新初始速度UI的数据到DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBox_MinSpeed_Leave(object sender, EventArgs e)
        {
            if (txtBox_MinSpeed.Text.Trim().Equals(""))
            {
                txtBox_MinSpeed.Text = Convert.ToString(this.AxisMinSpeed);
            }
            if (UIUpdater != null)
            {
                UIUpdater(this);
            }
        }

        /// <summary>
        /// 设置轴运动的最大速度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBox_MaxSpeed_TextChanged(object sender, EventArgs e)
        {
            if (this.txtBox_MaxSpeed.Text.Trim().Equals(""))
            {
                return;
            }
            this.AxisMaxSpeed = Convert.ToDouble(this.txtBox_MaxSpeed.Text.Trim());
        }

        /// <summary>
        /// 更新最大速度的UI数据到DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBox_MaxSpeed_Leave(object sender, EventArgs e)
        {
            if (txtBox_MaxSpeed.Text.Trim().Equals(""))
            {
                txtBox_MaxSpeed.Text = Convert.ToString(this.AxisMaxSpeed);
            }
            if (UIUpdater != null)
            {
                UIUpdater(this);
            }
        }

        /// <summary>
        /// 设置要移动的距离
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBox_Move_Length_TextChanged(object sender, EventArgs e)
        {
            if (this.txtBox_Move_Length.Text.Trim().Equals(""))
            {
                return;
            }
            this.AxisMoveLength = Convert.ToDouble(this.txtBox_Move_Length.Text.Trim());
        }

        /// <summary>
        /// 更新移动距离的UI数据到DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBox_Move_Length_Leave(object sender, EventArgs e)
        {
            if (txtBox_Move_Length.Text.Trim().Equals(""))
            {
                txtBox_Move_Length.Text = Convert.ToString(this.AxisMoveLength);
            }
            if (UIUpdater != null)
            {
                UIUpdater(this);
            }
        }

        private void btn_Home_Click(object sender, EventArgs e)
        {
            if ( this.AxisIndex < 0 )
            {
                MessageBox.Show("该轴未被初始化！");
            }
            else
            {
                Control.MotionControl.AxisGoHome(Control.MotionControl.axisList[this.AxisIndex]);
            }
            
        }


        private void btn_Move_Positive_KeyDown(object sender, KeyEventArgs e)
        {
            if( this.AxisMoveModel == 0 || this.AxisMoveModel == 2 )
            {
                Control.MotionControl.PMove(Control.MotionControl.axisList[this.AxisIndex]);
            }
            else
            {
                Control.MotionControl.VMove(Control.MotionControl.axisList[this.AxisIndex],1);
            }
        }

        private void btn_MoveNagetive_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.AxisMoveModel == 0 || this.AxisMoveModel == 2)
            {
                Control.MotionControl.PMove(Control.MotionControl.axisList[this.AxisIndex]);
            }
            else
            {
                Control.MotionControl.VMove(Control.MotionControl.axisList[this.AxisIndex], 0);
            }
        }

        /// <summary>
        /// 按钮弹起则运动停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Move_Positive_KeyUp(object sender, KeyEventArgs e)
        {
            if( this.AxisMoveModel == 2)
            {
                Control.MotionControl.StopDecel(Control.MotionControl.axisList[this.AxisIndex]);
            }
           
        }

        /// <summary>
        /// 按钮弹起则运动停止
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_MoveNagetive_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.AxisMoveModel == 2)
            {
                Control.MotionControl.StopDecel(Control.MotionControl.axisList[this.AxisIndex]);
            }
        }
       
    }
}
