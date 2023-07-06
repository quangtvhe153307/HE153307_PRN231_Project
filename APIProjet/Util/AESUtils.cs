using BusinessObjects;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace APIProject.Util
{
    public interface IAESUtils
    {
        public string Encrypt(User user);
        public User Decrypt(string cipherText);
    }
    public class AESUtils : IAESUtils
    {
        private IConfiguration _config;
        private readonly string key;
        public AESUtils(IConfiguration configuration)
        {
            this._config = configuration;
            key = _config["jwt:secret"];
        }
        public string Encrypt(User user)
        {
            byte[] iv = new byte[16];
            byte[] array;
            string userJson = JsonSerializer.Serialize(user);
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(userJson);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(array);
            //return "";
        }
        public User Decrypt(string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);
            User user = null;
            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.IV = iv;
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                            {
                                user = JsonSerializer.Deserialize<User>(streamReader.ReadToEnd());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return user;
            //return null;
        }
    }
}
