using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WampRouter.Model;

namespace WampRouter
{
	/// <summary>
	/// Interaction logic for WebSocketWindow.xaml
	/// </summary>
	public partial class WebSocketWindow : Window
	{
		WampRouter.WebSockets.WebSocketServer _server;
		WebSockets.WebSocketClientWriter _client;
		WebSockets.WebSocketClientReader _clientReader;
		//WebSockets.WebSocketSession _session;


		#region constructor
		public WebSocketWindow()
		{
			InitializeComponent();

			_server = new WebSockets.WebSocketServer();

			_server.ClientConnected += (sender, e) => { Log(e.Log); };
			_server.ClientDisconnected += (sender, e) => { Log(e.Log); };
			_server.LogRequest += (sender, e) => { Log("server: LOG - " + e.Log); };
		}
		#endregion

		#region RunWebSocketServer_Click()
		private void RunWebSocketServer_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				_server.Listen(8088);
			}
			catch (Exception ex)
			{
				MessageBox.Show(WampShared.Misc.GetErrorMessage(ex), "", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
		#endregion

		#region StopWebSocketServer_Click()
		private void StopWebSocketServer_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				_server.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(WampShared.Misc.GetErrorMessage(ex), "", MessageBoxButton.OK, MessageBoxImage.Error);
			}

		}
		#endregion

		#region Log()
		private void Log(string log)
		{
			try
			{
				this.Dispatcher.BeginInvoke((Action)delegate ()
				{
					textBlockLog.Text += log + Environment.NewLine;
				});
			}
			catch (Exception ex)
			{
				MessageBox.Show(WampShared.Misc.GetErrorMessage(ex), "", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
		#endregion

		#region StartClient_Click()
		private void StartClient_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				_client = new WebSockets.WebSocketClientWriter();
				_client.LogRequest += (s, args) => { Log("Client Writer LOG: " + args.Log); };
				_client.Start(8088);
			}
			catch (Exception ex)
			{
				MessageBox.Show(WampShared.Misc.GetErrorMessage(ex), "", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
		#endregion

		#region StopClient_Click()
		private void StopClient_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				_client.Stop();
			}
			catch (Exception ex)
			{
				MessageBox.Show(WampShared.Misc.GetErrorMessage(ex), "", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
		#endregion

		#region SendClientMessage_Click()
		private void SendClientMessage_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				_client.SendMessage(textBoxSend.Text);
			}
			catch (Exception ex)
			{
				MessageBox.Show(WampShared.Misc.GetErrorMessage(ex), "", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
		#endregion

		#region StartClientReader_Click()
		private void StartClientReader_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				_clientReader = new WebSockets.WebSocketClientReader();
				_clientReader.LogRequest += (s, args) => { Log("Client Reader LOG: " + args.Log); };
				_clientReader.Start(8088);
			}
			catch (Exception ex)
			{
				MessageBox.Show(WampShared.Misc.GetErrorMessage(ex), "", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
		#endregion

		#region StopClientReader_Click()
		private void StopClientReader_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				_clientReader.Stop();
			}
			catch (Exception ex)
			{
				MessageBox.Show(WampShared.Misc.GetErrorMessage(ex), "", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
		#endregion

	}
}
