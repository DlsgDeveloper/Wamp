using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using WampShared;

namespace WampRouter.WebSockets
{
    class WebSocketClientWriter
	{
		TcpClient	        _tcpClient;
		NetworkStream       _stream = null;

        public event EventHandler<LogEventArgs> LogRequest;


        // PUBLIC PROPERTIES
        #region public properties

        public string Id { get; }
        public bool IsMasking { get { return false; } }

        #endregion


        // PUBLIC METHODS
        #region public methods

        #region Start()
        public void Start(int port)
		{
			ThreadPool.QueueUserWorkItem(_ =>
			{

				_tcpClient = new TcpClient("127.0.0.1", port);
                LogRequest?.Invoke(this, new LogEventArgs("Connection was established...."));

                _stream = _tcpClient.GetStream();
			});
		}
		#endregion

		#region Stop()
		public void Stop()
		{
			_tcpClient.Close();
			_stream.Close();

            LogRequest?.Invoke(this, new LogEventArgs("Client writer stopped."));
        }
        #endregion

        #region SendMessage()
        public void SendMessage(string textToSend)
        {
            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);

            // ---send the text---
            Console.WriteLine("Sending : " + textToSend);
            _stream.Write(bytesToSend, 0, bytesToSend.Length);
            /*
            Byte[] response = new Byte[_tcpClient.ReceiveBufferSize];
            int bytesReturned = _stream.Read(response, 0, (int)_tcpClient.ReceiveBufferSize);

            string returnData = Encoding.UTF8.GetString(response, 0, bytesReturned);

            LogRequest?.Invoke(this, new LogEventArgs("Server Response: " + returnData));*/
        }
        #endregion

        #endregion

    }
}
