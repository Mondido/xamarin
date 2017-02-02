using System;
using System.Collections.Generic;

namespace Mondido.Base
{
	public class WebParams :List<KeyValuePair<string, string>>
	{
		public Param Status
		{
			get 
			{
				var s = GetVal("status");
				if (s == string.Empty)
				{
					return Param.UNKNOWN;
				}
				try
				{
					return (Mondido.Base.Param)Enum.Parse(typeof(Param), s.ToUpper());
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
					pms.Add(item[0].Replace("?",""), item[1]);
				}
			}
			return pms;
		}

		internal void Add(string key, string val)
		{
			var item = Get(key);
			if (item != null) 
			{
				this.Remove(new KeyValuePair<string, string>(item.Value.Key, item.Value.Value));

			}
			this.Add(new KeyValuePair<string, string>(key, val));
			         
		}

		internal string GetVal(string key)
		{
			var item = this.Get(key);
			if (item.HasValue)
			{
				return item.Value.Value;
			}
			return string.Empty;
		}

		internal KeyValuePair<string, string>? Get(string key)
		{
			foreach (KeyValuePair<string, string> item in this)
			{
				if (item.Key == key)
				{
					return item;
				}
			}
			return null;
		}
	}
}
