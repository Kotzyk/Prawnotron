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
        List<Ustawa> _listaUst;
        readonly List<string> _listaTytulow= new List<string>();
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
                dic.Clear();
                dic.Add("rok", textBox_rok.Text);
                dic.Add("poz", textBox_poz.Text);
                dic.Add("nr", textBox_nr.Text);
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message);
            }
            finally
            {
                _listaUst = await ApiClient.SzukajAsync(dic);
                Debug.WriteLine(_listaUst.Count);
                foreach (Ustawa u in _listaUst)
                {
                    _listaTytulow.Add(u.Tytul);
                }
                listBox_listaUstaw.Items.Refresh();
                stopwatch.Stop();
                Debug.WriteLine(stopwatch.Elapsed);
            }
        }
    }
}