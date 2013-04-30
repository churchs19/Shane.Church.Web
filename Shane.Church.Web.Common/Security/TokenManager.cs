using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Shane.Church.Web.Common.Security
{
	public static class TokenManager
	{
		private static object _encryptLockObject = new object();
		private static object _decryptLockObject = new object();
		private static int _saltLength = 16;

		public static string EncodeToken(string identity)
		{
			byte[] pwBytes = Encoding.Unicode.GetBytes("R0ckies2o12!");
			//			byte[] ivBytes = Convert.FromBase64String(ConfigurationManager.AppSettings["TokenEncryptionIV"]);
			byte[] cipherTextBytes = null;
			byte[] concatBytes = null;
			byte[] saltBytes = GenerateKeyGenerateRandomBytes(_saltLength);

			try
			{
				string credential = identity;

				// Make cryptographic operations thread-safe
				lock (_encryptLockObject)
				{
					Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(pwBytes, saltBytes, 10);

					Aes aesCrypt = AesManaged.Create();
					aesCrypt.IV = rfc2898DeriveBytes.GetBytes(aesCrypt.BlockSize / 8);
					aesCrypt.Key = rfc2898DeriveBytes.GetBytes(aesCrypt.KeySize / 8);

					// Encryption will be performed using memory stream
					using (MemoryStream memoryStream = new MemoryStream())
					{
						// To perform encryption, use Write mode
						using (CryptoStream cryptoStream = new CryptoStream(
														   memoryStream,
														   aesCrypt.CreateEncryptor(),
															CryptoStreamMode.Write))
						{

							// Start encrypting data
							cryptoStream.Write(Encoding.Unicode.GetBytes(credential),
												0,
											   Encoding.Unicode.GetByteCount(credential));

							// Finish the encryption operation
							cryptoStream.FlushFinalBlock();
						}

						// Move encrypted data from memory into a byte array
						cipherTextBytes = memoryStream.ToArray();
					}

					// Return encrypted data
					concatBytes = new byte[saltBytes.Length + cipherTextBytes.Length];
					System.Buffer.BlockCopy(saltBytes, 0, concatBytes, 0, saltBytes.Length);
					System.Buffer.BlockCopy(cipherTextBytes, 0, concatBytes, saltBytes.Length, cipherTextBytes.Length);
					return Convert.ToBase64String(concatBytes);
				}
			}
			finally
			{
				//Clean in memory byte arrays
				ClearBytes(pwBytes);
				ClearBytes(saltBytes);

				if (cipherTextBytes != null)
					ClearBytes(cipherTextBytes);
				if (concatBytes != null)
					ClearBytes(concatBytes);
			}
		}

		public static string DecodeToken(string encryptedToken)
		{
			byte[] pwBytes = Encoding.Unicode.GetBytes("R0ckies2o12!");
			byte[] tokenBytes = Convert.FromBase64String(encryptedToken);
			byte[] cipherTextBytes = GetCipherTextBytesFromToken(tokenBytes, _saltLength);
			byte[] saltBytes = GetSaltBytesFromToken(tokenBytes, _saltLength);
			string credentials = null;
			string[] credentialsArray = null;

			try
			{
				// Make cryptographic operations thread-safe
				lock (_decryptLockObject)
				{
					Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(pwBytes, saltBytes, 10);

					Aes aesCrypt = AesManaged.Create();
					aesCrypt.IV = rfc2898DeriveBytes.GetBytes(aesCrypt.BlockSize / 8);
					aesCrypt.Key = rfc2898DeriveBytes.GetBytes(aesCrypt.KeySize / 8);

					// Encryption will be performed using memory stream
					using (MemoryStream memoryStream = new MemoryStream())
					{
						// To perform encryption, use Write mode
						using (CryptoStream cryptoStream = new CryptoStream(memoryStream,
																			aesCrypt.CreateDecryptor(),
																			CryptoStreamMode.Write))
						{
							// Start decrypting data
							cryptoStream.Write(cipherTextBytes,
												0,
											   cipherTextBytes.Length);

							// Finish the encryption operation
							cryptoStream.FlushFinalBlock();
						}

						// Move encrypted data from memory into a byte array
						credentials = Encoding.Unicode.GetString(memoryStream.ToArray());
					}
					return credentials;
				}
			}
			finally
			{
				//Clean in memory byte arrays
				ClearBytes(pwBytes);
				ClearBytes(saltBytes);
				ClearBytes(tokenBytes);
				ClearBytes(cipherTextBytes);
				credentials = null;
				if (credentialsArray != null)
					ClearStrings(credentialsArray);
			}

			throw new NotImplementedException();
		}

		private static byte[] GenerateKeyGenerateRandomBytes(int length)
		{
			byte[] key = new byte[length];
			RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
			provider.GetBytes(key);
			return key;
		}

		private static byte[] GetSaltBytesFromToken(byte[] tokenBytes, int length)
		{
			byte[] bytes = new byte[length];
			System.Buffer.BlockCopy(tokenBytes, 0, bytes, 0, length);
			return bytes;
		}

		private static byte[] GetCipherTextBytesFromToken(byte[] tokenBytes, int saltLength)
		{
			byte[] bytes = new byte[tokenBytes.Length - saltLength];
			System.Buffer.BlockCopy(tokenBytes, saltLength, bytes, 0, tokenBytes.Length - saltLength);
			return bytes;
		}

		private static void ClearBytes(byte[] buffer)
		{
			// Check arguments. 
			if (buffer == null)
			{
				throw new ArgumentException("buffer");
			}

			// Set each byte in the buffer to 0. 
			for (int x = 0; x < buffer.Length; x++)
			{
				buffer[x] = 0;
			}
		}

		private static void ClearStrings(string[] buffer)
		{
			// Check arguments. 
			if (buffer == null)
			{
				throw new ArgumentException("buffer");
			}

			for (int i = 0; i < buffer.Length; i++)
			{
				buffer[i] = null;
			}
		}
	}
}
