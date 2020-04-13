using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TeamCalendar
{
    public static class Encrypce
    {
        private static string password = "5d56as1d56adad5165as1251d5a60";

        public static string Start(string heslo, bool encryption)
        {

            if (encryption == true)
            {
                return StringCipher.Encrypt(heslo, password);
            }
            else if (encryption == false)
            {
                return StringCipher.Decrypt(heslo, password);
            }
            return "";

        }
    }
}
