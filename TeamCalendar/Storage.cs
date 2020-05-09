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

        public List<Meeting> meetings;

        public Storage()
        {
            users = new List<User>();
            meetings = new List<Meeting>();
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

        public List<Meeting> findMeetingByInvitedByDate(DateTime date)
        {
            List<Meeting> result = new List<Meeting>();
            foreach(Meeting meeting in meetings)
            {
                if (meeting.isInvited() || meeting.isMain())
                {
                    if(
                        meeting.From.Year <= date.Year && meeting.To.Year >= date.Year
                        && meeting.From.Month <= date.Month && meeting.To.Month >= date.Month
                        && meeting.From.Day <= date.Day && meeting.To.Day >= date.Day
                      )
                    {
                        result.Add(meeting);
                    }
                }
            }
            return result;
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
                foreach (Meeting m in meetings)
                {
                    if (m.id == id)
                    {
                        if (m is T meeting)
                        {
                            return meeting;
                        }
                    }
                }
            }
            else
            {
                throw new Exception("Neznámý typ");
            }

            throw new Exception("Nepodařilo se najít daný objekt v uložišti.");
        }

        public List<Meeting> FindMeetingsByDate(DateTime date)
        {
            List<Meeting> vysledky = new List<Meeting>();
            foreach (Meeting meeting in meetings)
            {
                //Zjistit jestli to je v ten den
                if (true)
                {
                    vysledky.Add(meeting);
                }
            }
            return vysledky;
        }
    }
    


}
