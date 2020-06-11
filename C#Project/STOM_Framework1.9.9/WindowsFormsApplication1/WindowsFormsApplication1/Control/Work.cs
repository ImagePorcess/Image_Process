using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STOM.Service;
using STOM.Model;
using STOM.API;
using System.Data;


namespace STOM.Control
{
    class Work
    {
        /// <summary>
        /// 配置数据库连接
        /// </summary>
        public static AccessDBService ConfigDB = new AccessDBService();


        /// <summary>
        /// 系统配置表
        /// </summary>
        public static DataTable ConfigData;
        /// <summary>
        /// 系统配置对象
        /// </summary>
        public static ConfigModel SysConfigModel = new ConfigModel();

        /// <summary>
        /// MES配置表
        /// </summary>
        public static DataTable PdcaConfigData;
        /// <summary>
        /// PDCA配置对象
        /// </summary>
        public static PdcaModel PdcaConfigModel = new PdcaModel();

        /// <summary>
        /// ErrorCode配置表
        /// </summary>
        public static DataTable ErrorCodeConfigData;
        /// <summary>
        /// ErrorCode配置对象
        /// </summary>
        public static ErrorCodeModel ErrorCodeConfigModel = new ErrorCodeModel();

        /// <summary>
        /// 激光的配置表
        /// </summary>
        public static DataTable LaserConfigData;
        /// <summary>
        /// 激光配置对象,后面做一个基类对象
        /// </summary>
        public static CognexModel CognexConfigModel = new CognexModel();
        public static void InitWork()
        {
            InitSysConfig();
            InitMESConfig();
            InitLaserConfig();
        }
        /// <summary>
        /// 初始化系统配置
        /// </summary>
        public static void InitSysConfig()
        {
            try
            {
                ConfigData = ConfigDB.QueryTable("Configuration");
                SysConfigModel.CurCustomer = (Customer)Convert.ToInt32(ConfigData.Select("Names='CurCustomer'")[0]["Values"]);
                SysConfigModel.CurPowerPort = (PowerPort)Convert.ToInt32(ConfigData.Select("Names='PowerPort'")[0]["Values"]);
                SysConfigModel.CurScanWay = (ScanWay)Convert.ToInt32(ConfigData.Select("Names='ScanWay'")[0]["Values"]);
                SysConfigModel.CurTestModel = (TestModel)Convert.ToInt32(ConfigData.Select("Names='TestModel'")[0]["Values"]);
                SysConfigModel.CurTestWay = (TestWay)Convert.ToInt32(ConfigData.Select("Names='TestWay'")[0]["Values"]);
                SysConfigModel.EditorAllowErrorValue = Convert.ToDouble(ConfigData.Select("Names='EditorAllowErrorValue'")[0]["Values"]);
                SysConfigModel.EditorCallBackEnable = ConfigData.Select("Names='EditorCallBackEnable'")[0]["Values"].ToString() == "1" ? true : false;
                SysConfigModel.EditorSaveCallBackEnable = ConfigData.Select("Names='EditorSaveCallBackEnable'")[0]["Values"].ToString() == "1" ? true : false;
                SysConfigModel.LimitMaxValue = Convert.ToDouble(ConfigData.Select("Names='LimitMaxValue'")[0]["Values"]);
                SysConfigModel.LimitMinValue = Convert.ToDouble(ConfigData.Select("Names='LimitMinValue'")[0]["Values"]);
                SysConfigModel.LogEnable = ConfigData.Select("Names='LogEnable'")[0]["Values"].ToString() == "1" ? true : false;
                SysConfigModel.LogSaveDays = Convert.ToInt32(ConfigData.Select("Names='LogSaveDays'")[0]["Values"]);
                SysConfigModel.HandSetColorEnable = ConfigData.Select("Names='HandSetColor'")[0]["Values"].ToString() == "1" ? true : false;
                SysConfigModel.SaveDataEnable = ConfigData.Select("Names='SaveDataEnable'")[0]["Values"].ToString() == "1" ? true : false;
                SysConfigModel.SaveDataPath = ConfigData.Select("Names='SaveDataPath'")[0]["Values"].ToString();
                SysConfigModel.SimTrayBottom = Convert.ToDouble(ConfigData.Select("Names='SimTrayBottom'")[0]["Values"]);
                SysConfigModel.SimTrayTop = Convert.ToDouble(ConfigData.Select("Names='SimTrayTop'")[0]["Values"]);
                SysConfigModel.SnFoxEnable = ConfigData.Select("Names='SnFoxEnable'")[0]["Values"].ToString() == "1" ? true : false;
                SysConfigModel.SnHandSetEnable = ConfigData.Select("Names='SnHandSetEnable'")[0]["Values"].ToString() == "1" ? true : false;
                SysConfigModel.SnLength = Convert.ToInt32(ConfigData.Select("Names='SnLength'")[0]["Values"]);
                SysConfigModel.TestNum = Convert.ToInt32(ConfigData.Select("Names='TestNum'")[0]["Values"]);
                SysConfigModel.ScanBarcodeEnable = ConfigData.Select("Names='ScanBarcodeEnable'")[0]["Values"].ToString() == "1" ? true : false;
                SysConfigModel.EnableErrorCode = ConfigData.Select("Names='EnableErrorCode'")[0]["Values"].ToString() == "1" ? true : false;
                SysConfigModel.EnableHeart = ConfigData.Select("Names='EnableHeartBeat'")[0]["Values"].ToString() == "1" ? true : false;
                SysConfigModel.EnableMesCommunicate = ConfigData.Select("Names='EnableMesCommunicate'")[0]["Values"].ToString() == "1" ? true : false;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("初始化系统参数异常" + ex.Message);
            }
        }

