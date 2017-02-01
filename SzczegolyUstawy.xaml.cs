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
    /// Interaction logic for SzczegolyUstawy.xaml
    /// </summary>
    public partial class SzczegolyUstawy : Window
    {
        public SzczegolyUstawy()
        {
            InitializeComponent();
        }
        public SzczegolyUstawy(Ustawa u):this()
        {
            textBox_tytul.Text = u.Tytul;
            textBox_nr.Text = u.Nr.ToString();
            textBox_autor.Text = u.Autor_Nazwa;
            textBox_label.Text = u.Label;
            textBox_dataPub.Text = u.Data_Publikacji;
            textBox_dataWyd.Text = u.Data_Wydania;
            textBox_DataWej.Text = u.Data_Wejscia_W_Zycie;
            textBox_Syg.Text = u.Sygnatura;
        }

        private void button_Zapisz_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Wyciąganie artykułów (jak, gdzie?) i zapisywanie do PDF.
        }
    }
}
