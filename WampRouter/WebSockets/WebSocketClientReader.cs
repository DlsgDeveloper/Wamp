using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using WampShared;

namespace WampRouter.WebSockets
{
	public class WebSocketClientReader
	{
        TcpClient       _tcpClient;
        NetworkStream   _stream = null;

        private bool    _listening = true;

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
                Byte[] response = new Byte[_tcpClient.ReceiveBufferSize];

                while (_listening)
                {
                    int bytesRead = _stream.Read(response, 0, (int)_tcpClient.ReceiveBufferSize);

                    if (bytesRead > 0)
                    {
                        string returnData = Encoding.UTF8.GetString(response, 0, bytesRead);

                        LogRequest?.Invoke(this, new LogEventArgs("Server Response: " + returnData));

                        if (returnData == "q")
                            _listening = false;
                    }
                }

                _tcpClient.Close();
                _stream.Close();

                LogRequest?.Invoke(this, new LogEventArgs("Client reader stopped."));
            });
        }
        #endregion

        #region Stop()
        public void Stop()
        {
        }
        #endregion

        #endregion
    
    }
}
