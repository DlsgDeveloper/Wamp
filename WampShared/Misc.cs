using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WampShared
{
	public class Misc
	{
		public static string GetErrorMessage(Exception ex)
		{
			string error = ex.Message;

			while ((ex = ex.InnerException) != null)
				error += Environment.NewLine + ex.Message;

			return error;
		}
	}
}
