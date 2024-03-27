using System.Text;

namespace Membership
{
    internal class Decoding
    {

        public static string GetDecodedPassword()
        {
            string encodedPassword = GetEncodedPassword();
            byte[] decodedBytes = Convert.FromBase64String(encodedPassword);
            return Encoding.UTF8.GetString(decodedBytes);
        }

        private static string GetEncodedPassword()
        {
            using (StreamReader reader = new StreamReader("C:\\Users\\evelin.totev\\OneDrive - EGT Digital Ltd\\Desktop\\password.txt"))
            {
                string line = reader.ReadLine();
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(line));
            }
        }

    }
}

