using System;
using System.Collections.Generic;

namespace Mondido.Base
{
	public class WebParams : Dictionary<string,string> 
	{
		public Param Status
		{
			get 
			{
				if(!this.ContainsKey("status"))
				{
					return Param.UNKNOWN;
				}
				try
				{
					return (Mondido.Base.Param)Enum.Parse(typeof(Param), this["status"].ToUpper());
				}
				catch
				{
					return Param.UNKNOWN;
				}
			}
		}

		public static WebParams FromUrl(string url) 
		{
			var pms = new WebParams();
			var param = url.Split('?');
			if (param.Length < 2)
			{
				return pms;
			}
			var arr = param[1].Split('&');

			foreach (string s in arr)
			{
				var item = s.Split('=');
				if (item.Length > 1)
				{
					string thisKey = item[0].Replace("?", "");
					if (pms.ContainsKey(thisKey))
					{
						pms[thisKey] = item[1];
					}
					else
					{
						pms.Add(thisKey, item[1]);
					}
				}

			}
			return pms;
		}


		internal string GetVal(string key)
		{
			if (this.ContainsKey(key)) 
			{
				return this[key];
			}
			return string.Empty;
		}

	}
}
