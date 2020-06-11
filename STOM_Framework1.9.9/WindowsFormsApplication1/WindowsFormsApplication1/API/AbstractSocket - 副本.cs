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

        private bool _cognexStatus;

        public bool CognexStatus
        {
            get { return _cognexStatus; }
            set { _cognexStatus = value; }
        }

        public event EventHandler<byte[]> OnRecData; //定义一个委托类型的事件  
        private StringBuilder sbReceiveDataBuffer = new StringBuilder();

        public Socket client;
        private byte[] dataBuffer = new byte[1024];
        bool Listening = false;
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
                CognexStatus = client.Connected;
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(string.Format("连接{0}:{1}失败!请检查网络是否连接成功",Ip,Port) + ex.Message);
            }

        }


        public void GetStatus()
        {
            CognexStatus = client.Connected;
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

                if (!CognexStatus)
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
        public void InitSocketAsynchronous()
        {
            try
            {
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                client.SendTimeout = 1000;
                
                IPEndPoint remotePoint = new IPEndPoint(Ip, Port);
                client.Bind(remotePoint);
                client.Listen(10);
                AsyncAccept(client);

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(string.Format("连接{0}:{1}失败!请检查网络是否连接成功", Ip, Port) + ex.Message);
            }
        }


        public void AsyncAccept(Socket client)
        {
            client.BeginAccept(asyncResult =>
            {
                client.EndAccept(asyncResult);
                AsyncSend("Hello!");
                System.Windows.Forms.MessageBox.Show( AsyncReceive());
            }, client);
        }

        private void CallBackConnect(IAsyncResult asy)
        {
            try
            {
                client.EndConnect(asy); //结束连接请求
                //开始异步接收服务端消息
                client.BeginReceive(dataBuffer, 0, dataBuffer.Length, SocketFlags.None, new
                    AsyncCallback(CallBackRecieve), null);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("异步连接服务器失败,请检查网线是否插好" + ex.Message, "提示!");
                //client.Close();               
            }
        }

        private void ReceiveCallBack(IAsyncResult ar)
        {
            CognexStatus = true;
            try
            {
                StateObject state = (StateObject)ar.AsyncState;
                Socket handler = state.workSocket;
                int read = handler.EndReceive(ar);
             
                if (Listening && read > 0)
                {
                   
                    //在此次可以对data进行按需处理
                    OnRecData(client,state.buffer);

                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallBack), state);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "提示!");

            }
        }
        private void CallBackRecieve(IAsyncResult iar)
        {
            try
            {
                int count = client.EndReceive(iar);
                string receiveString = Encoding.Default.GetString(dataBuffer, 0, count);
                int tsLength = EndCode.Length;
                for (int i = 0; i < receiveString.Length; )
                {
                    if (i <= receiveString.Length - tsLength)
                    {
                        if (receiveString.Substring(i, tsLength) != EndCode)
                        {
                            sbReceiveDataBuffer.Append(receiveString[i]);
                            i++;
                        }
                        else
                        {
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
                client.BeginReceive(dataBuffer, 0, Buffer.Length, SocketFlags.None, new AsyncCallback(CallBackRecieve), client);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("异步接受数据出现异常,断开连接" + ex.Message, "提示!");
                //CloseConnect();
            }
        }


    

        public void AsyncSend(string msg)
        {
            ClearBuffer();
            Buffer = System.Text.Encoding.UTF8.GetBytes(msg + EndCode);
            try
            {
                client.BeginSend(Buffer, 0, Buffer.Length, SocketFlags.None, asyncResult =>
                    {
                       int length = client.EndSend(asyncResult);
                    }, null);
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        public string AsyncReceive()
        {

            ClearBuffer();
            try
            {
                client.BeginReceive(Buffer,0,Buffer.Length,SocketFlags.None,asyncResult=>
                    {
                        int length = client.EndReceive(asyncResult);
                    }
                     , client );
               
            }
            catch(Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            return string.Format("%s", Buffer);
        }


        private void CallBackSend(IAsyncResult iar)
        {
            try
            {
                //结束挂起的异步发送，并返回发送的字节数
                ((Socket)iar.AsyncState).EndSend(iar);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("异步Send数据出现异常" + ex.Message, "提示!");
            }
        }

        /// <summary>
        /// 接受到数据时触发
        /// </summary>
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
        #endregion

        public class StateObject
        {
            public Socket workSocket = null;

            public const int BufferSize = 100;

            public byte[] buffer = new byte[BufferSize];

            public StringBuilder sb = new StringBuilder();
        }
        

    }
}
