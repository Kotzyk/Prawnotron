using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Prawnotron
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Ustawa> _listaUst = new List<Ustawa>();
        readonly List<string> _listaTytulow = new List<string>();
        Stopwatch stopwatch = new Stopwatch();
        Dictionary<string, string> dic = new Dictionary<string, string>();

        public MainWindow()
        {
            InitializeComponent();
            listBox_listaUstaw.ItemsSource = _listaTytulow;
        }

        async void button_szukajUstawy_Click(object sender, RoutedEventArgs e)
        {
            stopwatch.Reset();
            stopwatch.Start();
            try
            {
                _listaUst = await ApiClient.SzukajAsync(dic);
                Debug.WriteLine(_listaUst.Count);
                foreach (Ustawa u in _listaUst)
                {
                    _listaTytulow.Add(u.Tytul);
                }
                listBox_listaUstaw.Items.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                stopwatch.Stop();
                dic.Clear();
                listView.Items.Refresh();
                Debug.WriteLine(stopwatch.Elapsed);
            }
        }

        private void button_dodaj_click(object sender, RoutedEventArgs e)
        {
            try
            {
                WyborUstaw wybor = new WyborUstaw();
                if (wybor.ShowDialog() != null)
                {
                    dic = wybor.dic;
                }
                listView.ItemsSource = dic;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}