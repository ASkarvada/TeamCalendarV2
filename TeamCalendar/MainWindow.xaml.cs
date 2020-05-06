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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace TeamCalendar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string reg_heslo { get; set; }
        public string reg_hesloZnovu { get; set; }
        public string login_heslo { get; set; }
        
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void b_Zaregistrovat_Click(object sender, RoutedEventArgs e)
        {
            reg_heslo = tb_reg_heslo.Password;
            reg_hesloZnovu = tb_reg_hesloZnovu.Password;
            
            if (tb_reg_jmeno.Text != "" || tb_reg_jmeno.Text != " " || reg_heslo != "" || reg_heslo != " " || reg_hesloZnovu != "" || reg_hesloZnovu != " ")
            {
                if (reg_heslo == reg_hesloZnovu)
                {

                    StorageManager.GetStorage().users.Add(User.Create(tb_reg_jmeno.Text, Encrypce.Start(reg_heslo, true)));
                    MessageBox.Show("Byl jste registrován");

                    StorageManager.loggedUser = StorageManager.GetStorage().findUserByName(tb_reg_jmeno.Text);
                    Win_Calendar win_cal = new Win_Calendar();
                    win_cal.Show();
                    this.Close();

                }
                else
                {
                    MessageBox.Show("Hesla nejsou stejná!");
                }
            }
            else
            {
                MessageBox.Show("Zkontrolujte vaše údaje ještě jednou", "Chyba");
            }
        }

        private void b_Prihlasit_Click(object sender, RoutedEventArgs e)
        {
            login_heslo = tb_login_heslo.Password;

            User user = StorageManager.GetStorage().findUserByName(tb_login_jmeno.Text);
            if (user == null)
            {
                MessageBox.Show("Uživatel neexstuje");
            }
            else
            {
                if (login_heslo == Encrypce.Start(user.Password, false))
                {
                    MessageBox.Show("Uživatel úspešně přihlášen");
                    StorageManager.loggedUser = user;
                    Win_Calendar win_cal = new Win_Calendar();
                    win_cal.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Špatné heslo");

                }
            }

           
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StorageManager.Save();
        }
    }
}
