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

namespace Prawnotron
{
    /// <summary>
    /// Interaction logic for WyborUstaw.xaml
    /// </summary>
    public partial class WyborUstaw : Window
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();

        public WyborUstaw()
        {
            InitializeComponent();
        }

        private void button_dodaj_Click(object sender, RoutedEventArgs e)
        {
            dic.Clear();
            dic.Add("rok", textBox_Rok.Text);
            dic.Add("poz", textBox_Pozycja.Text);
            dic.Add("nr", textBox_Numer.Text);
        }
    }
}
