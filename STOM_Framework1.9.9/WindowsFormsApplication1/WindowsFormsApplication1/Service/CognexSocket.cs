using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using STOM.Control;
namespace STOM.API
{
    public class CognexSocket : AbstractSocket
    {
       
        public  CognexSocket(string endCode)
        {
            this.Ip = IPAddress.Parse(Work.CognexConfigModel.Ip);
            this.Port = Convert.ToInt32(Work.CognexConfigModel.Port);
            this.EndCode = endCode;
            InitSocket();       
        }

    }
}
