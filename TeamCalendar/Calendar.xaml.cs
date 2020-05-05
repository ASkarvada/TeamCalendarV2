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
        public int changeOfMonth = 0;
        public int changeOfYear = 0;
        public Win_Calendar()
        {
            InitializeComponent();
            CalendarScreen(DateTime.Now);

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
                //var result = this.GetType().GetField("day_"+x+"_"+y).GetValue(this);
                string name = ("day_" + x + "_" + y);
                var result = (Button)this.FindName(name);
                
                result.Content = day.Day;
                result.Tag = day.Date;

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

        public void onBtnClick(Button btn)
        {
            DateTime day = DateTime.FromBinary(Convert.ToInt64(btn.Tag));
            //otevrit okno 
        }

        private void b_sipka_vpred_Click(object sender, RoutedEventArgs e)
        {
            DateTime date;
            changeOfMonth++;
            if (DateTime.Now.Month + changeOfMonth > 12)
            { changeOfYear++; date = new DateTime(DateTime.Now.Year + changeOfYear, 1, DateTime.Now.Day); }
            date = new DateTime(DateTime.Now.Year + changeOfYear, DateTime.Now.Month + changeOfMonth, DateTime.Now.Day);
            CalendarScreen(date);
        }

        private void b_sipka_zpet_Click(object sender, RoutedEventArgs e)
        {
            DateTime date;
            changeOfMonth--;
            if (DateTime.Now.Month + changeOfMonth > 12)
            { changeOfYear--; date = new DateTime(DateTime.Now.Year + changeOfYear, 1, DateTime.Now.Day); }
            date = new DateTime(DateTime.Now.Year + changeOfYear, DateTime.Now.Month + changeOfMonth, DateTime.Now.Day);
            CalendarScreen(date);
        }

        private void CalendarScreen(DateTime date)
        {
            //tbl_uzivatel.Text = StorageManager.loggedUser.Name;

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
            RenderCallendar(date.Year, date.Month);
        }
    }

}
