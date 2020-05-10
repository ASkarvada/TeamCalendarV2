using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TeamCalendar
{
    /// <summary>
    /// Interakční logika pro Win_DetailOfMeeting.xaml
    /// </summary>
    public partial class Win_DetailOfMeeting : Window
    {
        Meeting meet;
        Color color;

        public Win_DetailOfMeeting(string id)
        {
            InitializeComponent();

            meet = StorageManager.GetStorage().findById<Meeting>(new Guid(id));

            tb_nazevSch.Text = meet.Name;
            tb_od_h.Text = Convert.ToString(meet.From.Hour);
            tb_od_m.Text = Convert.ToString(meet.From.Minute);
            tb_place.Text = meet.Misto;
            tb_do_h.Text = Convert.ToString(meet.To.Hour);
            tb_do_m.Text = Convert.ToString(meet.To.Minute);
            dp_od.SelectedDate = meet.From.Date;
            dp_do.SelectedDate = meet.To.Date;
            b_color.Background = new SolidColorBrush(Color.FromArgb(meet.Color.A, meet.Color.R, meet.Color.G, meet.Color.B));
            color = meet.Color;
            tbl_createdBy.Text = StorageManager.GetStorage().findById<User>(meet.CreatedBy.id).Name;
            
            foreach (User item in meet.AgreedByUser)
            {
                tb_agreed.Text += " " + item.Name;
            }
            foreach (User item in meet.RejectedByUser)
            {
                tb_rejected.Text += " " + item.Name;
            }

            List<User> users = StorageManager.GetStorage().users;
            foreach (User item in users)
            {
                lb_people.Items.Add(item.Name);
            }

            foreach (Relation<User> user in meet.InvitedUser)
            {
                tb_people.Text += " " + user.Get().Name;
                
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

        private void b_reject_Click(object sender, RoutedEventArgs e)
        {
            List<User> rejectedUserClone = meet.RejectedByUser.GetRange(0, meet.RejectedByUser.Count);

            for (int i = 0; i < rejectedUserClone.Count; i++)
            {
                User item = rejectedUserClone[i];

                if (item.id != StorageManager.loggedUser.id)
                {
                    if (!meet.RejectedByUser.Contains(item))
                    {
                        meet.RejectedByUser.Add(StorageManager.loggedUser);
                    }

                }
                //else
                //{
                //    meet.RejectedByUser.Remove(item);
                //    meet.AgreedByUser.Add(StorageManager.loggedUser);
                //}
            }


            StorageManager.GetStorage().UpdateMeeting(meet);
            StorageManager.Save();


            this.Close();



            //foreach (var item in meet.RejectedByUser)
            //{
            //    if (item != StorageManager.loggedUser)
            //    {
            //        meet.RejectedByUser.Add(StorageManager.loggedUser);
            //    }
            //    else
            //    {
            //        meet.AgreedByUser.Remove(item);
            //        meet.RejectedByUser.Add(StorageManager.loggedUser);
            //    }
            //}
            //string s = "";
            //foreach (var item in meet.RejectedByUser)
            //{
            //    s += " " + item;
            //}
            //tb_rejected.Text = s;
            //this.Close();

        }

        private void b_agree_Click(object sender, RoutedEventArgs e)
        {
            List<User> agreedUserClone = meet.AgreedByUser.GetRange(0, meet.AgreedByUser.Count);

            for (int i = 0; i < agreedUserClone.Count; i++)
            {
                User item = agreedUserClone[i];

                if (item.id != StorageManager.loggedUser.id)
                {
                    if(!meet.AgreedByUser.Contains(item))
                    {
                        meet.AgreedByUser.Add(StorageManager.loggedUser);
                    }
                    
                }
                //else
                //{
                //    meet.RejectedByUser.Remove(item);
                //    meet.AgreedByUser.Add(StorageManager.loggedUser);
                //}
            }


            StorageManager.GetStorage().UpdateMeeting(meet);
            StorageManager.Save();


            this.Close();
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

        private void b_upravit_Click(object sender, RoutedEventArgs e)
        {

            if(CreatingMeeting.DateCheck(tb_od_h.Text, tb_od_m.Text, tb_do_h.Text, tb_do_m.Text))
            {
                //ohlídat
                DateTime from = new DateTime(dp_od.SelectedDate.Value.Year, dp_od.SelectedDate.Value.Month, dp_od.SelectedDate.Value.Day, Int32.Parse(tb_od_h.Text), Int32.Parse(tb_od_m.Text), 0);
                DateTime to = new DateTime(dp_do.SelectedDate.Value.Year, dp_do.SelectedDate.Value.Month, dp_do.SelectedDate.Value.Day, Int32.Parse(tb_do_h.Text), Int32.Parse(tb_do_m.Text), 0);

                meet.Name = tb_nazevSch.Text;
                meet.InvitedUser = CreatingMeeting.loadInvitedUsers(tb_people.Text);
                meet.Misto = tb_place.Text;
                meet.Color = color;
                meet.From = from;
                meet.To = to;
                meet.CreatedBy = meet.CreatedBy;
                meet.AgreedByUser = meet.AgreedByUser;
                meet.RejectedByUser = meet.RejectedByUser;


                StorageManager.GetStorage().UpdateMeeting(meet);
                StorageManager.Save();
                this.Close();
                System.Windows.MessageBox.Show("Meeting upraven");
            }

            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
            
        }

        private void b_odstranit_Click(object sender, RoutedEventArgs e)
        {
            StorageManager.GetStorage().DeleteMeeting(meet);
            StorageManager.Save();
            this.Close();
            System.Windows.MessageBox.Show("Meeting odstraněn");
        }
    }
}
