using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Mondido.Base
{
	public class Pay
	{
		private WebView browser;
		private Action<WebParams> onSuccessCallback;
		private Action<WebParams> onFailCallback;
		private Action<WebParams> onAuthorizeCallback;
		private WebParams dataParams;

		public Pay(WebView b, 
		           WebParams data,
		           Action<WebParams> onSuccess,
				   Action<WebParams> onFail,
  					Action<WebParams> onAuthorize = null

		)
		{
			this.browser = b;
			this.onSuccessCallback = onSuccess;
			this.onFailCallback = onFail;
			this.onAuthorizeCallback = onAuthorize;
			this.dataParams = data;
			this.browser.Navigated += this.OnChange;
		}

		public void ExecuteHostedPayment(WebParams prm=null)
		{
			if (prm != null)
			{
				this.dataParams = prm;
			}

			var source = new HtmlWebViewSource();
			var sb = new StringBuilder();
			sb.Append("<html><body><form action=\"https://pay.mondido.com/v1/form\" method=\"post\">");

			//add device data. lang, hardware, software, etc.

			foreach (KeyValuePair<string, string> p in this.dataParams)
			{
				sb.Append("<input type=\"hidden\" name=\""+p.Key+"\" value=\""+p.Value+"\">");
			}
			sb.Append("<script type=\"text/javascript\">document.forms[0].submit();</script>");
			sb.Append("</body></html>");

			source.Html = sb.ToString();
			browser.Source = source;
		}

		public void OnChange(object sender, Xamarin.Forms.WebNavigatedEventArgs e)
		{

			var url = e.Url;
			var prms = WebParams.FromUrl(url);
			if (prms == null)
			{
				return;
			}
			if (prms.Status == Param.APPROVED)
			{
				this.onSuccessCallback(prms);
				return;
			}

			if (prms.Status == Param.AUTHORIZED)
			{
				this.onAuthorizeCallback(prms);
				return;
			}

			if (prms.Status == Param.FAILED || prms.Status == Param.DECLINED)
			{
				this.onFailCallback(prms);
				return;
			}
			if (prms.Status == Param.UNKNOWN)
			{
				return;
			}
		}

	}
}
