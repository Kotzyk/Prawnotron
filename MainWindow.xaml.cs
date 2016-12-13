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
        List<Ustawa> listaUst;
        List<string> listaTytulow= new List<string>();
        Dictionary<string, string> dic = new Dictionary<string, string>();
        public MainWindow()
        {
            InitializeComponent();
            listBox_listaUstaw.ItemsSource = listaTytulow;
        }

        private async void button_szukajUstawy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                dic.Add("rok", textBox_rok.Text);
                //dic.Add("poz", textBox_poz.Text);
                //dic.Add("nr", textBox_nr.Text);
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message);
            }
            finally
            {
                listaUst = await ApiClient.SzukajAsync(dic);
                Debug.WriteLine(listaUst.Count);
                foreach (Ustawa u in listaUst)
                    listaTytulow.Add(u.TytulSkrocony);
                listBox_listaUstaw.Items.Refresh();
            }
        }
    }
}