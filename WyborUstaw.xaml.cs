using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;

namespace Prawnotron
{
    /// <summary>
    /// Interaction logic for WyborUstaw.xaml
    /// </summary>
    public partial class WyborUstaw
    {
        /// <summary>
        /// Słownik <see cref="Dictionary{TKey,TValue}"/> łączący nazwy pól tekstowych z nazwami parametrów Ustawy
        /// </summary>
        /// <seealso cref="Ustawa"/>
        public static Dictionary<string, string> Dic = new Dictionary<string, string>();

        /// <summary>
        /// Konstruktor okna
        /// </summary>
        public WyborUstaw()
        {
            InitializeComponent();
        }

        private void button_dodaj_Click(object sender, RoutedEventArgs e)
        {
            int liczbaZal;

            try
            {
                #region checkboxes

                if (checkBox_Rok.IsChecked == true)
                {
                    int rok;
                    if (int.TryParse(textBox_Rok.Text, out rok))
                    {
                        Dic.Add("rok", rok.ToString());
                      
                    }
                    else
                    {
                        MessageBox.Show("Wprowadzona wartosc nie jest numeryczna!", "Blad w roku");
                    }
                }
                if (checkBox_Pozycja.IsChecked == true)
                {
                    int pozycja;
                    if (int.TryParse(textBox_Pozycja.Text, out pozycja))
                    {
                        Dic["poz"] = pozycja.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Wprowadzona wartosc nie jest numeryczna!", "Blad w pozycji");
                    }
                }
                if (checkBox_Numer.IsChecked == true)
                {
                    int numer;
                    if (int.TryParse(textBox_Numer.Text, out numer))
                    {
                        Dic.Add("nr", numer.ToString());
                    }
                    else
                    {
                        MessageBox.Show("Wprowadzona wartosc nie jest numeryczna!", "Blad w numerze");
                    }
                }
                if (checkBox_Sygnatura.IsChecked == true)
                    if (textBox_Sygnatura.Text != "TextBox")
                    {
                        Dic.Add("sygnatura", textBox_Sygnatura.Text);
                    }
                    else
                    {
                        MessageBox.Show("Zaznaczono i nie wprowadzono wartosci!", "Blad w sygnaturze");
                    }
                DateTime data;
                if (checkBox_DataWyd.IsChecked == true)
                    if (DateTime.TryParseExact(textBox_DataWyd.Text, "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out data))
                    {
                        Dic.Add("data_wydania", data.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        MessageBox.Show("Nieprawidlowy format daty, sprobuj rok-miesiac-dzien!", "Blad w dacie wydania");
                    }
                if (checkBox_DataWej.IsChecked == true)
                    if (DateTime.TryParseExact(textBox_DataWej.Text, "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out data))
                    {
                        Dic.Add("data_wejscia_w_zycie", data.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        MessageBox.Show("Nieprawidlowy format daty, sprobuj rok-miesiac-dzien!", "Blad w dacie wejscia");
                    }
                if (checkBox_DataPub.IsChecked == true)
                    if (DateTime.TryParseExact(textBox_DataPub.Text, "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out data))
                    {
                        Dic.Add("data_publikacji", data.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        MessageBox.Show("Nieprawidlowy format daty, sprobuj rok-miesiac-dzien!", "Blad w dacie publikacji");
                    }
                if (checkBox_Label.IsChecked == true)
                    if (textBox_Label.Text != "TextBox")
                    {
                        Dic.Add("label", textBox_Label.Text);
                    }
                    else
                    {
                        MessageBox.Show("Zaznaczono i nie wprowadzono wartosci!", "Blad w labelu");
                    }
                if (checkBox_Tytul.IsChecked == true)
                    if (textBox_Tytul.Text != "TextBox")
                    {
                        Dic.Add("tytul", textBox_Tytul.Text);
                    }
                    else
                    {
                        MessageBox.Show("Zaznaczono i nie wprowadzono wartosci!", "Blad w tytule");
                    }
                if (checkBox_TytulSkr.IsChecked == true)
                    if (textBox_TytulSkr.Text != "TextBox")
                    {
                        Dic.Add("tytul_skrocony", textBox_TytulSkr.Text);
                    }
                    else
                    {
                        MessageBox.Show("Zaznaczono i nie wprowadzono wartosci!", "Blad w tytule skroconym");
                    }
                if (checkBox_Autor.IsChecked == true)
                    if (textBox_Autor.Text != "TextBox")
                    {
                        Dic.Add("autor_nazwa", textBox_Autor.Text);
                    }
                    else
                    {
                        MessageBox.Show("Zaznaczono i nie wprowadzono wartosci!", "Blad w autorze");
                    }
                if (checkBox_Zrodlo.IsChecked == true)
                    if (textBox_Zrodlo.Text != "TextBox")
                    {
                        Dic.Add("zrodlo", textBox_Zrodlo.Text);
                    }
                    else
                    {
                        MessageBox.Show("Zaznaczono i nie wprowadzono wartosci!", "Blad w zrodle");
                    }
                if (checkBox_Kodeks.IsChecked == true)
                    if (textBox_Kodeks.Text != "TextBox")
                    {
                        Dic.Add("kodeks", textBox_Kodeks.Text);
                    }
                    else
                    {
                        MessageBox.Show("Zaznaczono i nie wprowadzono wartosci!", "Blad w kodeksie");
                    }
                if (checkBox_LiczbaZal.IsChecked == true)
                    if (int.TryParse(textBox_LiczbaZal.Text, out liczbaZal))
                    {
                        Dic.Add("liczba_zalacznikow", liczbaZal.ToString());
                    }
                    else
                    {
                        MessageBox.Show("Wprowadzona wartosc nie jest wartoscia numeryczna!", "Blad w liczbie zal");
                    }
                if (checkBox_Typ.IsChecked == true)
                    if (textBox_Typ.Text != "TextBox")
                    {
                        Dic.Add("typ_nazwa", textBox_Typ.Text);
                    }
                    else
                    {
                        MessageBox.Show("Zaznaczono i nie wprowadzono wartosci!", "Blad w typie");
                    }
            #endregion
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Close();
            }
        }
    }
}
