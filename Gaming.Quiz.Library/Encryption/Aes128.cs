using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace  Gaming.Quiz.Library.Encryption
{
    public class Aes128
    {
        //Below key should have maximum 16 characters = 16 bytes = 128 bits
        //Unicode-8: 1 character = 8 bits = 1 byte.
        static readonly string IVKey = "vEgEt@4+t$unAE!=";
        static readonly string Key = "k!rIt@hEiBl@CK/=";

        //Encoding should be UTF8 only for 128 bits encoding
        static readonly Encoding encoding = Encoding.UTF8;


        public static string Base16Encrypt(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = encoding.GetBytes(Key); ;
                aes.IV = encoding.GetBytes(IVKey); ;

                byte[] encrypted = EncryptStringToBytes(plainText, aes.Key, aes.IV);

                return Convert.ToBase64String(encrypted);
            }
        }


        static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    return msEncrypt.ToArray();
                }
            }

        }
        public static string Base16Decrypt(string cipher)
        {
            Encoding encoding = Encoding.UTF8;
            using (Aes aes = Aes.Create())
            {
                aes.IV = encoding.GetBytes(IVKey);
                aes.Key = encoding.GetBytes(Key);

                byte[] encrypted = EncryptStringToBytes(cipher, aes.Key, aes.IV);
                string decrypted = DecryptStringFromBytes(encrypted, aes.Key, aes.IV);

                return Convert.ToBase64String(encrypted);
            }
        }

        private static string ByteArrayToString(byte[] bytes)
        {
            return BitConverter.ToString(bytes).Replace("-", string.Empty);
        }

        private static byte[] StringToByteArray(string hex)
        {
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];

            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }

    }
}
