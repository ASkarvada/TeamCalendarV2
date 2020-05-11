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
        
        Color color { get; set; }

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

            color = Color.FromArgb(100, 221, 221, 221);

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
            bool dateCheck = CreatingMeeting.DateCheck(tb_od_h.Text, tb_od_m.Text, tb_do_h.Text, tb_do_m.Text);

            if(dateCheck)
            {
                DateTime from = new DateTime(dp_od.SelectedDate.Value.Year, dp_od.SelectedDate.Value.Month, dp_od.SelectedDate.Value.Day, Int32.Parse(tb_od_h.Text), Int32.Parse(tb_od_m.Text), 0);
                DateTime to = new DateTime(dp_do.SelectedDate.Value.Year, dp_do.SelectedDate.Value.Month, dp_do.SelectedDate.Value.Day, Int32.Parse(tb_do_h.Text), Int32.Parse(tb_do_m.Text), 0);

                if (from < to && dateCheck)
                {
                    if (tb_nazevSch.Text == "" || tb_nazevSch.Text == " " || tb_place.Text == "" || tb_place.Text == " ")
                    {
                        System.Windows.MessageBox.Show("Některá pole jsou prázdná");
                    }
                    else
                    {
                        List<Relation<User>> agreedByUser = new List<Relation<User>>();
                        agreedByUser.Add(Relation<User>.Create(StorageManager.GetStorage().findUserByName(StorageManager.loggedUser.Name)));
                        List<Relation<User>> rejectedByUser = new List<Relation<User>>();

                        StorageManager.GetStorage().meetings.Add(Meeting.Create(tb_nazevSch.Text, CreatingMeeting.loadInvitedUsers(tb_people.Text), tb_place.Text, color, from, to, StorageManager.loggedUser, agreedByUser, rejectedByUser));

                        StorageManager.Save();
                        this.Close();
                        System.Windows.MessageBox.Show("Meeting vytvořen");
                    }
                    
                }
                else
                {
                    System.Windows.MessageBox.Show("Datumy jsou špatně!");
                }
            }
            
            
        }

        

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }
    }
}
