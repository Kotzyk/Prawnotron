using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prawnotron.Properties;
using System.Text.RegularExpressions;
using System.Windows;

namespace Prawnotron
{
    /// <summary>
    /// Łączność z API poprzez <see cref="HttpClient" /> i pobieranie szczegółów oraz treści ustaw. <seealso cref="Ustawa" /><seealso cref="Ustawa.ParseUstawa" />
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    public static class ApiClient
    {
        static readonly HttpClient Client = new HttpClient();

        /// <summary>
        /// Konstruktor przypisujący podstawowe parametry klienta HTTP
        /// </summary>
        static ApiClient()
        {
            Client.BaseAddress = new Uri(Resources.Base_API1);
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        /// <summary>
        /// Zapisuje pliki .json z API dotyczące konkretnej ustawy
        /// </summary>
        /// <param name="id">ID ustawy <see cref="Ustawa.Id"/></param>
        /// <returns></returns>
        static async Task GetSavejson(int id)
        {
            using (Client)
            {
                string ustawa;
                try
                {
                    HttpResponseMessage response = await Client.GetAsync(Resources.Base_API1 + id);
                    if (response.IsSuccessStatusCode)
                    {
                        ustawa = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        ustawa = "cos sie popsulo";
                    }
                }
                catch (HttpRequestException e)
                {
                    Client.Dispose();
                    ustawa = e.Message;
                }
                JObject obj = JObject.Parse(ustawa);

                using (StreamWriter sw = File.CreateText("../../Json/ustawa_" + id + ".json"))
                {
                    JsonSerializer j = new JsonSerializer {Formatting = Formatting.Indented};
                    j.Serialize(sw, obj);
                }
            }
        }

        /// <summary>
        /// Wyszukuje ustawy w API korzystając z metody ?conditions[].
        /// Pobiera słownik (nazwa pola w GUI, treść pola w GUI),
        /// a zwraca listę <see cref="Ustawa" /> do wyświetlenia w interfejsie.
        /// </summary>
        /// <param name="conds">Lista <see cref="Dictionary{TKey,TValue}" />:
        /// Key = Nazwa parametru, np.<see cref="Ustawa.Rok" />;
        /// Value = Zawartość <see cref="TextBox" /></param>
        /// <returns>
        /// Lista <see cref="Ustawa" /> do wyświetlenia w GUI
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Jeśli lista jest null</exception>
        /// <exception cref="System.ArgumentException">Jeśli lista argumentów jest pusta</exception>
        /// <seealso cref="GetSavejson" />
        /// <seealso cref="Ustawa.RemoveDatasetName" />
        public static async Task<List<Ustawa>> SzukajAsync(Dictionary<string, string> conds)
        {
            //check if null
            if (conds == null)
            {
                throw new ArgumentNullException(nameof(conds)); 
            }
            //check if empty
            if (conds.Count == 0)
                throw new ArgumentException(Resources.cannot_be_an_empty_collection, nameof(conds));

            List<Ustawa> wynikiList = new List<Ustawa>();

            string apiStr = Resources.DzU_Search;
            StringBuilder sb = new StringBuilder(apiStr);

            foreach (KeyValuePair<string, string> warunek in conds)
            {
                if (warunek.Value != "" && warunek.Value != " ")
                    sb.Append($"&conditions[dziennik_ustaw.{warunek.Key}]={warunek.Value}");
            }

            try
            {
                HttpResponseMessage responseMessage = await Client.GetAsync(sb.ToString());
                if (responseMessage.IsSuccessStatusCode)
                {
                    string ustawa = await responseMessage.Content.ReadAsStringAsync();
                    Regex rgx = new Regex(@"\{.*\[");
                    ustawa = rgx.Replace(ustawa, "");
                    Regex rgx2 = new Regex(@"\]\,.\w{5}.*\}\}");
                    ustawa = rgx2.Replace(ustawa, "");
                    string[] podustawy = Regex.Split(ustawa, @"\}\}\,");

                    for (int i = 0; i < podustawy.Length; i++)
                    {
                        string temp = podustawy[i];
                        if (i < (podustawy.Length - 1))
                            temp = temp + "}}";
                        podustawy[i] = temp;
                    } //podustawy wczytuje poprawnie
                    Parallel.For(0, podustawy.Length, i =>
                    {
                        using (TextWriter sw = new StreamWriter($"../../Json/Ustawa_{i + 1}.json"))
                        {
                            sw.Write(podustawy[i]);
                        }
                        Ustawa u = Ustawa.ParseUstawa(i + 1);
                        wynikiList.Add(u);
                    }); //Todo: exception handling
                }
                else
                {
                    wynikiList = new List<Ustawa>();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return wynikiList;
        }

        //TODO: poprawne szukanie po ?q=


        /// <summary>
        /// Asynchronicznie zapisuje treść wybranej ustawy z API do pliku <c>HTML</c> do dalszej obróbki.
        /// <seealso cref="HttpClient"/>
        /// <seealso cref="StringBuilder"/>
        /// <seealso cref="HttpResponseMessage"/>
        /// <seealso cref="FileStream"/>
        /// </summary>
        /// <param name="ustawa"><see cref="Ustawa"/>, której treść pobieramy</param>
        /// <returns>Treść ustawy zapisana w pliku <c>HTML</c></returns>
        public static async Task GetContentAsync(Ustawa ustawa)
        {
            byte counter = 1;
            HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.Accepted);
            var s = Resources.Base_API2 + $"{ustawa.Dokument_Id}/{ustawa.Dokument_Id}_{counter}.html";
            Uri adres = new Uri(s);
            var tresc = "";
            try
            {
                //responseMessage = await Client.GetAsync(adres);
                Debug.WriteLine("tu jestem XD");
            }
            catch (HttpRequestException e)
            {
                MessageBox.Show(e.Message);
                
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            
            #region DownloadRegion
            Debug.WriteLine("teraz tutaj");
            while (responseMessage.IsSuccessStatusCode)
            {
                Debug.WriteLine("XD");
                using(WebClient client2 = new WebClient())
                {
                    try
                    {
                        //client2.DownloadFile(adres, $"../../tresci http/tresc_{ustawa.Dokument_Id}.html");
                            tresc += client2.DownloadString(adres);
                            counter++;

                            responseMessage = await Client.GetAsync(adres);
                        
                    }
                    catch (HttpRequestException hEx)
                    {
                        MessageBox.Show(hEx.Message);
                    }
                    catch (WebException we)
                    {
                        MessageBox.Show("Sie popsuło\n" + we.Message);
                    }
                    catch (Exception e)
                    {
                       MessageBox.Show(e.Message);
                    }
               }
            }
            
            Debug.WriteLine("zapisuję");
            //File.WriteAllText($"../../tresci http/tresc_{ustawa.Dokument_Id}.html", tresc);
            Debug.WriteLine("zapisane");
            #endregion
        }
    }
}
