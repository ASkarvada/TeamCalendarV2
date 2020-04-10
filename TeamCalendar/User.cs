using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamCalendar
{
    public class User : Type
    {

        public string Name { get; set; }

        public string Password { get; set; }

        public static User create(string name, string password)
        {
            User user = new User();
            user.Name = name;
            user.Password = password;

            return user;
        }
    }
}
