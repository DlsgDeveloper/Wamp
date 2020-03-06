using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WampShared;

namespace WampRouter.WebSockets
{
    public class WebSocketServer
    {
        private bool _listening = false;
        NetworkStream _clientStreamWriter;
        NetworkStream _clientStreamReader;

        CancellationTokenSource _cancellationTokenSource;

        public event EventHandler<LogEventArgs>     ClientConnected;
        public event EventHandler<LogEventArgs>     ClientDisconnected;
        public event EventHandler<LogEventArgs>     LogRequest;


        // PUBLIC METHODS
        #region public methods

        #region Listen()
        public void Listen(int port)
        {
            if (_listening) 
                throw new Exception("Already listening!");

            _listening = true;

            TcpListener tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();

            ClientConnected?.Invoke(this, new LogEventArgs($"Server has started on 127.0.0.1:{port}. Waiting for a connection..."));

            ThreadPool.QueueUserWorkItem(_ =>
            {
                TcpClient tcpClientWriter = tcpListener.AcceptTcpClient();
                TcpClient tcpClientReader = tcpListener.AcceptTcpClient();

                LogRequest?.Invoke(this, new LogEventArgs("Client connected."));

                _clientStreamWriter = tcpClientWriter.GetStream();
                byte[] bufferWriter = new byte[tcpClientWriter.ReceiveBufferSize];

                _clientStreamReader = tcpClientReader.GetStream();
                byte[] bufferReader = new byte[tcpClientReader.ReceiveBufferSize];


                while (_listening)
                {
                    //---read incoming stream---
                    int bytesRead;

                    try
                    {
                        using (_cancellationTokenSource = new CancellationTokenSource())
                        {
                            using (_cancellationTokenSource.Token.Register(() => _clientStreamWriter.Close()))
                            {
                                bytesRead = _clientStreamWriter.Read(bufferWriter, 0, tcpClientWriter.ReceiveBufferSize);
                               
                                if (bytesRead > 0)
                                {
                                    //---convert the data received into a string---
                                    string dataReceived = Encoding.ASCII.GetString(bufferWriter, 0, bytesRead);
                                    LogRequest?.Invoke(this, new LogEventArgs("Received : " + dataReceived));

                                    //---write back the text to the client---
                                    if (dataReceived.Length > 0)
                                    {
                                        byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(dataReceived);
                                        LogRequest?.Invoke(this, new LogEventArgs("Sending back : " + dataReceived));
                                        _clientStreamReader.Write(bytesToSend, 0, bytesToSend.Length);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (_listening)
                            throw;
                        else
                        {
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes("q");
                            LogRequest?.Invoke(this, new LogEventArgs("Closing Client Reader..."));
                            _clientStreamReader.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                    }

                }

                tcpClientWriter.Close();
                tcpClientReader.Close();
                tcpListener.Stop();
                _clientStreamReader.Flush();
                _clientStreamReader.Dispose();
                _clientStreamWriter.Dispose();

                ClientDisconnected?.Invoke(this, new LogEventArgs("Server stopped."));
            });
        }
        #endregion

        #region Close()
        public void Close()
        {
            if (_listening)
            {
                _listening = false;

                _cancellationTokenSource.Cancel();
            }
        }
        #endregion

        #endregion

    }
}
