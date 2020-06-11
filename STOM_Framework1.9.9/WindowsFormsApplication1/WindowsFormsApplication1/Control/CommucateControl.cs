using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using STOM.API;
using STOM.Service;
namespace STOM.Control
{
    static public class CommucateControl
    {
       
        static public CognexSocket cognexSocket = new CognexSocket("\r\n");

        static public PDCASocket pdcaSocket = new PDCASocket("\r\n");

        static public bool ConnectCognexSockets()
        {
            bool flag = false;
            if ( cognexSocket == null )
            {
                cognexSocket = new CognexSocket("\r\n");
                
            }
            cognexSocket.ConnectServer();

            return flag;
        }


        static public void ConnectPDCASocket()
        {
            if( pdcaSocket == null || pdcaSocket.client == null )
            {
                pdcaSocket = new PDCASocket("\r\n"); //异步通信
            }
        }


        static public void DisposeCommucate()
        {
            cognexSocket.CloseConnect();
            pdcaSocket.ClosePDCAConnect();
        }
       

    }
}
