using System;
using System.Collections.Generic;
using System.Globalization;
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
        public Dictionary<string, string> dic = new Dictionary<string, string>();

        public WyborUstaw()
        {
            InitializeComponent();
        }

        private void button_dodaj_Click(object sender, RoutedEventArgs e)
        {
            dic.Clear();
            int rok, pozycja, numer, liczbaZal;
            DateTime data;
            try
            {
                if (checkBox_Rok.IsChecked == true)
                    if (Int32.TryParse(textBox_Rok.Text, out rok))
                    {
                        dic.Add("rok", rok.ToString());
                    }
                    else
                    {
                        MessageBox.Show("Wprowadzona wartosc nie jest numeryczna!", "Blad w roku");
                    }
                if (checkBox_Pozycja.IsChecked == true)
                    if (Int32.TryParse(textBox_Pozycja.Text, out pozycja))
                    {
                        dic.Add("poz", pozycja.ToString());
                    }
                    else
                    {
                        MessageBox.Show("Wprowadzona wartosc nie jest numeryczna!", "Blad w pozycji");
                    }
                if (checkBox_Numer.IsChecked == true)
                    if (Int32.TryParse(textBox_Numer.Text, out numer))
                    {
                        dic.Add("nr", numer.ToString());
                    }
                    else
                    {
                        MessageBox.Show("Wprowadzona wartosc nie jest numeryczna!", "Blad w numerze");
                    }
                if (checkBox_Sygnatura.IsChecked == true)
                    if (textBox_Sygnatura.Text != "TextBox")
                    {
                        dic.Add("sygnatura", textBox_Sygnatura.Text);
                    }
                    else
                    {
                        MessageBox.Show("Zaznaczono i nie wprowadzono wartosci!", "Blad w sygnaturze");
                    }
                if (checkBox_DataWyd.IsChecked == true)
                    if (DateTime.TryParseExact(textBox_DataWyd.Text, "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out data))
                    {
                        dic.Add("data_wydania", data.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        MessageBox.Show("Nieprawidlowy format daty, sprobuj rok-miesiac-dzien!", "Blad w dacie wydania");
                    }
                if (checkBox_DataWej.IsChecked == true)
                    if (DateTime.TryParseExact(textBox_DataWej.Text, "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out data))
                    {
                        dic.Add("data_wejscia_w_zycie", data.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        MessageBox.Show("Nieprawidlowy format daty, sprobuj rok-miesiac-dzien!", "Blad w dacie wejscia");
                    }
                if (checkBox_DataPub.IsChecked == true)
                    if (DateTime.TryParseExact(textBox_DataPub.Text, "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out data))
                    {
                        dic.Add("data_publikacji", data.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        MessageBox.Show("Nieprawidlowy format daty, sprobuj rok-miesiac-dzien!", "Blad w dacie publikacji");
                    }
                if (checkBox_Label.IsChecked == true)
                    if (textBox_Label.Text != "TextBox")
                    {
                        dic.Add("label", textBox_Label.Text);
                    }
                    else
                    {
                        MessageBox.Show("Zaznaczono i nie wprowadzono wartosci!", "Blad w labelu");
                    }
                if (checkBox_Tytul.IsChecked == true)
                    if (textBox_Tytul.Text != "TextBox")
                    {
                        dic.Add("tytul", textBox_Tytul.Text);
                    }
                    else
                    {
                        MessageBox.Show("Zaznaczono i nie wprowadzono wartosci!", "Blad w tytule");
                    }
                if (checkBox_TytulSkr.IsChecked == true)
                    if (textBox_TytulSkr.Text != "TextBox")
                    {
                        dic.Add("tytul_skrocony", textBox_TytulSkr.Text);
                    }
                    else
                    {
                        MessageBox.Show("Zaznaczono i nie wprowadzono wartosci!", "Blad w tytule skroconym");
                    }
                if (checkBox_Autor.IsChecked == true)
                    if (textBox_Autor.Text != "TextBox")
                    {
                        dic.Add("autor_nazwa", textBox_Autor.Text);
                    }
                    else
                    {
                        MessageBox.Show("Zaznaczono i nie wprowadzono wartosci!", "Blad w autorze");
                    }
                if (checkBox_Zrodlo.IsChecked == true)
                    if (textBox_Zrodlo.Text != "TextBox")
                    {
                        dic.Add("zrodlo", textBox_Zrodlo.Text);
                    }
                    else
                    {
                        MessageBox.Show("Zaznaczono i nie wprowadzono wartosci!", "Blad w zrodle");
                    }
                if (checkBox_Kodeks.IsChecked == true)
                    if (textBox_Kodeks.Text != "TextBox")
                    {
                        dic.Add("kodeks", textBox_Kodeks.Text);
                    }
                    else
                    {
                        MessageBox.Show("Zaznaczono i nie wprowadzono wartosci!", "Blad w kodeksie");
                    }
                if (checkBox_LiczbaZal.IsChecked == true)
                    if (Int32.TryParse(textBox_LiczbaZal.Text, out liczbaZal))
                    {
                        dic.Add("liczba_zalacznikow", liczbaZal.ToString());
                    }
                    else
                    {
                        MessageBox.Show("Wprowadzona wartosc nie jest wartoscia numeryczna!", "Blad w liczbie zal");
                    }
                if (checkBox_Typ.IsChecked == true)
                    if (textBox_Typ.Text != "TextBox")
                    {
                        dic.Add("typ_nazwa", textBox_Typ.Text);
                    }
                    else
                    {
                        MessageBox.Show("Zaznaczono i nie wprowadzono wartosci!", "Blad w typie");
                    }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                this.Close();
            }
        }
    }
}
