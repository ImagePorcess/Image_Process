using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace STOM.API
{

    public abstract class AbstractSocket
    {
        IPAddress _ip;
        /// <summary>
        /// ip地址
        /// </summary>
        public IPAddress Ip
        {
            get { return _ip; }
            set { _ip = value; }
        }
        int _port;
        /// <summary>
        /// 端口号
        /// </summary>
        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        byte[] _buffer;
        /// <summary>
        /// 数据包
        /// </summary>
        public byte[] Buffer
        {
            get { return _buffer; }
            set { _buffer = value; }
        }

        private string _endCode;
        /// <summary>
        /// 结束符
        /// </summary>
        public string EndCode
        {
            get { return _endCode; }
            set { _endCode = value; }
        }

        private bool _connectStatus = false;

        public bool ConnectStatus
        {
            get { return _connectStatus; }
            set { _connectStatus = value; }
        }

        public event EventHandler<byte[]> OnRecData; //定义一个委托类型的事件  
        private StringBuilder sbReceiveDataBuffer = new StringBuilder();

        public Socket client;
        private byte[] dataBuffer = new byte[1024];

        #region 同步通信
        /// <summary>
        /// 使用同步的方式建立通信连接
        /// </summary>
        public void InitSocket()
        {
            try
            {
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Connect(Ip, Port);
                ConnectStatus = client.Connected;
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(string.Format("连接{0}:{1}失败!请检查网络是否连接成功",Ip,Port) + ex.Message);
            }

        }


        public void GetStatus()
        {
            ConnectStatus = client.Connected;
        }

        /// <summary>
        /// 开始连接服务器
        /// </summary>
        public void ConnectServer()
        {
            try
            {
                if( client == null)
                {
                    InitSocket();
                }

                GetStatus();

                if (!ConnectStatus)
                {
                    client.Connect(Ip, Port);
                    System.Windows.Forms.MessageBox.Show("通信已经建立");
                }
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 断开服务器连接
        /// </summary>
        public void CloseConnect()
        {
            if (client == null) return;
            if( client.Connected )
            {
                client.Shutdown(SocketShutdown.Both);
                client.Disconnect(true);
            }
            client.Dispose();
            client = null;
        }
        /// <summary>
        /// 清除缓冲区的数据
        /// </summary>
        public void ClearBuffer()
        {
            if( client != null)
            {
                int byteBuffer = 0;
                byteBuffer = client.Available;
                byte[] tempBuffer = new byte[1024];
                while( byteBuffer > 0)
                {
                    client.Receive(tempBuffer,SocketFlags.None);
                    byteBuffer = client.Available;
                }
                tempBuffer = null;
            }
        }


        virtual public void SendMsg(string msg)
        {
            Buffer = new byte[1024];

            if (client != null)
            {
                ClearBuffer();
                Buffer = Encoding.ASCII.GetBytes(msg + EndCode);
                try
                {
                    client.Send(Buffer);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(string.Format("send {0}失败!", msg) + ex.Message);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("请初始化cognexSoket");
            }
        }

        virtual public string ReceiveMsg()
        {
            string resMsg = "";

            if (client != null)
            {
                bool read = true;
                byte[] temp = new byte[1];

                try
                {
                    while (read)
                    {
                        client.Receive(temp);
                        resMsg += Encoding.ASCII.GetString(temp);
                        if (resMsg.Contains("\r\n"))
                        {
                            read = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(string.Format("cognexSoket接收数据异常:{0}", resMsg) + ex.Message);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("请初始化cognexSoket");
            }
            return resMsg; 
        }
        #endregion

        #region 异步通信
        /// <summary>
        /// 初始化异步通讯,注册接收数据的回调函数
        /// </summary>
        public bool InitSocketAsynchronous()
        {
            ConnectStatus = false;
            try
            {
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                client.SendTimeout = 1000;
               
                IPEndPoint remotePoint = new IPEndPoint(Ip, Port);
                //client.Connect(remotePoint);

                //StateObject state = new StateObject();
                
                //client.BeginReceive(state.buffer, 0, StateObject.BufferSize, SocketFlags.None,
                //    new AsyncCallback(ReceiveCallBack), state);

                client.BeginConnect(remotePoint, asyncResult =>
                {
                    client.EndConnect(asyncResult); //结束连接请求
                    //开始异步接收服务端消息
                    client.BeginReceive(dataBuffer, 0, dataBuffer.Length, SocketFlags.None, new
                        AsyncCallback(ReceiveCallBack), null);
                }, null);

                ConnectStatus = true;
            }
            catch (Exception ex)
            {
                ConnectStatus = false;
                System.Windows.Forms.MessageBox.Show("连接PDCA失败!请检查网络是否连接成功" + ex.Message);                
            }
            return ConnectStatus;
        }

        /// <summary>
        /// 异步接收数据的回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void ReceiveCallBack(IAsyncResult ar)
        {
            ConnectStatus = true;
            try
            {
                int count = client.EndReceive(ar);
                string TerminateString = "\r\n";
                string receiveString = Encoding.Default.GetString(dataBuffer, 0, count);
                int tsLength = TerminateString.Length;
                for (int i = 0; i < receiveString.Length; )
                {
                    if (i <= receiveString.Length - tsLength)
                    {
                        if (receiveString.Substring(i, tsLength) != TerminateString)
                        {
                            sbReceiveDataBuffer.Append(receiveString[i]);
                            i++;
                        }
                        else
                        {
                            //OnRecData(client, state.buffer);
                            this.OnReceiveData(sbReceiveDataBuffer.ToString());
                            sbReceiveDataBuffer.Clear();
                            i += tsLength;
                        }
                    }
                    else
                    {
                        sbReceiveDataBuffer.Append(receiveString[i]);
                        i++;
                    }
                }

                client.BeginReceive(dataBuffer, 0, dataBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), client);
                
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "提示!");

            }
        }

        /// <summary>
        /// 异步发送数据到服务器
        /// </summary>
        /// <param name="msg"></param>
        public void AsyncSend(string msg)
        {
            byte[] buffer = new byte[System.Text.Encoding.UTF8.GetByteCount(msg + EndCode)];
            buffer = System.Text.Encoding.UTF8.GetBytes(msg + EndCode);

            try
            {
                client.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, asyncResult =>
                    {
                        int length = client.EndSend(asyncResult);
                    }, null);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
         
        public void ClosePDCAConnect()
        {
            if (client == null) return;
            if (client.Connected)
            {
                client.Shutdown(SocketShutdown.Both);
                client.Disconnect(true);
            }
            client.Dispose();
            client = null;
        }
        #endregion

        public class StateObject
        {
            public const int BufferSize = 10;

            public byte[] buffer = new byte[BufferSize];

            public StringBuilder sb = new StringBuilder();
        }

        public event EventHandler<ReceiveEventArgs> EventHandlerReceive;
        /// <summary>
        /// 触发接收数据事件
        /// </summary>
        /// <param name="msg"></param>
        private void OnReceiveData(string msg)
        {
            EventHandler<ReceiveEventArgs> handler = EventHandlerReceive;
            if (handler != null)
            {
                handler(null, new ReceiveEventArgs(msg));
            }
        }
        public class ReceiveEventArgs : EventArgs
        {
            public ReceiveEventArgs(string message)
            {
                Message = message;
            }
            public string Message { get; set; }
        }
    }
}
