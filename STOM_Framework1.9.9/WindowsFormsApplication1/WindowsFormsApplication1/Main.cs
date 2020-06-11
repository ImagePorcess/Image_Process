using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserControlResult.Model;
using STOM.Control;
using STOM.Model;
using STOM.API;
using System.Net;
using System.Text.RegularExpressions;
using STOM.UserComponents;
using STOM.Service;
namespace STOM
{
    /// <summary>
    /// 更新TextBox的数据委托
    /// </summary>
    /// <param name="box"></param>
    public delegate void UpdateTextBoxAttribute(UserTextBox box);

    /// <summary>
    /// 更新checkBox的数据委托
    /// </summary>
    /// <param name="chkBox"></param>
    public delegate void UpdateChkBoxAttribute(UserChkBox chkBox);

   
    public partial class Main : Form
    {

        private static object lookObject = new object();
        
        private int startX;
        private int startY;
        public Main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            TestResultModel tm = new TestResultModel();
            tm.BB = "1.0";
            tm.BT = "1.1";
            tm.CB = "1.2";
            tm.CT = "1.3";
            tm.Result = "OK";
            userControlResult_ONE.UpdateResult(tm);
        }

        private void Main_MouseDown(object sender, MouseEventArgs e)
        {
            if( e.Button == MouseButtons.Left)
            {
                startX = e.X;
                startY = e.Y;
            }
        }

