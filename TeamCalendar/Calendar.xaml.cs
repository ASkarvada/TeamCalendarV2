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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TeamCalendar
{
    /// <summary>
    /// Interakční logika pro Win_Calendar.xaml
    /// </summary>
    public partial class Win_Calendar : Window
    {
        public int month;
        public int year;
        
        public Win_Calendar()
        {
            InitializeComponent();
            CalendarScreen(DateTime.Now);
            CreateMeetingControl();
        }

       public List<DateTime> FetchDays(int year, int month) //metoda pro generaci (zobrazeného) měsíce
        {
            
            List<DateTime> result = new List<DateTime>();
            for (int i = 1; i <= DateTime.DaysInMonth(year,month); i++)
            {
                result.Add(new DateTime(year, month, i));
            }

            return result;
        }

        public void RenderCallendar(int year, int month)
        {
            List<DateTime> days = FetchDays(year, month);
            int x = 0; //den 0 = pondělí
            int y = 0; //řada
            int firstButtonX = 0;
            int lastButtonX = 0;
            int lastButtonY = 0;
            foreach (DateTime day in days)
            {
                
                if(day.DayOfWeek == DayOfWeek.Monday)
                {
                    x = 0;
                }
                else if (day.DayOfWeek == DayOfWeek.Tuesday)
                {
                    x = 1;
                }
                else if (day.DayOfWeek == DayOfWeek.Wednesday)
                {
                    x = 2;
                }
                else if (day.DayOfWeek == DayOfWeek.Thursday)
                {
                    x = 3;
                }
                else if (day.DayOfWeek == DayOfWeek.Friday)
                {
                    x = 4;
                }
                else if (day.DayOfWeek == DayOfWeek.Saturday)
                {
                    x = 5;
                }
                else if (day.DayOfWeek == DayOfWeek.Sunday)
                {
                    x = 6;
                }

                //Render btn
                string name = ("day_" + x + "_" + y);
                var result = (Button)this.FindName(name);
                
                result.Content = day.Day;
                result.Tag = day.Date.ToBinary();

                if (day.Day == 1) firstButtonX = x;     //pro vypnutí buttonů (1. cyklus)
                if(days.Count - day.Day == 0)           //pro vypnutí buttonů (2. cyklus)
                {
                    lastButtonX = x;
                    lastButtonY = y;
                }

                if (x == 6) { x = 0; y++; }
                else x++;
                
            }
            
            //Vypnutí ostatních buttonů před polem
            for (int i = 0; i < firstButtonX; i++)
            {
                string name = ("day_" + i + "_0");
                var result = (Button)this.FindName(name);
                result.IsEnabled = false;
            }
            //Vypnutí ostatních buttonů za polem
            lastButtonX++; //aby se změnil až další (poposlední)
            for (int i = (days.Count + firstButtonX); i < 42; i++)
            {
                if (lastButtonX == 8) { lastButtonX = 0; lastButtonY = 5; }
                
                string name = ("day_" + (lastButtonX) + "_" + lastButtonY);
                if (name == "day_7_3") { lastButtonX = 0; lastButtonY = 4; name = ("day_" + (lastButtonX) + "_" + lastButtonY); }
                if (name == "day_7_4") { lastButtonX = 0; lastButtonY = 5; name = ("day_" + (lastButtonX) + "_" + lastButtonY); }

                var result = (Button)this.FindName(name);
                result.IsEnabled = false;
                if (lastButtonX == 6) { lastButtonX = 0; lastButtonY++; }
                else lastButtonX++;
            }

        }

        private void b_sipka_vpred_Click(object sender, RoutedEventArgs e)
        {
            month++;
            if (month > 12) { year++; month = 1; }
            DateTime date = new DateTime(year, month, 1);
            CalendarScreen(date);
        }

        private void b_sipka_zpet_Click(object sender, RoutedEventArgs e)
        {
            month--;
            if (month < 1) { year--; month = 12; }
            DateTime date = new DateTime(year, month, 1);
            CalendarScreen(date);
        }

        private void CalendarScreen(DateTime date)
        {
            tbl_uzivatel.Text = StorageManager.loggedUser.Name;

            foreach (UIElement element in grid1.Children) //povolení všech buttonů
            {
                if (element is Button)
                {
                    Button btn = (Button)element;
                    if (btn.Name != "b_sipka_vpred" && btn.Name != "b_sipka_zpet" && btn.Name != "b_SNS")
                    {
                        btn.IsEnabled = true;
                        btn.Content = "";
                    }
                }
            }

            tbl_rok.Text = date.ToString("yyyy");
            tbl_mesic.Text = date.ToString("MMMM");
            month = date.Month;
            year = date.Year;
            RenderCallendar(date.Year, date.Month);
        }

        private void day_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Meeting_win window = new Meeting_win(btn);
            window.Show();
            window.Closed += new System.EventHandler(this.isClosed);
            //CalendarScreen(new DateTime(year, month, Convert.ToInt32(btn.Content)));
            
        }

        public void CreateMeetingControl()
        {
            List<DateTime> days = FetchDays(year, month);
            foreach (DateTime day in days) //procházení všech dní v měsíci
            {
                //Relation<Meeting> meeting = new Relation<Meeting>();
                List<Meeting> mD = StorageManager.GetStorage().findMeetingByInvitedByDate(day);

                foreach (Meeting meetingInMonth in mD) //procházení všech meetingů v měsíci
                {
                    
                    if(meetingInMonth.From.Day == 2)
                    {
                        System.Windows.Controls.Button newBtn = new Button();

                        newBtn.Content = meetingInMonth.Name;
                        newBtn.Name = "Button" + meetingInMonth.Name;
                        newBtn.Height = 20;
                        newBtn.Width = 70;

                        sp1.Children.Add(newBtn);
                    }
                    



                    //int x = 0;
                    //int y = 0;
                    //for (int i = 0; i < 42; i++)
                    //{
                    //    string name = "day_" + x + "_" + y;
                    //    var result = (Button)this.FindName(name);
                    //    if (result.Content == btn.Tag)
                    //    {
                    //        btn.Height = 20;
                    //        btn.Margin = new Thickness(10, 30, 10, 10);
                    //        Grid.SetColumn(btn, x);
                    //    }
                    //    x++;
                    //    if (x == 7) { x = 0; y++; }
                    //}
                }
            }
        }

        public void isClosed(object sender, EventArgs e)
        {
            CreateMeetingControl();
        }

    }

}
