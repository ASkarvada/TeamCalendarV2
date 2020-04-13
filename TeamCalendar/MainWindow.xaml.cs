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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void b_Zaregistrovat_Click(object sender, RoutedEventArgs e)
        {
            if (tb_reg_jmeno.Text != "" || tb_reg_jmeno.Text != " " || tb_reg_heslo.Text != "" || tb_reg_heslo.Text != " " || tb_reg_hesloZnovu.Text != "" || tb_reg_hesloZnovu.Text != " ")
            {
                if (tb_reg_heslo.Text == tb_reg_hesloZnovu.Text)
                {
                    StorageManager.GetStorage().users.Add(User.Create(tb_reg_jmeno.Text, tb_reg_heslo.Text));
                    MessageBox.Show("Byl jste registrován");
                   
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
            User user = StorageManager.GetStorage().findUserByName(tb_login_jmeno.Text);
            if (user == null)
            {
                MessageBox.Show("Uživatel neexstuje");
            }
            else
            {
                if (tb_login_heslo.Text == user.Password)
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