        private void Main_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - startX;
                this.Top += e.Y - startY;
            }

        }

        private void picBox_Min_MouseClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void picBox_Min_MouseLeave(object sender, EventArgs e)
        {
            this.picBox_Min.BackColor = Form.DefaultBackColor;
        }

        private void picBox_Min_MouseMove(object sender, MouseEventArgs e)
        {
            this.picBox_Min.BackColor = Color.Gainsboro;
        }

        private void picBox_Exit_MouseClick(object sender, MouseEventArgs e)
        {
            if( MessageBox.Show("确定退出？","提示",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //add 资源释放
                CommucateControl.DisposeCommucate();
                this.Dispose();
                this.Close();
            }
        }

        private void picBox_Menu_MouseClick(object sender, MouseEventArgs e)
        {
            
            Point p = this.picBox_Menu.Location;
            p.X += this.Location.X;
            p.Y += this.Location.Y;
            contextMenuStrip.Show(p);
        }

        private void pictureBox_Exit_MouseLeave(object sender, EventArgs e)
        {
            this.picBox_Exit.BackColor = Form.DefaultBackColor;
        }

        private void picBox_Exit_MouseMove(object sender, MouseEventArgs e)
        {
            this.picBox_Exit.BackColor = Color.Gainsboro;
        }

        private void picBox_Menu_MouseMove(object sender, MouseEventArgs e)
        {
            this.picBox_Menu.BackColor = Color.Gainsboro;
        }

        private void picBox_Menu_MouseLeave(object sender, EventArgs e)
        {
            this.picBox_Menu.BackColor = Form.DefaultBackColor;
        }

        private void toolStripMenuItemAdmin_Click(object sender, EventArgs e)
        {

        }

        private void tabCtrl_Config_DrawItem(object sender, DrawItemEventArgs e)
        {
            SolidBrush brush = new SolidBrush(Color.Black);
            RectangleF tabTextArea = (RectangleF)tabCtrl_Config.GetTabRect(e.Index);

            StringFormat txtFormat = new StringFormat();
            txtFormat.LineAlignment = StringAlignment.Center;
            txtFormat.Alignment = StringAlignment.Center;
            e.Graphics.DrawString(tabCtrl_Config.Controls[e.Index].Text, SystemInformation.MenuFont, brush, tabTextArea);

        }
        /// <summary>
        /// 加载主界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_Load(object sender, EventArgs e)
        {
           
            LoadTableAdapter();

            LoadColorConfig();

            LoadErrorCodeItemList();

            LoadSysConfig();

            LoadPDCAConfig();         

            LoadLaser();     

            LoadMotionAxis();

            LoadMotionIO();

            RegisteSysConfigAttribute();

            RegisterPDCA();

            RegisterLaser();

            RegisterCognexCommucate();

            RegisterPDCACommucate();
        }
        /// <summary>
        /// 通过适配器将数据加载到表中
        /// </summary>
        public void LoadTableAdapter()
        {
            this.colorTableAdapter.Fill(this.dBStomDataSet.Color);
            this.errorCodeTableAdapter.Fill(this.dBStomDataSet.ErrorCode);
            this.motionIOTableAdapter.Fill(this.dBStomDataSet.MotionIO);
            this.motionAxisTableAdapter.Fill(this.dBStomDataSet.MotionAxis);
            this.cognexTableAdapter.Fill(this.dBStomDataSet.Cognex);          
        }
        /// <summary>
        /// 加载颜色cmbBox
        /// </summary>
        public void LoadColorConfig()
        {
            this.cmbBox_Color.Items.Clear();
            for (int i = 0; i < this.dBStomDataSet.Color.Rows.Count; i++)
                this.cmbBox_Color.Items.Add(this.dBStomDataSet.Color.Rows[i][3].ToString());
        }
        /// <summary>
        /// 加载errorcode的列表到cmbBox中
        /// </summary>
        public void LoadErrorCodeItemList()
        {
            this.cmbBox_ErrorCodeNum.Items.Clear();
            for (int i = 0; i < this.dBStomDataSet.ErrorCode.Rows.Count; i++)
                this.cmbBox_ErrorCodeNum.Items.Add(this.dBStomDataSet.ErrorCode.Rows[i]["ID"].ToString());
        }

        /// <summary>
        /// 加载系统参数显示
        /// </summary>
        public void LoadSysConfig()
        {
            this.numBox_TestNum.OldValue = Work.SysConfigModel.TestNum.ToString();
            this.numBox_SNLength.OldValue = Work.SysConfigModel.SnLength.ToString();
            this.numBox_TopPos.OldValue = Work.SysConfigModel.SimTrayTop.ToString();
            this.numBox_BottomPos.OldValue = Work.SysConfigModel.SimTrayBottom.ToString();
            this.textBox_MinLimit.Text = this.numBox_MinLimit.OldValue = Work.SysConfigModel.LimitMinValue.ToString();
            this.textBox_MaxLimit.Text = this.numBox_MaxLimit.OldValue = Work.SysConfigModel.LimitMaxValue.ToString();
            this.numBox_EditorAllowErrorValue.OldValue = Work.SysConfigModel.EditorAllowErrorValue.ToString();
            this.numBox_LogSaveDays.OldValue = Work.SysConfigModel.LogSaveDays.ToString();
            this.txtBox_SaveDataPath.Text = Work.SysConfigModel.SaveDataPath;

         

            this.cmbBox_TestModel.SelectedIndex = (int)Work.SysConfigModel.CurTestModel;
            this.cmbBox_TestWay.SelectedIndex = (int)Work.SysConfigModel.CurTestWay;
            this.cmbBox_Customer.SelectedIndex = (int)Work.SysConfigModel.CurCustomer;
            this.cmbBox_Port.SelectedIndex = (int)Work.SysConfigModel.CurPowerPort;

            this.userChkBox_SnFoxEnable.ChkChecked = Work.SysConfigModel.SnFoxEnable;
            this.userChkBox_SnHandSetEnable.ChkChecked = Work.SysConfigModel.SnHandSetEnable;
            this.userChkBox_HandSetColor.ChkChecked = Work.SysConfigModel.HandSetColorEnable;
            this.userChkBox_EditorCallBackEnable.ChkChecked = Work.SysConfigModel.EditorCallBackEnable;
            this.userChkBox_EditorSaveCallBackEnable.ChkChecked = Work.SysConfigModel.EditorSaveCallBackEnable;
            this.userChkBox_LogEnable.ChkChecked = Work.SysConfigModel.LogEnable;
            this.userChkBox_SaveDataEnable.ChkChecked = Work.SysConfigModel.LogEnable;
            this.userChkBox_ScanBarcodeEnable.ChkChecked = Work.SysConfigModel.ScanBarcodeEnable;
            this.userChkBox_EnableErrorCode.ChkChecked = Work.SysConfigModel.EnableErrorCode;
            this.userChkBox_EnableHeartBeat.ChkChecked = Work.SysConfigModel.EnableHeart;
            this.userChkBox_EnableMesCommunicate.ChkChecked = Work.SysConfigModel.EnableMesCommunicate;

            this.rdoBtn_ScanOne.Checked = Work.SysConfigModel.CurScanWay == ScanWay.OneWay ? true : false;
            this.rdoBtn_ScanTwo.Checked = Work.SysConfigModel.CurScanWay == ScanWay.TwoWay ? true : false;           

        }

        public void RegisteSysConfigAttribute()
        {
            this.numBox_SNLength.TextBoxUpdate += new UpdateTextBoxAttribute(UpdateSysConfigAttribute);
            this.numBox_TestNum.TextBoxUpdate += new UpdateTextBoxAttribute(UpdateSysConfigAttribute);
            this.numBox_TopPos.TextBoxUpdate += new UpdateTextBoxAttribute(UpdateSysConfigAttribute);
            this.numBox_BottomPos.TextBoxUpdate += new UpdateTextBoxAttribute(UpdateSysConfigAttribute);
            this.numBox_MaxLimit.TextBoxUpdate += new UpdateTextBoxAttribute(UpdateSysConfigAttribute);
            this.numBox_MinLimit.TextBoxUpdate += new UpdateTextBoxAttribute(UpdateSysConfigAttribute);
            this.numBox_EditorAllowErrorValue.TextBoxUpdate += new UpdateTextBoxAttribute(UpdateSysConfigAttribute);
            this.numBox_LogSaveDays.TextBoxUpdate += new UpdateTextBoxAttribute(UpdateSysConfigAttribute);

            this.userChkBox_SnFoxEnable.ChkBoxUpdater += new UpdateChkBoxAttribute(UpdateSysConfigEnableAttribute);
            this.userChkBox_SnHandSetEnable.ChkBoxUpdater += new UpdateChkBoxAttribute(UpdateSysConfigEnableAttribute);
            this.userChkBox_HandSetColor.ChkBoxUpdater += new UpdateChkBoxAttribute(UpdateSysConfigEnableAttribute);
            this.userChkBox_LogEnable.ChkBoxUpdater += new UpdateChkBoxAttribute(UpdateSysConfigEnableAttribute);
            this.userChkBox_EditorCallBackEnable.ChkBoxUpdater += new UpdateChkBoxAttribute(UpdateSysConfigEnableAttribute);
            this.userChkBox_EditorSaveCallBackEnable.ChkBoxUpdater += new UpdateChkBoxAttribute(UpdateSysConfigEnableAttribute);
            this.userChkBox_SaveDataEnable.ChkBoxUpdater += new UpdateChkBoxAttribute(UpdateSysConfigEnableAttribute);
            this.userChkBox_ScanBarcodeEnable.ChkBoxUpdater += new UpdateChkBoxAttribute(UpdateSysConfigEnableAttribute);
            this.userChkBox_EnableErrorCode.ChkBoxUpdater += new UpdateChkBoxAttribute(UpdateSysConfigEnableAttribute);
            this.userChkBox_EnableHeartBeat.ChkBoxUpdater += new UpdateChkBoxAttribute(UpdateSysConfigEnableAttribute);
            this.userChkBox_EnableMesCommunicate.ChkBoxUpdater += new UpdateChkBoxAttribute(UpdateSysConfigEnableAttribute);

            
        }

        /// <summary>
        /// 更新系统配置model
        /// </summary>
        public void UpdateSysConfigModel()
        {
           
        }
        /// <summary>
        /// 加载MES配置参数
        /// </summary>
        public void LoadPDCAConfig()
        {
            this.userTextBox_PDCAIP.OldValue = Work.PdcaConfigModel.Ip;
            this.userTextBox_PDCAPort.OldValue = Work.PdcaConfigModel.Port;
            this.userTextBox_PDCAPath.OldValue = Work.PdcaConfigModel.Path;
            this.userTextBox_PDCAUser.OldValue = Work.PdcaConfigModel.User;
            this.userTextBox_PDCAPsw.OldValue = Work.PdcaConfigModel.Psw;
            this.userTextBox_PDCAProject.OldValue = Work.PdcaConfigModel.Project;
            this.userTextBox_PDCABU.OldValue = Work.PdcaConfigModel.Bu;
            this.userTextBox_PDCAFloor.OldValue = Work.PdcaConfigModel.Floor;
            this.userTextBox_PDCALine.OldValue = Work.PdcaConfigModel.Line;
            this.userTextBox_PDCAAEID.OldValue = Work.PdcaConfigModel.AEID;
            this.userTextBox_PDCAAESubID.OldValue = Work.PdcaConfigModel.AESubID;
            this.userTextBox_PDCAAEVendor.OldValue = Work.PdcaConfigModel.AEVendor;
            this.userTextBox_MachineSN.OldValue = Work.PdcaConfigModel.MachineSN;
            this.userTextBox_SWRev.OldValue = Work.PdcaConfigModel.SwRev;
            this.userTextBox_HWRev.OldValue = Work.PdcaConfigModel.HwRev;
            this.userTextBox_LSWRev.OldValue = Work.PdcaConfigModel.LswRev;
            this.numBox_WaitAskTime.OldValue = Work.PdcaConfigModel.WaitAskTime.ToString();
            this.numBox_WaitUpdataTime.OldValue = Work.PdcaConfigModel.WaitUpdataTime.ToString();            
        }

        public void RegisterPDCA()
        {
            this.userTextBox_PDCAIP.TextBoxUpdate += new UpdateTextBoxAttribute(UpdatePDCAConfig);
            this.userTextBox_PDCAPort.TextBoxUpdate += new UpdateTextBoxAttribute(UpdatePDCAConfig);
            this.userTextBox_PDCAPath.TextBoxUpdate += new UpdateTextBoxAttribute(UpdatePDCAConfig);
            this.userTextBox_PDCAUser.TextBoxUpdate += new UpdateTextBoxAttribute(UpdatePDCAConfig);
            this.userTextBox_PDCAPsw.TextBoxUpdate += new UpdateTextBoxAttribute(UpdatePDCAConfig);
            this.userTextBox_PDCAProject.TextBoxUpdate += new UpdateTextBoxAttribute(UpdatePDCAConfig);
            this.userTextBox_PDCABU.TextBoxUpdate += new UpdateTextBoxAttribute(UpdatePDCAConfig);
            this.userTextBox_PDCAFloor.TextBoxUpdate += new UpdateTextBoxAttribute(UpdatePDCAConfig);
            this.userTextBox_PDCALine.TextBoxUpdate += new UpdateTextBoxAttribute(UpdatePDCAConfig);
            this.userTextBox_PDCAAEID.TextBoxUpdate += new UpdateTextBoxAttribute(UpdatePDCAConfig);
            this.userTextBox_PDCAAESubID.TextBoxUpdate += new UpdateTextBoxAttribute(UpdatePDCAConfig);
            this.userTextBox_PDCAAEVendor.TextBoxUpdate += new UpdateTextBoxAttribute(UpdatePDCAConfig);
            this.userTextBox_MachineSN.TextBoxUpdate += new UpdateTextBoxAttribute(UpdatePDCAConfig);
            this.userTextBox_SWRev.TextBoxUpdate += new UpdateTextBoxAttribute(UpdatePDCAConfig);
            this.userTextBox_HWRev.TextBoxUpdate += new UpdateTextBoxAttribute(UpdatePDCAConfig);
            this.userTextBox_LSWRev.TextBoxUpdate += new UpdateTextBoxAttribute(UpdatePDCAConfig);
            this.numBox_WaitAskTime.TextBoxUpdate += new UpdateTextBoxAttribute(UpdatePDCAConfig);
            this.numBox_WaitUpdataTime.TextBoxUpdate += new UpdateTextBoxAttribute(UpdatePDCAConfig);  
        }


        /// <summary>
        /// 加载激光设备参数
        /// </summary>
        public void LoadLaser()
        {
            this.userTextBox_LaserIP.OldValue = Work.CognexConfigModel.Ip;
            this.numBox_Port.OldValue = Work.CognexConfigModel.Port;
            this.numBox_DelayTime.OldValue = Work.CognexConfigModel.DelayTime.ToString();
            this.numBox_TimeOutValue.OldValue = Work.CognexConfigModel.WaitTime.ToString();
            this.userChkBox_DelayEnable.ChkChecked = Work.CognexConfigModel.EnableDelay;       
        }

        public void RegisterLaser()
        {
            this.userTextBox_LaserIP.TextBoxUpdate += new UpdateTextBoxAttribute(UpdateLaserConfig);
            this.numBox_Port.TextBoxUpdate += new UpdateTextBoxAttribute(UpdateLaserConfig);
            this.numBox_DelayTime.TextBoxUpdate += new UpdateTextBoxAttribute(UpdateLaserConfig);
            this.numBox_TimeOutValue.TextBoxUpdate += new UpdateTextBoxAttribute(UpdateLaserConfig);
            this.userChkBox_DelayEnable.ChkBoxUpdater += new UpdateChkBoxAttribute(UpdateLaserConfigEnableAttribute);
        }

        
        private void btn_SaveData_Click(object sender, EventArgs e)
        {
            string code = this.dataGridView_ColorCfg.CurrentRow.Cells[1].Value.ToString();
            string cognex_code = this.dataGridView_ColorCfg.CurrentRow.Cells[2].Value.ToString();
            string descript = this.dataGridView_ColorCfg.CurrentRow.Cells[3].Value.ToString();
            MessageBox.Show(string.Format("新增{0:D}条数据", this.colorTableAdapter.InsertQuery(code, cognex_code, descript)));
            this.colorTableAdapter.Fill(this.dBStomDataSet.Color);
            LoadColorConfig();
        }

        private void btn_DeleteData_Click(object sender, EventArgs e)
        {
            int id = (int)this.dataGridView_ColorCfg.CurrentRow.Cells[0].Value;
            string code = this.dataGridView_ColorCfg.CurrentRow.Cells[1].Value.ToString();
            string cognex_code = this.dataGridView_ColorCfg.CurrentRow.Cells[2].Value.ToString();
            string descript = this.dataGridView_ColorCfg.CurrentRow.Cells[3].Value.ToString();
            MessageBox.Show(string.Format("删除{0:D}条数据", this.colorTableAdapter.DeleteQuery(id, code, cognex_code, descript)));
            this.colorTableAdapter.Fill(this.dBStomDataSet.Color);
            LoadColorConfig();
        }

        /// <summary>
        /// 加载轴数据到界面
        /// </summary>
        private void LoadMotionAxis()
        {
            string curAxisName = "";
            this.motionCardAxis1.RegisterAxisToModel();
            
            curAxisName = this.motionCardAxis1.AxisDescript;

            try
            {
                foreach (MotionAxis axis in MotionControl.axisList)
                {
                     this.motionCardAxis1.LoadStatus = axis.UpdateCurUI();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(curAxisName + "Axis信息加载失败,可能原因是委托注册失败导致的\r" + ex.Message);
            }
            
            
        }

        /// <summary>
        /// 加载IO点位数据到界面
        /// </summary>
        private void LoadMotionIO()
        {
            string curIOName = "";
            this.IO_Input_Reset.RegisterIOToModel();
            this.IO_Input_CarrierOri.RegisterIOToModel();
            this.IO_Input_CarrierAct.RegisterIOToModel();
            this.IO_Input_Cylinder.RegisterIOToModel();
            this.IO_Input_StartLeft.RegisterIOToModel();
            this.IO_Input_StartRight.RegisterIOToModel();
            this.IO_Output_Vacuum.RegisterIOToModel();
            this.IO_Output_Cylinder.RegisterIOToModel();
            this.IO_Output_GreenLight.RegisterIOToModel();
            this.IO_Output_YellowLight.RegisterIOToModel(); 

            try
            {
                foreach (MotionIO io in MotionControl.ioList)
                {
                    curIOName = io.IoDescript;
                    io.IoUpdateUI();  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(curIOName + "IO信息加载失败,可能原因是委托注册失败导致的\r" + ex.Message);
            }
            
        }

        /// <summary>
        /// 获取所有IO的状态信息,并刷新到界面上
        /// </summary>
        public void UpdateIOStatus()
        {
            foreach(MotionIO io in MotionControl.ioList)
            {
                io.IoStatus = Convert.ToBoolean( MotionControl.GetIOStatus(io) );
                
                io.IoUpdateUI();  
            }
        }

        public void UpdateAxisStatus()
        {
            foreach (MotionAxis axis in MotionControl.axisList)
            {
                MotionControl.GetAxisStatus(axis);
                axis.UpdateCurUI();
            }
        }


        /// <summary>
        /// 根据IO界面设置更新数据到数据库
        /// </summary>
        /// <param name="newPortNum">IO更新后点位</param>
        /// <param name="newStatus">IO状态</param>
        /// <param name="oldStyle">IO的类型 1:输入 0:输出</param>
        /// <param name="oldPortNum">IO更新前点位 </param>
        /// <returns></returns>
        public  bool UpdateMotionIO( int newPortNum,int newStatus, int oldStyle, int oldPortNum )
        {
            bool flag = false;
            this.motionIOTableAdapter.UpdateQuery(newPortNum, newStatus, oldStyle, oldPortNum);
            return flag;
        }

        public bool UpdateMotionAxis(int newAxisNum, string newDescript, int newMoveEnable, string newMinSpeed, string newMaxSpeed, int newEnableStatus, int newPEL, int newNEL, int newEMG, int newALM, string newCurPos, string newMoveLength, int newMoveModel, int OldAxisNum, string OldDescript)
        {
            bool flag = false;
            this.motionAxisTableAdapter.UpdateQuery(newAxisNum,
                newDescript,
                newMoveEnable,
                newMinSpeed,
                newMaxSpeed,
                newEnableStatus,
                newPEL,
                newNEL,
                newEMG,
                newALM,
                newCurPos,
                newMoveLength,
                newMoveModel,
                OldAxisNum, OldDescript);
            return flag;
        }

        private void btn_Connect_Click(object sender, EventArgs e)
        {
            RegisterCognexCommucate();        
        }

        private void btn_Brake_Click(object sender, EventArgs e)
        {
            CommucateControl.DisposeCommucate();   
        }


        private void btn_SendS_Click(object sender, EventArgs e)
        {
            string cmd = string.Format("S{0},{1},{2},{3},{4},{5}", "1", "HousingSN", "Color", "0", Work.SysConfigModel.LimitMinValue, Work.SysConfigModel.LimitMaxValue);
            CommucateControl.cognexSocket.SendMsg(cmd);
            this.txtBox_Msgs.Text += "发送数据：" + cmd + CommucateControl.cognexSocket.EndCode;
            this.txtBox_Msgs.Text += "接收数据：" + CommucateControl.cognexSocket.ReceiveMsg();
        }

        private void btn_SendSBack_Click(object sender, EventArgs e)
        {
            string cmd = string.Format("S{0},{1},{2},{3},{4},{5}", "1", "HousingSN", "Color", "1", Work.SysConfigModel.LimitMinValue, Work.SysConfigModel.LimitMaxValue);
            CommucateControl.cognexSocket.SendMsg(cmd);
            this.txtBox_Msgs.Text += "发送数据：" + cmd + CommucateControl.cognexSocket.EndCode;
            this.txtBox_Msgs.Text += "接收数据：" + CommucateControl.cognexSocket.ReceiveMsg();
        }

        private void btn_SendT_Click(object sender, EventArgs e)
        {
            double AxisCurPos = 13.59;
            string cmd = string.Format("T{0},{1},{2}", "1", AxisCurPos.ToString("0.00"), "0");
            CommucateControl.cognexSocket.SendMsg(cmd);
            this.txtBox_Msgs.Text += "发送数据：" + cmd + CommucateControl.cognexSocket.EndCode;
            this.txtBox_Msgs.Text += "接收数据：" + CommucateControl.cognexSocket.ReceiveMsg();
        }

        private void btn_SendTBack_Click(object sender, EventArgs e)
        {
            double AxisCurPos = 20.89;
            string cmd = string.Format("T{0},{1},{2}", "1", AxisCurPos.ToString("0.00"), "1");
            CommucateControl.cognexSocket.SendMsg(cmd);
            this.txtBox_Msgs.Text += "发送数据：" + cmd + CommucateControl.cognexSocket.EndCode;
            this.txtBox_Msgs.Text += "接收数据：" + CommucateControl.cognexSocket.ReceiveMsg();
        }

        private void btn_SendP_Click(object sender, EventArgs e)
        {
            double AxisCurPos = 20.89;
            string cmd = string.Format("P{0},{1},{2}", "1", AxisCurPos.ToString("0.00"), 1);
            CommucateControl.cognexSocket.SendMsg(cmd);
            this.txtBox_Msgs.Text += "发送数据：" + cmd + CommucateControl.cognexSocket.EndCode;
            
        }

        private void btn_GetData_Click(object sender, EventArgs e)
        {
            this.txtBox_Msgs.Text += "接收数据：" + CommucateControl.cognexSocket.ReceiveMsg();
        }

        private void btn_ClearData_Click(object sender, EventArgs e)
        {
            this.txtBox_Msgs.Text = null;
        }     

        private void txtBox_MES_Port_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
        }
      
        private void cmbBox_TestModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.configurationTableAdapter.UpdateQuery("CurTestModel", this.cmbBox_TestModel.SelectedIndex.ToString(), "CurTestModel", Work.SysConfigModel.CurTestModel.ToString());
            Work.SysConfigModel.CurTestModel = (TestModel)this.cmbBox_TestModel.SelectedIndex;

            switch(Work.SysConfigModel.CurTestModel)
            {
                case TestModel.ProduteModel:
                    this.lbl_WorkModel.Text = "生产模式";
                    break;
                case TestModel.GrrModel:
                    this.lbl_WorkModel.Text = "GRR模式";
                    break;
                case TestModel.DebugModel:
                    this.lbl_WorkModel.Text = "工程模式";
                    break;
                case TestModel.MotionTestModel:
                    this.lbl_WorkModel.Text = "运动测试模式";
                    break;
                case TestModel.CorrelationModel:
                    this.lbl_WorkModel.Text = "correlation模式";
                    break;
                default:
                    Work.SysConfigModel.CurTestModel = 0;
                    this.lbl_WorkModel.Text = "生产模式";
                    break;
            }

            
        }

        private void cmbBox_TestWay_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.configurationTableAdapter.UpdateQuery("CurTestWay", this.cmbBox_TestWay.SelectedIndex.ToString(), "CurTestWay", Work.SysConfigModel.CurTestWay.ToString());
            Work.SysConfigModel.CurTestWay = (TestWay)this.cmbBox_TestWay.SelectedIndex;
        }

        private void rdoBtn_ScanOne_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdoBtn_ScanOne.Checked)
            {
                this.configurationTableAdapter.UpdateQuery("ScanWay", "0", "ScanWay", Convert.ToInt32(Work.SysConfigModel.CurScanWay).ToString());
                Work.SysConfigModel.CurScanWay = ScanWay.OneWay;
            }
            else
            {
                this.configurationTableAdapter.UpdateQuery("ScanWay", "1", "ScanWay", Convert.ToInt32(Work.SysConfigModel.CurScanWay).ToString());
                Work.SysConfigModel.CurScanWay = ScanWay.TwoWay;
            }
        }

       
        private void cmbBox_Customer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbBox_Port_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.configurationTableAdapter.UpdateQuery("PowerPort", this.cmbBox_Port.SelectedIndex.ToString(), "PowerPort", Convert.ToInt32(Work.SysConfigModel.CurPowerPort).ToString());
            Work.SysConfigModel.CurPowerPort = (PowerPort)Convert.ToInt32(this.cmbBox_Port.SelectedIndex);
             
        }


        public void UpdateSysConfigAttribute(UserTextBox box)
        {          
            this.configurationTableAdapter.UpdateQuery(box.Descript, box.NewValue, box.Descript, box.OldValue);
            Work.InitSysConfig();
            box.OldValue = box.NewValue;
        }
        public void UpdateSysConfigEnableAttribute(UserChkBox chkBox)
        {
            string oldValue = chkBox.ChkChecked == true ? "1" : "0";
            string newValue = chkBox.ChkChecked == true ? "0" : "1";
            this.configurationTableAdapter.UpdateQuery(chkBox.Descript, oldValue, chkBox.Descript, newValue);
            Work.InitSysConfig();
        }
       
        public void UpdatePDCAConfig(UserTextBox box)
        {
            this.pdcaTableAdapter.UpdateQuery(box.Descript, box.NewValue, box.Descript, box.OldValue);
            Work.InitMESConfig();
            box.OldValue = box.NewValue;
        }

        public void UpdateLaserConfig(UserTextBox box)
        {
            this.cognexTableAdapter.UpdateQuery(box.Descript, box.NewValue, box.Descript, box.OldValue);
            Work.InitLaserConfig();
            box.OldValue = box.NewValue;
        }

        public void UpdateLaserConfigEnableAttribute(UserChkBox chkBox)
        {
            string oldValue = chkBox.ChkChecked == true ? "1" : "0";
            string newValue = chkBox.ChkChecked == true ? "0" : "1";
            this.cognexTableAdapter.UpdateQuery(chkBox.Descript, oldValue, chkBox.Descript, newValue);
            Work.InitLaserConfig();
        }

        private void btn_SendMsg_Click(object sender, EventArgs e)
        {
            CommucateControl.pdcaSocket.SendSend_msg(); 
        }

        private static void PDCADataReceived(object o, STOM.API.AbstractSocket.ReceiveEventArgs rea)
        {
           string[] dataArrayRecv = rea.Message.Replace('\n', (char)32).Replace('\r', (char)32).Trim().Split(','); //观澜过滤回车，换行字符
           lock (lookObject)
           {
              
               if (dataArrayRecv[0].Contains("Ack") )
               {
                   MessageBox.Show(dataArrayRecv[0]);
               }
               else
               {
                   MessageBox.Show(dataArrayRecv[0]);
               }
           }
        }

        private void PDCADataReceived(Object sender, byte[] data)
        {
            int count = data.Count();
            string receiveString = Encoding.Default.GetString(data, 0, count);
            
            lock (lookObject)
            {
                if ( receiveString.Contains("\r\n"))
                {
                    if (receiveString.Contains("Ack"))
                    {
                        MessageBox.Show("A");
                        MessageBox.Show(receiveString);
                    }
                    else if (receiveString.Contains("Test"))
                    {
                        MessageBox.Show("T");
                        MessageBox.Show(receiveString);
                    }
                }
                else
                {
                    MessageBox.Show("N");
                    MessageBox.Show(receiveString);
                }
               
            }
        }

        private void btn_RequestFGSN_Click(object sender, EventArgs e)
        {           
 
        }

        private void btn_ConnectPDCA_Click(object sender, EventArgs e)
        {
            RegisterPDCACommucate();
        }

        private void btn_BrakeConnect_Click(object sender, EventArgs e)
        {
            CommucateControl.pdcaSocket.OnRecData -= PDCADataReceived;
            CommucateControl.pdcaSocket.DisposePDCA();
        }

        /// <summary>
        /// 注册PDCA通信,指定委托
        /// </summary>
        public void RegisterPDCACommucate()
        {
            if (CommucateControl.pdcaSocket.client == null)
            {
                CommucateControl.pdcaSocket = new PDCASocket("\r\n");
            }
            CommucateControl.pdcaSocket.OnRecData += PDCADataReceived;
            CommucateControl.pdcaSocket.EventHandlerReceive += PDCADataReceived;
            CommucateControl.pdcaSocket.GetStatus();

            if (CommucateControl.pdcaSocket.ConnectStatus)
            {
                MessageBox.Show("PDCA通信已经建立!");
            }  
        }

        /// <summary>
        /// 注册cognex的通信
        /// </summary>
        public void RegisterCognexCommucate()
        {
            if (CommucateControl.cognexSocket.client == null)
            {
                CommucateControl.cognexSocket.ConnectServer();
            }

            CommucateControl.cognexSocket.GetStatus();

            if (CommucateControl.cognexSocket.ConnectStatus)
            {
                MessageBox.Show("Cognex通信已经建立!");
            }  
        }
       
    }
}
