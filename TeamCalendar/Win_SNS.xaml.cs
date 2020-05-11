using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TeamCalendar
{
    /// <summary>
    /// Interakční logika pro Win_SNS.xaml
    /// </summary>
    public partial class Win_SNS : Window
    {
        Color color { get; set; }
        public Win_SNS()
        {
            InitializeComponent();

            List<User> users = StorageManager.GetStorage().users;
            foreach (User item in users)
            {
                lb_people.Items.Add(item.Name);
            }

            color = Color.FromArgb(100, 221, 221, 221);
        }

        private void b_createMeeting_Click(object sender, RoutedEventArgs e)
        {
            CreatingMeeting.DateCheckSNS(tb_h.Text, tb_m.Text);

            DateTime from = FromTo();

            List<Relation<User>> agreedByUser = new List<Relation<User>>();
            agreedByUser.Add(Relation<User>.Create(StorageManager.GetStorage().findUserByName(StorageManager.loggedUser.Name)));
            List<Relation<User>> rejectedByUser = new List<Relation<User>>();

            StorageManager.GetStorage().meetings.Add(Meeting.Create(tb_nazevSch.Text, CreatingMeeting.loadInvitedUsers(tb_people.Text), tb_place.Text, color, from, from + new TimeSpan(Int32.Parse(tb_h.Text), Int32.Parse(tb_m.Text), 0), StorageManager.loggedUser, agreedByUser, rejectedByUser));

            StorageManager.Save();
            this.Close();
            System.Windows.MessageBox.Show("Meeting vytvořen");
        }

        private DateTime FromTo()
        {
            List<DateTime> cannotFrom = new List<DateTime>();
            List<DateTime> cannotTo = new List<DateTime>();
            List<Meeting> meetings = StorageManager.GetStorage().meetings;
            List<User> users = StorageManager.GetStorage().users;
            foreach (Meeting meeting in meetings)
            {
                foreach (User user in users)
                {
                    if(meeting.CreatedBy.id == user.id || meeting.isInvitedSNS(user.id))
                    {
                        cannotFrom.Add(meeting.From);
                        cannotTo.Add(meeting.To);
                    }
                }
                
            }

            cannotFrom = cannotFrom.Distinct().ToList();
            cannotFrom = cannotFrom.OrderBy(x => x.TimeOfDay).ToList();
            cannotTo = cannotTo.Distinct().ToList();
            cannotTo = cannotTo.OrderBy(x => x.TimeOfDay).ToList();

            DateTime dtNow = DateTime.Now;
            if (dtNow.Hour > 20) { dtNow = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day + 1, 8, 0, 0); }
            if (dtNow.Hour < 8) { dtNow = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, 8, 0, 0); }
            TimeSpan ts = new TimeSpan(Int32.Parse(tb_h.Text), Int32.Parse(tb_m.Text), 0);

            if (dtNow.Hour <= 8 && dtNow.Hour >= 20) dtNow = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day + 1, 8, 0, 0);

            List<DateTime> dtList = Win_Calendar.FetchDays(dtNow.Year, dtNow.Month);

            foreach (var item in dtList) //procházení dnů v měsíci
            {
                List<Meeting> listOfMeetingsInDay = StorageManager.GetStorage().FindMeetingsByDate(item);
                listOfMeetingsInDay = listOfMeetingsInDay.OrderBy(o => o.From).ToList();

                DateTime sum = dtNow + ts;

                for (int i = 0; i < listOfMeetingsInDay.Count; i++)
                {
                    if (dtNow <= listOfMeetingsInDay[i].From)
                    {

                        if (listOfMeetingsInDay[i].From.Hour > sum.Hour) //pokud nejdřívější meeting začne později než by skončil náš spontální
                        {
                            return dtNow;
                        }
                        else
                        {

                            if (cannotFrom.Contains(listOfMeetingsInDay[i].From))
                            {
                                DateTime cannot1 = listOfMeetingsInDay[i].To;
                                DateTime cannot2 = listOfMeetingsInDay[i + 1].From;
                                TimeSpan betweenMeetings = cannot2 - cannot1;


                                if (betweenMeetings >= ts)
                                {
                                    dtNow = cannot1;
                                    return dtNow;
                                }

                            }

                        }
                    }
                }
                
            }

            return new DateTime(2020, 5, 20, 20, 20, 20);
        }

        private void lb_people_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string selectedPerson = lb_people.SelectedItem.ToString();
            if (StorageManager.loggedUser.Name != selectedPerson)
            {
                List<string> list = tb_people.Text.Split(' ').ToList();
                list.Add(selectedPerson);
                tb_people.Text = String.Join(" ", list.Distinct().ToList().ToArray());
            }
        }

        private void b_color_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                b_color.Background = new SolidColorBrush(Color.FromArgb(cd.Color.A, cd.Color.R, cd.Color.G, cd.Color.B));
                color = Color.FromArgb(cd.Color.A, cd.Color.R, cd.Color.G, cd.Color.B);
            }
        }
    }
}
