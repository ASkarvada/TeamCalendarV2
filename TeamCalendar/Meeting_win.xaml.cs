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
    /// Interakční logika pro Meeting_win.xaml
    /// </summary>
    public partial class Meeting_win : Window
    {
        List<Relation<User>> selectedUsers = new List<Relation<User>>();
        Color color;

        public Meeting_win(System.Windows.Controls.Button btn)
        {
            InitializeComponent();

            DateTime date = DateTime.FromBinary(Convert.ToInt64(btn.Tag));
            dp_od.SelectedDate = date;
            dp_do.SelectedDate = date;

            List<User> users = StorageManager.GetStorage().users;
            foreach (User item in users)
            {
                lb_people.Items.Add(item.Name);
            }
            
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
            if(cd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                b_color.Background = new SolidColorBrush(Color.FromArgb(cd.Color.A, cd.Color.R, cd.Color.G, cd.Color.B));
                color = Color.FromArgb(cd.Color.A, cd.Color.R, cd.Color.G, cd.Color.B);
            }
        }

        private void b_createMeeting_Click(object sender, RoutedEventArgs e)
        {
            DateTime from = new DateTime(dp_od.SelectedDate.Value.Year, dp_od.SelectedDate.Value.Month, dp_od.SelectedDate.Value.Day, Convert.ToInt32(tb_od_h.Text), Convert.ToInt32(tb_od_m.Text), 0);
            DateTime to = new DateTime(dp_do.SelectedDate.Value.Year, dp_do.SelectedDate.Value.Month, dp_do.SelectedDate.Value.Day, Convert.ToInt32(tb_do_h.Text), Convert.ToInt32(tb_do_m.Text), 0);

            //Relation

            StorageManager.GetStorage().meetings.Add(Meeting.Create(tb_nazevSch.Text, selectedUsers, tb_place.Text, color, from, to, StorageManager.loggedUser));
            StorageManager.Save();
            this.Close();
            System.Windows.MessageBox.Show("Meeting vytvořen");
            //Win_Calendar win = new Win_Calendar();
            //win.CreateMeetingControl();

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            
        }
    }
}
