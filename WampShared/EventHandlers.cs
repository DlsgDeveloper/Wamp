using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WampShared
{
	public class LogEventArgs : EventArgs
	{
		public string Log { get; }

		public LogEventArgs(string log)
		{
			this.Log = log;
		}
	}
}
