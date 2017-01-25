using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
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
        static int rokPocz = 1918; //rok od ktorego zaczynaja pojawiac sie ustawy
        static int liczbaLat = (DateTime.Now.Year - 1918)+1; //aby w comboboxie bylo do roku w ktorym odpalamy program
        static int[] lata = Enumerable.Range(rokPocz, liczbaLat).ToArray(); //tablica z latami
        static string[] typy = { "Obwieszczenie", "Rozporządzenie", "Ustawa", "Oświadczenie", "Poprawki", "Uchwała", "Dekret", "Zarządzenie" }; //tablica z typami ustaw
        /// <summary>
        /// Konstruktor okna
        /// </summary>
        public WyborUstaw()
        {
            InitializeComponent();
            foreach (var year in lata)
            {
                comboBox_Rok.Items.Add(year);
            }
            foreach (string typ in typy)
            {
                comboBox_Typ.Items.Add(typ);
            }
        }

        private void button_dodaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                #region checkboxes
                if (checkBox_Rok.IsChecked == true)
                {
                    int rok;
                    if (int.TryParse(comboBox_Rok.Text, out rok))
                    {
                        Dic["rok"] = rok.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Wprowadzona wartosc nie jest numeryczna!", "Blad w roku");
                    }
                }
                if (checkBox_Pozycja.IsChecked == true)
                {
                    int pozycja;
                    if (!int.TryParse(textBox_Pozycja.Text, out pozycja) || string.IsNullOrWhiteSpace(textBox_Pozycja.Text))
                    {
                        MessageBox.Show("Wprowadzona wartosc nie jest numeryczna!", "Blad w pozycji");
                    }
                    else if (Int32.Parse(textBox_Pozycja.Text) <= 0 || Int32.Parse(textBox_Pozycja.Text) > 5010)
                    {
                        MessageBox.Show("Wpowadz wartosc pomiedzy 1 a 5010");
                    }
                    else
                    {
                        Dic["poz"] = pozycja.ToString();
                    }
                }
                if (checkBox_Numer.IsChecked == true)
                {
                    int numer;
                    if (!int.TryParse(textBox_Numer.Text, out numer) || string.IsNullOrWhiteSpace(textBox_Numer.Text))
                    {
                        MessageBox.Show("Wprowadzona wartosc nie jest numeryczna!", "Blad w numerze");
                    }
                    else if (Int32.Parse(textBox_Numer.Text) < 0 || Int32.Parse(textBox_Numer.Text) > 299)
                    {
                        MessageBox.Show("Wpowadz wartosc pomiedzy 0 a 299");
                    }
                    else
                    {
                        Dic["nr"] = numer.ToString();
                    }
                }
                DateTime data;
                if (checkBox_DataWyd.IsChecked == true)
                    if (DateTime.TryParseExact(Data_Wyd_Picker.Text, "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out data))
                    {
                        Dic["data_wydania"] = data.ToString("yyyy-MM-dd");
                    }
                if (checkBox_DataWej.IsChecked == true)
                    if (DateTime.TryParseExact(Data_Wej_Picker.Text, "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out data))
                    {
                        Dic["data_wejscia_w_zycie"] = data.ToString("yyyy-MM-dd");
                    }
                if (checkBox_DataPub.IsChecked == true)
                    if (DateTime.TryParseExact(Data_Pub_Picker.Text, "yyyy-MM-dd", CultureInfo.CurrentCulture, DateTimeStyles.None, out data))
                    {
                        Dic["data_publikacji"] = data.ToString("yyyy-MM-dd");
                    }
                if (checkBox_Autor.IsChecked == true)
                {
                    Regex autor = new Regex(@"[A-Z]{1}\w+");
                    if (autor.IsMatch(textBox_Autor.Text))
                    {
                        Dic["autor_nazwa"] = textBox_Autor.Text.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Nazwa autora jest niepoprawna!", "Blad w autorze");
                    }
                }
                if (checkBox_Typ.IsChecked == true)
                {
                    if (!string.IsNullOrWhiteSpace(comboBox_Typ.Text))
                    {
                        Dic["typ_nazwa"] = comboBox_Typ.Text.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Nie wprowadzono typu!", "Blad w typie");
                    }
                }
            #endregion
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            if(Dic.Count != 0)
            {
                this.Close();
            }
            else
            {
                if(MessageBox.Show("Chcesz poprawic kryteria?", "Blad kryteriow", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                    this.Close();
            }
        }
    }
}
