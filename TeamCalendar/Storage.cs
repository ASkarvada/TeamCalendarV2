using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamCalendar
{
    public class Storage
    {
        public List<User> users;

        public Relation<Meeting> meetings;

        public Storage()
        {
            users = new List<User>();
        }

        public User findUserByName(string name)
        {
            foreach (User user in users)
            {
                if (user.Name == name)
                {
                    return user;
                }
            }
            return null;
        }

        public T findById<T>(Guid id) where T : Type
        {
            if (typeof(T) == typeof(User))
            {
                foreach (User u in users)
                {
                    if (u.id == id)
                    {
                        if (u is T user)
                        {
                            return user;
                        }
                    }
                }
            }
            else if (typeof(T) == typeof(Meeting))
            {

            }
            else
            {
                throw new Exception("Neznámý typ");
            }

            throw new Exception("Nepodařilo se najít daný objekt v uložišti.");
        }

    }
    


}
