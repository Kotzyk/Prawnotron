﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Linq;

namespace Prawnotron
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class KryteriaWindow
    {
        public List<Ustawa> _listaUst = new List<Ustawa>();
        readonly List<string> _listaTytulow = new List<string>();
        Stopwatch _stopwatch = new Stopwatch();
        Dictionary<string, string> _dic = new Dictionary<string, string>();

        /// <summary>
        /// Konstruktor okna głównego, ustawia ItemSource okienka listy ustaw
        /// </summary>
        public KryteriaWindow()
        {
            InitializeComponent();
            listBox_listaUstaw.ItemsSource = _listaTytulow;
            button_szukajUstawy.IsEnabled = false;
        }

        async void button_szukajUstawy_Click(object sender, RoutedEventArgs e)
        {
            _stopwatch.Reset();
            _stopwatch.Start();
            try
            {
                _listaUst = await ApiClient.SzukajAsync(_dic);
                Debug.WriteLine(_listaUst.Count);
                foreach (Ustawa u in _listaUst)
                {
                    _listaTytulow.Add(u.Tytul);
                }
                listBox_listaUstaw.Items.Refresh();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Nie znaleziono ustawy o zadanych kryteriach", "Nie znaleziono ustawy", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                _stopwatch.Stop();
                ConditionsListView.Items.Refresh();
                button_szczegoly.IsEnabled = true;
                Debug.WriteLine(_stopwatch.Elapsed);
            }
        }

        private void button_dodaj_click(object sender, RoutedEventArgs e)
        {
            try
            {
                WyborUstaw wybor = new WyborUstaw();
                if (wybor.ShowDialog() != null)
                {
                    _dic = WyborUstaw.Dic;
                }
                ConditionsListView.ItemsSource = _dic;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if(_dic.Count != 0)
                {
                    button_szukajUstawy.IsEnabled = true;
                }
                ConditionsListView.Items.Refresh();
            }
        }

        private void button_usun_Click(object sender, RoutedEventArgs e)
        {
            _dic.Clear();
            _listaTytulow.Clear();
            button_szukajUstawy.IsEnabled = false;
            button_szczegoly.IsEnabled = false;
            listBox_listaUstaw.Items.Refresh();
            ConditionsListView.Items.Refresh();
        }

        private void button_szczegoly_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Ustawa ustawa = _listaUst.ElementAt(listBox_listaUstaw.SelectedIndex);
                SzczegolyUstawy szcz = new SzczegolyUstawy(ustawa);
                if (szcz.ShowDialog() != null)
                {
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
     }
 }