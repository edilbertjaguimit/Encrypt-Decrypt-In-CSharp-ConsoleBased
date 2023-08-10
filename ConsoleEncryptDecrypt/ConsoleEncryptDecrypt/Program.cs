using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleEncryptDecrypt
{
    class Program
    {
        static void Main(string[] args)
        {
            EncryptDecrypt ed = new EncryptDecrypt("Edilbert");
            ed.Encrypt();
            ed.Decrypt();
        }
    }

    class EncryptDecrypt
    {
        private string hash = "f0xle@rn";
        public string txtValue { get; set; }
        private string txtEncrypt { get; set; }
        private string txtDecrypt { get; set; }

        public EncryptDecrypt(string value)
        {
            this.txtValue = value;
        }

        public void Encrypt()
        {
            byte[] data = UTF8Encoding.UTF8.GetBytes(txtValue);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider()
                { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateEncryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    txtEncrypt = Convert.ToBase64String(results, 0, results.Length);
                }
            }
            Console.WriteLine(txtEncrypt);
        }

        public void Decrypt()
        {
            byte[] data = Convert.FromBase64String(txtEncrypt);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider()
                { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateDecryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    txtDecrypt = UTF8Encoding.UTF8.GetString(results);
                }
            }
            Console.WriteLine(txtDecrypt);
        }
    }
}
