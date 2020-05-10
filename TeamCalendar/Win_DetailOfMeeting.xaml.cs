﻿using System;
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
            tbl_createdBy.Text = StorageManager.GetStorage().findById<User>(meet.CreatedBy.id).Name;

            foreach (User item in meet.AgreedByUser)
            {
                tb_agreed.Text += item.Name;
            }
            foreach (User item in meet.RejectedByUser)
            {
                tb_rejected.Text += item.Name;
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
            foreach (var item in meet.RejectedByUser)
            {
                if (item != StorageManager.loggedUser)
                {
                    meet.RejectedByUser.Add(StorageManager.loggedUser);
                }
                else
                {
                    meet.AgreedByUser.Remove(item);
                    meet.RejectedByUser.Add(StorageManager.loggedUser);
                }
            }
            string s = "";
            foreach (var item in meet.RejectedByUser)
            {
                s += " " + item;
            }
            tb_rejected.Text = s;
            this.Close();

        }

        private void b_agree_Click(object sender, RoutedEventArgs e)
        {
            //OPRAVIT
            foreach (var item in meet.AgreedByUser)
            {
                if (item != StorageManager.loggedUser)
                {
                    meet.AgreedByUser.Add(StorageManager.loggedUser);
                }
                else
                {
                    meet.RejectedByUser.Remove(item);
                    meet.AgreedByUser.Add(StorageManager.loggedUser);
                }
            }
            string s = "";
            foreach (var item in meet.AgreedByUser)
            {
                s += " " + item;
            }
            tb_agreed.Text = s;
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
            //Princip nahrazení


            //DateTime from = new DateTime(dp_od.SelectedDate.Value.Year, dp_od.SelectedDate.Value.Month, dp_od.SelectedDate.Value.Day, Int32.Parse(tb_od_h.Text), Int32.Parse(tb_od_m.Text), 0);
            //DateTime to = new DateTime(dp_do.SelectedDate.Value.Year, dp_do.SelectedDate.Value.Month, dp_do.SelectedDate.Value.Day, Int32.Parse(tb_do_h.Text), Int32.Parse(tb_do_m.Text), 0);

            //List<Relation<User>> agreedByUser = new List<Relation<User>>();
            //List<Relation<User>> rejectedByUser = new List<Relation<User>>();

            //StorageManager.GetStorage().meetings.Add(Meeting.Create(tb_nazevSch.Text, Meeting_win.loadInvitedUsers(tb_people.Text), tb_place.Text, color, from, to, StorageManager.loggedUser, agreedByUser, rejectedByUser));
            //StorageManager.Save();
            //this.Close();
            //System.Windows.MessageBox.Show("Meeting vytvořen");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StorageManager.GetStorage().meetings.Add(Meeting.Create(meet.Name, meet.InvitedUser, meet.Misto, meet.Color, meet.From, meet.To, StorageManager.GetStorage().findById<User>(meet.CreatedBy.id), meet.AgreedByUser, meet.RejectedByUser));
            //StorageManager.Save();
        }
    }
}