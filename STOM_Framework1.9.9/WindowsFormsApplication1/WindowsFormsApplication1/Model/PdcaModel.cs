using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STOM.Model
{
    class PdcaModel
    {
        private string _ip;
        /// <summary>
        /// PDCA-IP
        /// </summary>
        public string Ip
        {
            get { return _ip; }
            set { _ip = value; }
        }
        private string _port;
        /// <summary>
        /// PDCA端口
        /// </summary>
        public string Port
        {
            get { return _port; }
            set { _port = value; }
        }
        private string _path;
        /// <summary>
        /// 保存ErrorCode的公盘路径
        /// </summary>
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }
        private string _user;
        /// <summary>
        /// 保存ErrorCode使用的用户名
        /// </summary>
        public string User
        {
            get { return _user; }
            set { _user = value; }
        }
        private string _psw;
        /// <summary>
        /// 保存ErrorCode使用的密码
        /// </summary>
        public string Psw
        {
            get { return _psw; }
            set { _psw = value; }
        }
        private string _project;
        /// <summary>
        /// 当前的项目
        /// </summary>
        public string Project
        {
            get { return _project; }
            set { _project = value; }
        }
        private string _bu;
        /// <summary>
        /// ErrorCode记录需要的关键字
        /// </summary>
        public string Bu
        {
            get { return _bu; }
            set { _bu = value; }
        }
        private string _floor;
        /// <summary>
        /// 当前的楼层
        /// </summary>
        public string Floor
        {
            get { return _floor; }
            set { _floor = value; }
        }
        private string _line;
        /// <summary>
        /// 产线号
        /// </summary>
        public string Line
        {
            get { return _line; }
            set { _line = value; }
        }
        
        private string _AEID;
        /// <summary>
        /// 工站编号
        /// </summary>
        public string AEID
        {
            get { return _AEID; }
            set { _AEID = value; }
        }
        
        private string _AESubID;
        /// <summary>
        /// 格式化发送给PDCA ErrorCode需要用的数据
        /// </summary>
        public string AESubID
        {
            get { return _AESubID; }
            set { _AESubID = value; }
        }

        private string _AEVendor;
        /// <summary>
        ///  格式化发送给PDCA ErrorCode需要用的数据
        /// </summary>
        public string AEVendor
        {
            get { return _AEVendor; }
            set { _AEVendor = value; }
        }
        
        private string _machineSN;
        /// <summary>
        /// 设备的SN
        /// </summary>
        public string MachineSN
        {
            get { return _machineSN; }
            set { _machineSN = value; }
        }

        private string _swRev;
        /// <summary>
        /// 机器的版本号
        /// </summary>
        public string SwRev
        {
            get { return _swRev; }
            set { _swRev = value; }
        }
        
        private string _hwRev;
        /// <summary>
        /// 不清楚干什么用的
        /// </summary>
        public string HwRev
        {
            get { return _hwRev; }
            set { _hwRev = value; }
        }
       
        private string _lswRev;
        /// <summary>
        /// 激光软件版本号
        /// </summary>
        public string LswRev
        {
            get { return _lswRev; }
            set { _lswRev = value; }
        }

        private double _waitUpdataTime;
        /// <summary>
        /// 等待上传完成的时间
        /// </summary>
        public double WaitUpdataTime
        {
            get { return _waitUpdataTime; }
            set { _waitUpdataTime = value; }
        }

        private double _waitAskTime;
        /// <summary>
        /// 等待MES给出请求的时间（在本机作为服务端的时候使用）
        /// </summary>
        public double WaitAskTime
        {
            get { return _waitAskTime; }
            set { _waitAskTime = value; }
        }

    }
}
