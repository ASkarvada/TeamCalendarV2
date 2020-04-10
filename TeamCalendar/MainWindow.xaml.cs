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
    }
}
