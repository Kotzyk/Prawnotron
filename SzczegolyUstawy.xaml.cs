using System;
using System.Windows;


namespace Prawnotron
{
    /// <summary>
    /// Interaction logic for SzczegolyUstawy.xaml
    /// </summary>
    public partial class SzczegolyUstawy : Window
    {
        private Ustawa taUstawa;
        /// <summary>
        /// 
        /// </summary>
        public SzczegolyUstawy()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="u"></param>
        public SzczegolyUstawy(Ustawa u):this()
        {
            taUstawa = u;
            textBox_tytul.Text = taUstawa.Tytul;
            textBox_nr.Text = taUstawa.Nr.ToString();
            textBox_autor.Text = taUstawa.Autor_Nazwa;
            textBox_label.Text = taUstawa.Label;
            textBox_dataPub.Text = taUstawa.Data_Publikacji;
            textBox_dataWyd.Text = taUstawa.Data_Wydania;
            textBox_DataWej.Text = taUstawa.Data_Wejscia_W_Zycie;
            textBox_Syg.Text = taUstawa.Sygnatura;
        }

        private async void button_Zapisz_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Wyciąganie artykułów (jak, gdzie?) i zapisywanie do PDF.
            await ApiClient.GetContentAsync(taUstawa);
            Statue s = new Statue(taUstawa.Tresc_path);
            try
            {
                if(textBox_arty.Text != "")
                    s.Zapisz(textBox_arty.Text);
                else
                    throw new ApplicationException("Musisz wpisać przynajmniej jeden numer artykułu!");
            }
            catch (ApplicationException exception)
            {
                MessageBox.Show(exception.Message);
            }
            

        }
    }
}
