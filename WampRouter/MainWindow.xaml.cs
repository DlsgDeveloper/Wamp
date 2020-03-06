using System;
using System.Collections.Generic;
using System.Linq;
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
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Router _router = null;


		#region constructor
		public MainWindow()
		{
			InitializeComponent();
		}
		#endregion


		#region Open_Click()
		private void Open_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				_router = new Router();
			}
			catch (Exception ex)
			{
				MessageBox.Show(WampShared.Misc.GetErrorMessage(ex), "", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
		#endregion

		#region Close_Click()
		private void Close_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				_router.Dispose();
			}
			catch (Exception ex)
			{
				MessageBox.Show(WampShared.Misc.GetErrorMessage(ex), "", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
		#endregion

		#region OpenWebSocketMonitor_Click()
		private void OpenWebSocketMonitor_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				new WebSocketWindow().Show();
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

	}
}
