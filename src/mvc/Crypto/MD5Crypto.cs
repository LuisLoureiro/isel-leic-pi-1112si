using System.Security.Cryptography;
using System.Text;

namespace mvc.Crypto
{
    public class MD5Crypto
    {
        public static string GenerateMD5(string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] byteArray = Encoding.ASCII.GetBytes(str);

            byteArray = md5.ComputeHash(byteArray);

            StringBuilder hashedValue = new StringBuilder();

            foreach (byte b in byteArray)
            {
                // x2 - Traduz o valor do byte para o seu valor em hexadecimal de 8 bits.
                hashedValue.Append(b.ToString("x2"));
            }

            return hashedValue.ToString();
        }
    }
}