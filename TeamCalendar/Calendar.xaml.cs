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
            DeleteMeetingControl();
            CreateMeetingControl();
        }

        private void DeleteMeetingControl()
        {
            int x = 0;
            int y = 0;
            for (int i = 0; i < 42; i++) //procházení po dnech
            {
                string nameSPanel = ("sp_" + x + "_" + y);
                var resultOfSPanel = (StackPanel)this.FindName(nameSPanel);
                resultOfSPanel.Children.Clear();
                
                x++;
                if (x == 7) { x = 0; y++; }
            }
        }

        private void b_sipka_zpet_Click(object sender, RoutedEventArgs e)
        {
            month--;
            if (month < 1) { year--; month = 12; }
            DateTime date = new DateTime(year, month, 1);
            CalendarScreen(date);
            DeleteMeetingControl();
            CreateMeetingControl();
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
            window.b_createMeeting.Click += this.isClosed;
            
            
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
                    int x = 0;
                    int y = 0;
                    for (int i = 0; i < 42; i++) //procházení po dnech
                    {
                        string nameSPanel = ("sp_" + x + "_" + y);
                        string nameButton = ("day_" + x + "_" + y);
                        var resultOfSPanel = (StackPanel)this.FindName(nameSPanel);
                        var resultOfButton = (Button)this.FindName(nameButton);

                        if(resultOfButton.IsEnabled)
                        {
                            if (meetingInMonth.From.Day == Convert.ToInt32(resultOfButton.Content)) //ověření zda je schůzka ve dnu
                            {
                                bool firstElement = true;

                                Button newBtn = new Button();

                                newBtn.Content = meetingInMonth.Name;
                                newBtn.Name = "Button" + meetingInMonth.Name;
                                newBtn.Tag = meetingInMonth.id;
                                newBtn.Click += meeting_Click;
                                newBtn.Height = 20;
                                newBtn.Width = 70;
                                newBtn.Background = new SolidColorBrush(Color.FromArgb(meetingInMonth.Color.A,meetingInMonth.Color.R, meetingInMonth.Color.G, meetingInMonth.Color.B));

                                UIElementCollection element = resultOfSPanel.Children;
                                List<FrameworkElement> lstElement = element.Cast<FrameworkElement>().ToList();
                                var lstControl = lstElement.OfType<Control>();

                                foreach (Control contol in lstControl)
                                {
                                    if (Convert.ToString(contol.Tag) == Convert.ToString(meetingInMonth.id))
                                    {
                                        firstElement = false;
                                    }
                                }

                                if(firstElement)
                                {
                                    resultOfSPanel.Children.Add(newBtn);
                                }
                            }
                        }

                        x++;
                        if(x == 7) { x = 0; y++; }
                    }
                }
            }
        }

        public void isClosed(object sender, RoutedEventArgs e)
        {
            CreateMeetingControl();
        }

        public void meeting_Click(object sender, RoutedEventArgs e)
        {
            
        }

    }

}
