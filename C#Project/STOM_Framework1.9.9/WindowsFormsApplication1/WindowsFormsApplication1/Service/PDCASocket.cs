using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STOM.API;
using System.Net;
using STOM.Control;

namespace STOM.Service
{
    public class PDCASocket: AbstractSocket
    {
        public PDCASocket( string endCode)
        {
            this.Ip = IPAddress.Parse(Work.PdcaConfigModel.Ip);
            this.Port = Convert.ToInt32(Work.PdcaConfigModel.Port);
            this.EndCode = endCode;
            if( !InitSocketAsynchronous())
            {
                this.ClosePDCAConnect();
            }
        }

        public void SendSend_msg()
        {
            AsyncSend("Send_msg,@");
        }

        public void DisposePDCA()
        {          
            this.CloseConnect();
        }
        

    }
}
