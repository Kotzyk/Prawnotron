using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using HtmlAgilityPack;

namespace Prawnotron
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_continue_Click(object sender, RoutedEventArgs e)
        {
            KryteriaWindow k = new KryteriaWindow();
            k.Show();
            this.Close();
        }
    }
}