        /// <summary>
        /// 初始化MES配置,为兼容各个版本mes最好重载
        /// </summary>
        public static void InitMESConfig()
        {
            try
            {
                PdcaConfigData = ConfigDB.QueryTable("PDCA");
                PdcaConfigModel.Ip = PdcaConfigData.Select("Attribute='IP'")[0]["Values"].ToString();
                PdcaConfigModel.Port = PdcaConfigData.Select("Attribute='Port'")[0]["Values"].ToString();
                PdcaConfigModel.Path = PdcaConfigData.Select("Attribute='Path'")[0]["Values"].ToString();
                PdcaConfigModel.User = PdcaConfigData.Select("Attribute='User'")[0]["Values"].ToString();
                PdcaConfigModel.Psw = PdcaConfigData.Select("Attribute='Psw'")[0]["Values"].ToString();
                PdcaConfigModel.Project = PdcaConfigData.Select("Attribute='Project'")[0]["Values"].ToString();
                PdcaConfigModel.Bu = PdcaConfigData.Select("Attribute='Bu'")[0]["Values"].ToString();
                PdcaConfigModel.Floor = PdcaConfigData.Select("Attribute='Floor'")[0]["Values"].ToString();
                PdcaConfigModel.Line = PdcaConfigData.Select("Attribute='Line'")[0]["Values"].ToString();
                PdcaConfigModel.AEID = PdcaConfigData.Select("Attribute='AEID'")[0]["Values"].ToString();
                PdcaConfigModel.AESubID = PdcaConfigData.Select("Attribute='AESubID'")[0]["Values"].ToString();
                PdcaConfigModel.AEVendor = PdcaConfigData.Select("Attribute='AEVendor'")[0]["Values"].ToString();
                PdcaConfigModel.MachineSN = PdcaConfigData.Select("Attribute='MachineSN'")[0]["Values"].ToString();
                PdcaConfigModel.SwRev = PdcaConfigData.Select("Attribute='SwRev'")[0]["Values"].ToString();
                PdcaConfigModel.HwRev = PdcaConfigData.Select("Attribute='HwRev'")[0]["Values"].ToString();
                PdcaConfigModel.LswRev = PdcaConfigData.Select("Attribute='LswRev'")[0]["Values"].ToString();
                PdcaConfigModel.WaitUpdataTime = Convert.ToDouble(PdcaConfigData.Select("Attribute='WaitUpdataTime'")[0]["Values"].ToString());
                PdcaConfigModel.WaitAskTime = Convert.ToDouble(PdcaConfigData.Select("Attribute='WaitAskTime'")[0]["Values"].ToString());
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("初始化PDCA参数异常" + ex.Message);
            }
        }

        /// <summary>
        /// 加载激光设备的配置
        /// </summary>
        public static void InitLaserConfig()
        {
            try
            {
                LaserConfigData = ConfigDB.QueryTable("Cognex");
                CognexConfigModel.Ip = LaserConfigData.Select("Attribute='IP'")[0]["Values"].ToString();
                CognexConfigModel.Port = LaserConfigData.Select("Attribute='Port'")[0]["Values"].ToString();
                CognexConfigModel.WaitTime = Convert.ToDouble( LaserConfigData.Select("Attribute='TimeOutValue'")[0]["Values"]);
                CognexConfigModel.DelayTime = Convert.ToDouble(LaserConfigData.Select("Attribute='DelayTime'")[0]["Values"]);
                CognexConfigModel.EnableDelay = LaserConfigData.Select("Attribute='DelayEnable'")[0]["Values"].ToString() == "1" ? true : false;
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("初始化激光设备参数异常" + ex.Message);
            }
        }


        /// <summary>
        /// 更新config数据
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="attribute">属性名</param>
        /// <param name="value">属性值</param>
        public static void UpdateConfig(string table,string attribute,string value)
        {
            ConfigDB.UpdateTableAttribute(table, attribute, value);
        }
        /// <summary>
        /// 初始化errorcode配置表
        /// 用户通过选择cbm选择显示
        /// <para name="index">errorcode的编号</para>
        /// </summary>
        public static void LoadErrorCodeConfig(int index)
        {
            try
            {
                ErrorCodeConfigData = ConfigDB.QueryTable("ErrorCode");
                ErrorCodeConfigModel.TimeModel = ErrorCodeConfigData.Select(string.Format("ID=`{0}`", index))[0]["TimeModel"].ToString();
                ErrorCodeConfigModel.ErrorCode = ErrorCodeConfigData.Select(string.Format("ID=`{0}`", index))[0]["ErrorCode"].ToString();
                ErrorCodeConfigModel.Descript = ErrorCodeConfigData.Select(string.Format("ID=`{0}`", index))[0]["ErrorDescription"].ToString();
                ErrorCodeConfigModel.Color = ErrorCodeConfigData.Select(string.Format("ID=`{0}`", index))[0]["ColorCode"].ToString();
                ErrorCodeConfigModel.Notes = ErrorCodeConfigData.Select(string.Format("ID=`{0}`", index))[0]["Notes"].ToString();
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("读取ErrorCode信息失败" + ex.Message);
            }

        }
       

    }

}
