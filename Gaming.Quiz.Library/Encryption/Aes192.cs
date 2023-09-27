using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using  Gaming.Quiz.Library.Utility;

namespace  Gaming.Quiz.Library.Encryption
{
    public class Aes192
    {
        //Below key should have maximum 24 characters = 24 bytes = 192 bits
        static readonly string VIKey = "ollUfFySsj4piOgAmIReIg==";//Cannot contain symbols or special characters
        static readonly string Key = "akAshiSeIjuR0LaWLiETbA==";//Cannot contain symbols or special characters

        static readonly string AesSalt = "cry";

        public static String Base16Encrypt(String plainText)
        {
            if (String.IsNullOrEmpty(plainText))
                return "";

            String cryptoText = Base64Encrypt(plainText);
            cryptoText = Encode.StringToHex(cryptoText, Encoding.Unicode);
            return cryptoText;
        }

        public static String Base16Decrypt(String cryptoText)
        {
            if (String.IsNullOrEmpty(cryptoText))
                return "";

            cryptoText = Encode.HexToString(cryptoText, Encoding.Unicode);
            String plainText = Base64Decrypt(cryptoText);
            return plainText;
        }

        public static String Base64Encrypt(String plainText)
        {
            if (String.IsNullOrEmpty(plainText))
                return "";

            byte[] rawPlaintext = Encoding.Unicode.GetBytes(plainText);
            byte[] cipherText = null;

            using (Aes aes = new AesManaged())
            {
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = Convert.FromBase64String(Key);
                aes.IV = Convert.FromBase64String(VIKey);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(rawPlaintext, 0, rawPlaintext.Length);
                    }
                    cipherText = ms.ToArray();
                }
            }

            String mCryptoText = Convert.ToBase64String(cipherText);
            return mCryptoText;
        }

        public static String Base64Decrypt(String cryptoText)
        {
            if (String.IsNullOrEmpty(cryptoText))
                return "";

            byte[] cipherText = Convert.FromBase64String(cryptoText);
            byte[] plainText = null;

            using (Aes aes = new AesManaged())
            {
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = Convert.FromBase64String(Key);
                aes.IV = Convert.FromBase64String(VIKey);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherText, 0, cipherText.Length);
                    }
                    plainText = ms.ToArray();
                }
            }

            String mPlainText = Encoding.Unicode.GetString(plainText);
            return mPlainText;
        }

        private static String Base64EncryptWSalt(String vPlainText)
        {
            vPlainText += AesSalt;

            byte[] rawPlaintext = Encoding.Unicode.GetBytes(vPlainText);
            byte[] cipherText = null;

            using (Aes aes = new AesManaged())
            {
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = Convert.FromBase64String(Key);
                aes.IV = Convert.FromBase64String(VIKey);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(rawPlaintext, 0, rawPlaintext.Length);
                    }
                    cipherText = ms.ToArray();
                }
            }

            String mCryptoText = Convert.ToBase64String(cipherText);
            return mCryptoText;
        }

        private static String Base64DecryptWSalt(String vCryptoText)
        {
            byte[] cipherText = Convert.FromBase64String(vCryptoText);
            byte[] plainText = null;

            using (Aes aes = new AesManaged())
            {
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = Convert.FromBase64String(Key);
                aes.IV = Convert.FromBase64String(VIKey);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherText, 0, cipherText.Length);
                    }
                    plainText = ms.ToArray();
                }
            }

            String mPlainText = Encoding.Unicode.GetString(plainText);
            mPlainText = mPlainText.Remove(mPlainText.LastIndexOf(AesSalt), AesSalt.Length);
            return mPlainText;
        }
    }
}
