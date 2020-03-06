using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WampSharp.V2;
using WampSharp.V2.Realm;

namespace WampRouter.Model
{
    /// <summary>
    /// https://wamp-proto.org/_static/gen/wamp_latest.html#predefined-uris
    /// https://wampsharp.net/wamp2/getting-started-with-wampv2/
    /// </summary>
	public class Router : IDisposable
	{
		const string location = "ws://127.0.0.1:8080/";
        IWampHost _host;


        public Router()
        {
            _host = new DefaultWampHost(location);

            IArgumentsService instance = new ArgumentsService();

            IWampHostedRealm realm = _host.RealmContainer.GetRealmByName("realm1");

            Task<IAsyncDisposable> registrationTask = realm.Services.RegisterCallee(instance);
            // await registrationTask;
            registrationTask.Wait();

            _host.Open();

            //Console.WriteLine("Server is running on " + location);
            //Console.ReadLine();
        }


        // PUBLIC METHODS
        #region public methods

        #region Dispose()
        public void Dispose()
		{
            if (_host != null)
            {
                _host.Dispose();
                _host = null;
            }
        }
		#endregion

		#endregion


	}
}
