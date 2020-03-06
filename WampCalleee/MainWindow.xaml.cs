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
using WampCallee.Model;
using WampSharp.V2;
using WampSharp.V2.Client;

namespace WampCallee
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		const string location = "ws://127.0.0.1:8080/";


		public MainWindow()
		{
			InitializeComponent();
		}

		private async void OpenConnection_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				DefaultWampChannelFactory channelFactory = new DefaultWampChannelFactory();

				IWampChannel channel = channelFactory.CreateJsonChannel(location, "realm1");

				IWampRealmProxy realmProxy = channel.RealmProxy;

				await channel.Open();

				// Host WAMP application components

				// await openTask;
				//openTask.Wait();

				IArgumentsService instance = new ArgumentsService();

				IWampRealmProxy realm = channel.RealmProxy;

				Task<IAsyncDisposable> registrationTask = realm.Services.RegisterCallee(instance);
				// await registrationTask;
				registrationTask.Wait();
			}
			catch (Exception ex)
			{
				MessageBox.Show(WampShared.Misc.GetErrorMessage(ex), "", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
	}
}
