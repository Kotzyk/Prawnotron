using System;
using System.Windows;


namespace Prawnotron
{
 
    public partial class SzczegolyUstawy
    {
        private readonly Ustawa _taUstawa;
        
        public SzczegolyUstawy()
        {
            InitializeComponent();
        }
        
        public SzczegolyUstawy(Ustawa u):this()
        {
            _taUstawa = u;
            textBox_tytul.Text = _taUstawa.Tytul;
            textBox_nr.Text = _taUstawa.Nr.ToString();
            textBox_autor.Text = _taUstawa.Autor_Nazwa;
            textBox_label.Text = _taUstawa.Label;
            textBox_dataPub.Text = _taUstawa.Data_Publikacji;
            textBox_dataWyd.Text = _taUstawa.Data_Wydania;
            textBox_DataWej.Text = _taUstawa.Data_Wejscia_W_Zycie;
            textBox_Syg.Text = _taUstawa.Sygnatura;
        }

        private async void button_Zapisz_Click(object sender, RoutedEventArgs e)
        {
            await ApiClient.GetContentAsync(_taUstawa);
            Statue s = new Statue(_taUstawa.Tresc_path);
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
