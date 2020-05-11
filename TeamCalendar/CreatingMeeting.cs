using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace TeamCalendar
{
    public static class CreatingMeeting
    {
        public static void DateCheckSNS(string h, string m)
        {
            string chyby = "";
            int[] hodiny = { 00, 01, 02, 03, 04, 05, 06, 07, 08, 09, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
            List<int> lhodiny = new List<int>();
            lhodiny.AddRange(hodiny);

            int[] minuty = { 00, 01, 02, 03, 04, 05, 06, 07, 08, 09, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59 };
            List<int> lminuty = new List<int>();
            lminuty.AddRange(minuty);

            if (!lminuty.Contains(Int32.Parse(h))) chyby += "Špatný formát data - hodiny\n";
            if (!lminuty.Contains(Int32.Parse(m))) chyby += "Špatné formát data - minuty\n";

            if (chyby != "") System.Windows.MessageBox.Show(chyby);
        }

        public static bool DateCheck(string od_h, string od_m, string do_h, string do_m)
        {
            string chyby = "";
            int spravne = 0;
            int[] hodiny = { 00, 01, 02, 03, 04, 05, 06, 07, 08, 09, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
            List<int> lhodiny = new List<int>();
            lhodiny.AddRange(hodiny);

            int[] minuty = { 00, 01, 02, 03, 04, 05, 06, 07, 08, 09, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59 };
            List<int> lminuty = new List<int>();
            lminuty.AddRange(minuty);
            try
            {
                if (lhodiny.Contains(Int32.Parse(od_h))) spravne++;
            }
            catch
            {
                chyby += "Špatný formát data - od (hodiny)\n";
            }
            try
            {
                if (lhodiny.Contains(Int32.Parse(do_h))) spravne++;
            }
            catch
            {
                chyby += "Špatné formát data - do (hodiny)\n";
            }
            try
            {
                if (lminuty.Contains(Int32.Parse(od_m))) spravne++;
            }
            catch
            {
                chyby += "Špatný formát data - od (minuty)\n";
            }
            try
            {
                if (lminuty.Contains(Int32.Parse(do_m))) spravne++;
            }
            catch
            {
                chyby += "Špatné formát data - do (minuty)\n";
            }





            if (chyby == "" && spravne == 4) { return true; }
            else
            {
                System.Windows.MessageBox.Show(chyby);
                return false;
            }


        }

        

        public static List<Relation<User>> loadInvitedUsers(string loadString)
        {
            List<Relation<User>> result = new List<Relation<User>>();
            foreach (string name in loadString.Split(' '))
            {
                if (name != "")
                {
                    result.Add(Relation<User>.Create(StorageManager.GetStorage().findUserByName(name)));
                }
            }
            return result;
        }

        
    }

}
