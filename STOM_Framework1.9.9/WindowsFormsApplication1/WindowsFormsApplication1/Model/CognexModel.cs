using STOM.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace STOM.Model
{
    class CognexModel
    {
        private string _ip;
        /// <summary>
        /// cognex的IP
        /// </summary>
        public string Ip
        {
            get { return _ip; }
            set { _ip = value; }
        }
        private string _port;
        /// <summary>
        /// cognex的port
        /// </summary>
        public string Port
        {
            get { return _port; }
            set { _port = value; }
        }
        private double _waitTime;
        /// <summary>
        /// 通讯超时等待时间
        /// </summary>
        public double WaitTime
        {
            get { return _waitTime; }
            set { _waitTime = value; }
        }
        private double _delayTime;
        /// <summary>
        /// 通讯的延时
        /// </summary>
        public double DelayTime
        {
            get { return _delayTime; }
            set { _delayTime = value; }
        }
        private bool _enableDelay;
        /// <summary>
        /// 是否延时通讯
        /// </summary>
        public bool EnableDelay
        {
            get { return _enableDelay; }
            set { _enableDelay = value; }
        }
        

    }
}
