using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TeamCalendar
{
    public class Meeting : Type
    {
        public string Name { get; set; }

        public List<Relation<User>> InvitedUser { get; set; }

        public string Misto { get; set; }

        public Color Color { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public Relation<User> createdBy { get; set; }

        public static Meeting create(string name, List<Relation<User>> invitedUser, string misto, Color color, DateTime from, DateTime to, Relation<User> createdBy)
        {
            Meeting meeting = new Meeting();

            meeting.Name = name;
            meeting.InvitedUser = invitedUser;
            meeting.Misto = misto;
            meeting.Color = color;
            meeting.From = from;
            meeting.To = to;
            meeting.createdBy = createdBy;

            return meeting;
        }
    }
}
