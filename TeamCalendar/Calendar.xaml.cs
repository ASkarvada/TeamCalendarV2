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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TeamCalendar
{
    /// <summary>
    /// Interakční logika pro Win_Calendar.xaml
    /// </summary>
    public partial class Win_Calendar : Window
    {
        public Win_Calendar()
        {
            InitializeComponent();
            tbl_uzivatel.Text = StorageManager.loggedUser.Name;
            
        }
    }
}
