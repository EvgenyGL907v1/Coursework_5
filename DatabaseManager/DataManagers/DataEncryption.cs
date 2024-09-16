using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DatabaseManager
{
	/// <summary> Шифрование строки </summary>
	public static class DataEncryption
	{
		private const string Key = "SuperSecretKey12345678901234567890123";

		public static string Encrypt(string plainText)
		{
			byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
			byte[] keyBytes = Encoding.UTF8.GetBytes(Key);

			byte[] cipherBytes = new byte[plainBytes.Length];

			for (int i = 0; i < plainBytes.Length; i++)
			{
				cipherBytes[i] = (byte)(plainBytes[i] ^ keyBytes[i % keyBytes.Length]);
			}

			return Convert.ToBase64String(cipherBytes);
		}

		public static string Decrypt(string cipherText)
		{
			byte[] cipherBytes = Convert.FromBase64String(cipherText);
			byte[] keyBytes = Encoding.UTF8.GetBytes(Key);

			byte[] plainBytes = new byte[cipherBytes.Length];

			for (int i = 0; i < cipherBytes.Length; i++)
			{
				plainBytes[i] = (byte)(cipherBytes[i] ^ keyBytes[i % keyBytes.Length]);
			}

			return Encoding.UTF8.GetString(plainBytes);
		}
	}
}
