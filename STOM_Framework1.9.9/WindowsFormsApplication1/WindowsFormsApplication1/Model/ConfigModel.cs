using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STOM.API;
namespace STOM.Model
{
    
    class ConfigModel
    {
        private double _simTrayTop;
        /// <summary>
        /// Simtray扫描的顶部位置
        /// </summary>
        public double SimTrayTop
        {
            get { return _simTrayTop; }
            set { _simTrayTop = value; }
        }

        private double _simTrayBottom;
        /// <summary>
        /// SimTray扫描的底部位置
        /// </summary>
        public double SimTrayBottom
        {
            get { return _simTrayBottom; }
            set { _simTrayBottom = value; }
        }
        private TestModel _curTestModel;
        /// <summary>
        /// 当前测试模式
        /// </summary>
        internal TestModel CurTestModel
        {
            get { return _curTestModel; }
            set { _curTestModel = value; }
        }
        private TestWay _curTestWay;
        /// <summary>
        /// 当前测试方式
        /// </summary>
        internal TestWay CurTestWay
        {
            get { return _curTestWay; }
            set { _curTestWay = value; }
        }
        private int _testNum;
        /// <summary>
        /// 设置的测试次数
        /// </summary>
        public int TestNum
        {
            get { return _testNum; }
            set { _testNum = value; }
        }
        private ScanWay _curScanWay;
        /// <summary>
        /// 扫描方式
        /// </summary>
        internal ScanWay CurScanWay
        {
            get { return _curScanWay; }
            set { _curScanWay = value; }
        }
        private bool _saveDataEnable;
        /// <summary>
        /// 是否保存测试数据
        /// </summary>
        public bool SaveDataEnable
        {
            get { return _saveDataEnable; }
            set { _saveDataEnable = value; }
        }
        private string _saveDataPath;
        /// <summary>
        /// 数据保存路径
        /// </summary>
        public string SaveDataPath
        {
            get { return _saveDataPath; }
            set { _saveDataPath = value; }
        }
        private int _snLength;
        /// <summary>
        /// 设置SN的长度
        /// </summary>
        public int SnLength
        {
            get { return _snLength; }
            set { _snLength = value; }
        }
        private bool _scanBarcodeEnable;
        /// <summary>
        /// 是否启用扫码
        /// </summary>
        public bool ScanBarcodeEnable
        {
            get { return _scanBarcodeEnable; }
            set { _scanBarcodeEnable = value; }
        }

        private bool _snFoxEnable;
        /// <summary>
        /// 设置焦点自动到SN输入框中
        /// </summary>
        public bool SnFoxEnable
        {
            get { return _snFoxEnable; }
            set { _snFoxEnable = value; }
        }
        private bool _snHandSetEnable;
        /// <summary>
        /// 是否手动输入SN
        /// </summary>
        public bool SnHandSetEnable
        {
            get { return _snHandSetEnable; }
            set { _snHandSetEnable = value; }
        }
        private double _limitMaxValue;
        /// <summary>
        /// 物料上限
        /// </summary>
        public double LimitMaxValue
        {
            get { return _limitMaxValue; }
            set { _limitMaxValue = value; }
        }
        private double _limitMinValue;
        /// <summary>
        /// 物料下限
        /// </summary>
        public double LimitMinValue
        {
            get { return _limitMinValue; }
            set { _limitMinValue = value; }
        }
        private bool _colorHandSetEnable;
        /// <summary>
        /// 是否手动选择颜色
        /// </summary>
        public bool HandSetColorEnable
        {
            get { return _colorHandSetEnable; }
            set { _colorHandSetEnable = value; }
        }
        private double _editorAllowErrorValue;
        /// <summary>
        /// 编码器允许误差
        /// </summary>
        public double EditorAllowErrorValue
        {
            get { return _editorAllowErrorValue; }
            set { _editorAllowErrorValue = value; }
        }
        private bool _editorCallBackEnable;
        /// <summary>
        /// 是否使用编码器反馈
        /// </summary>
        public bool EditorCallBackEnable
        {
            get { return _editorCallBackEnable; }
            set { _editorCallBackEnable = value; }
        }
        private bool _editorSaveCallBackEnable;
        /// <summary>
        /// 是否保持编码器反馈
        /// </summary>
        public bool EditorSaveCallBackEnable
        {
            get { return _editorSaveCallBackEnable; }
            set { _editorSaveCallBackEnable = value; }
        }
        private int _logSaveDays;
        /// <summary>
        /// 日志保持时间
        /// </summary>
        public int LogSaveDays
        {
            get { return _logSaveDays; }
            set { _logSaveDays = value; }
        }
        private bool _logEnable;
        /// <summary>
        /// 是否启用日志
        /// </summary>
        public bool LogEnable
        {
            get { return _logEnable; }
            set { _logEnable = value; }
        }
        private Customer _curCustomer;
        /// <summary>
        /// 客户名称
        /// </summary>
        internal Customer CurCustomer
        {
            get { return _curCustomer; }
            set { _curCustomer = value; }
        }
        private PowerPort _curPowerPort;
        /// <summary>
        /// 功率表串口号
        /// </summary>
        internal PowerPort CurPowerPort
        {
            get { return _curPowerPort; }
            set { _curPowerPort = value; }
        }

        private bool _enableHeart;
        /// <summary>
        /// 是否开启心跳
        /// </summary>
        public bool EnableHeart
        {
            get { return _enableHeart; }
            set { _enableHeart = value; }
        }

        private bool _enableErrorCode;
        /// <summary>
        /// 是否开启ErrorCode上传
        /// </summary>
        public bool EnableErrorCode
        {
            get { return _enableErrorCode; }
            set { _enableErrorCode = value; }
        }

        private bool _enableMesCommunicate;
        /// <summary>
        /// 是否开启MES通讯
        /// </summary>
        public bool EnableMesCommunicate
        {
            get { return _enableMesCommunicate; }
            set { _enableMesCommunicate = value; }
        }

        
    }
}
