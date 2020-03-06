using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WampCallee.Model;
using WampSharp.V2;
using WampSharp.V2.Client;

namespace WampClient.Model
{
	class Calleee
	{
		const string location = "ws://127.0.0.1:8080/";
        IWampHost _host;


        public Calleee()
        {
            DefaultWampChannelFactory channelFactory = new DefaultWampChannelFactory();

            IWampChannel channel = channelFactory.CreateJsonChannel(location, "realm1");

            Task openTask = channel.Open();

            // await openTask;
            openTask.Wait();

            IArgumentsService instance = new ArgumentsService();

            IWampRealmProxy realm = channel.RealmProxy;

            Task<IAsyncDisposable> registrationTask = realm.Services.RegisterCallee(instance);
            // await registrationTask;
            registrationTask.Wait();

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
