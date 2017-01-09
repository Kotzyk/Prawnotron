using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace Prawnotron
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Ustawa> _listaUst;
        List<string> _listaTytulow= new List<string>();
        Dictionary<string, string> _dic = new Dictionary<string, string>();
        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            listBox_listaUstaw.ItemsSource = _listaTytulow;

            //Patrzcie, dziewczyny, tak się to testuje
            /*Statue ustawa = new Statue("strona_3.html");
            string p;
            string ustawaString = "";
            foreach (var page in ustawa.Pages)
            {

                p = Convert.ToString(page);
                ustawaString += p;

            }

            ustawa.Wyszukaj(ustawaString, "44");
            */

        }

        private async void button_szukajUstawy_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                _dic.Add("rok", textBox_rok.Text);
                //dic.Add("poz", textBox_poz.Text);
                //dic.Add("nr", textBox_nr.Text);
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message);
            }
            finally
            {
                _listaUst = await ApiClient.SzukajAsync(_dic);
                Debug.WriteLine(_listaUst.Count);
                foreach (Ustawa u in _listaUst)
                {
                    _listaTytulow.Add(u.Tytul);
                }
                listBox_listaUstaw.Items.Refresh();
            }
        }
    }
}