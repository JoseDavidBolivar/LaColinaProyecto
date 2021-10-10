using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ColinaApplication.Data.Clases
{
    public class Encriptacion
    {

        string Llave;
        string Vector;
        public Encriptacion()
        {
            Llave = "29d4e6606cbc7ba4fad305c8eeea1c27";
            Vector = "86f392ef0867c7e2";
        }
        public byte[] Encriptar(string Texto)
        {
            byte[] encrypted;
            using (Aes aesAlg = Aes.Create())
            {
                var x = Encoding.ASCII.GetBytes(Vector);
                aesAlg.Key = Encoding.ASCII.GetBytes(Llave);
                aesAlg.IV = Encoding.ASCII.GetBytes(Vector);

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(Texto);
                        }
                        var stringg = Convert.ToBase64String(msEncrypt.ToArray());
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;

        }

        public string DesEncriptar(byte[] Texto)
        {
            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.ASCII.GetBytes(Llave);
                aesAlg.IV = Encoding.ASCII.GetBytes(Vector);

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Texto))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
    }
}