using System;
using System.Collections.Generic;
using Mondido.Base;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace Mondido
{
	public partial class MondidoPage : ContentPage
	{

		Base.Pay payment;
		void OnSuccess(WebParams p)
		{
			browser.IsVisible = false;
			DisplayAlert("Success", "Payment is done!", "Thanks");	
		}

		void OnFail(WebParams p)
		{
			DisplayAlert("Failed", "Payment is failed!", "Ok");
			payment.ExecuteHostedPayment();
		}

		public MondidoPage()
		{
			InitializeComponent();
			WebParams data = new WebParams();
			// MD5(merchant_id + payment_ref + customer_ref + amount + currency + test + secret)
			var merchantId = "233";
			var secret = "$2a$10$gU.z.9QNc8VSGYqcJSOhv.";
			var paymentRef = Guid.NewGuid().ToString();
			var customerRef = "123";
			var amount = "10.00";
			var currency = "eur";

			data.Add("payment_ref", paymentRef);
			data.Add("customer_ref", customerRef);
			data.Add("amount", amount);
			data.Add("currency", currency);
			data.Add("merchant_id", merchantId);
			data.Add("success_url", "https://mywebsite.com/payment_success?data=value&data2=value2");
			data.Add("error_url", "https://mywebsite.com/payment_success?data=value&data2=value2");
			data.Add("test", "true");
			data.Add("authorize", "false");
			data.Add("store_card", "false");

			string metadataStr = @"{ customer: {name: 'Tester', email: 'name@company.com'} }";
			var metadata = JObject.Parse(metadataStr);
			data.Add("metadata", metadata.ToString());

			string itemsStr = "[{artno: '"+Guid.NewGuid().ToString()+"', description: 'An item', amount: '10.00', vat: '25.00', qty: '1' }]";
			var items = JArray.Parse(itemsStr);
			data.Add("items", items.ToString());

			var hash = (merchantId + paymentRef + customerRef + amount + currency + "test" + secret);
			data.Add("hash", hash.ToMD5());
			payment = new Pay(
				browser,
				data,
				OnSuccess,
				OnFail
			);

			payment.ExecuteHostedPayment();
		}
	}
}
