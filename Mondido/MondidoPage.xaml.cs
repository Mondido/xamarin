﻿using System;
using System.Collections.Generic;
using Mondido.Base;
using Newtonsoft.Json.Linq;
using Plugin.DeviceInfo;
using Xamarin.Forms;

namespace Mondido
{
	public partial class MondidoPage : ContentPage
	{

		Base.Payment payment;
		/// <summary>
		/// Success callback.
		/// </summary>
		/// <param name="p">List of paramas comping from the URL</param>
		void OnSuccess(WebParams p)
		{
			browser.IsVisible = false;
			DisplayAlert("Success", "Payment is done!", "Thanks");	
			//continue with your business here
		}

		/// <summary>
		/// Fail callback
		/// </summary>
		/// <param name="p">List of paramas comping from the URL</param>
		void OnFail(WebParams p)
		{
			DisplayAlert("Failed", "Payment is failed!", "Ok");
			payment.ExecuteHostedPayment(); //try again
		}

		public MondidoPage()
		{
			InitializeComponent();
			WebParams data = new WebParams();

			//Your account settings, fetch it from https://admin.mondido.com/en/settings#api
			var merchantId = "233";
			var secret = "$2a$10$gU.z.9QNc8VSGYqcJSOhv."; ///Shh, should be stored in your backend!
			var paymentRef = Guid.NewGuid().ToString();
			var customerRef = "123";
			var amount = "10.00";
			var currency = "eur";


			//Find out what to send in the documentation
			//https://doc.mondido.com/hosted#outgoing
			data.Add("payment_ref", paymentRef);
			data.Add("customer_ref", customerRef);
			data.Add("amount", amount);
			data.Add("currency", currency);
			data.Add("merchant_id", merchantId);
			data.Add("success_url", "https://api.mondido.com/success");// need to be a working URL for IOS
			data.Add("error_url", "https://api.mondido.com/fail");// need to be a working URL for IOS
			data.Add("test", "true");
			data.Add("authorize", "false");
			data.Add("store_card", "false");

			var device = @"{model: '"+CrossDeviceInfo.Current.Model+"', platform: '"+CrossDeviceInfo.Current.Platform+"', version: '"+CrossDeviceInfo.Current.Version+"', id: '"+CrossDeviceInfo.Current.Id+"'}";

			string metadataStr = @"{ customer: {name: 'Tester', email: 'name@company.com'}, device: "+device+"}";
			var metadata = JObject.Parse(metadataStr);
			data.Add("metadata", System.Net.WebUtility.UrlEncode(metadata.ToString()));

			string itemsStr = "[{artno: '"+Guid.NewGuid().ToString()+"', description: 'An item', amount: '10.00', vat: '25.00', qty: '1' }]";
			var items = JArray.Parse(itemsStr);
			data.Add("items", System.Net.WebUtility.UrlEncode(items.ToString()));


			// The hash *should* be generated in your backend for security reasons.
			// Recipe: MD5(merchant_id + payment_ref + customer_ref + amount + currency + test + secret)
			var hash = (merchantId + paymentRef + customerRef + amount + currency + "test" + secret); 
			data.Add("hash", hash.ToMD5());

			//Init the payment object
			payment = new Mondido.Base.Payment(
				browser,
				data,
				OnSuccess,
				OnFail
			);

			//execute the payment
			payment.ExecuteHostedPayment();
		}
	}
}
