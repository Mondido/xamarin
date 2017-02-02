using System.Text;
using PCLCrypto;

namespace Mondido.Base
{
	public static class Extensions
	{
		/// <summary>
		/// String to MD5
		/// </summary>
		/// <returns>The hash.</returns>
		/// <param name="s">String</param>
		public static string ToMD5(this string s)
		{
			IHashAlgorithmProvider algoProv = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Md5);
			byte[] dataTB = Encoding.UTF8.GetBytes(s);
			return ByteArrayToHex(algoProv.HashData(dataTB));
		}

		private static string ByteArrayToHex(byte[] hash)
		{
			var hex = new StringBuilder(hash.Length * 2);
			foreach (byte b in hash)
				hex.AppendFormat("{0:x2}", b);

			return hex.ToString();
		}
	}


}