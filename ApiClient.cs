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
                    JsonSerializer j = new JsonSerializer { Formatting = Formatting.Indented };
                    j.Serialize(sw, obj);
                    sw.Close();
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
            if (conds == null) throw new ArgumentNullException(nameof(conds));
            //check if empty
            if (conds.Count == 0)
                throw new ArgumentException(Resources.cannot_be_an_empty_collection, nameof(conds));

            List<Ustawa> wynikiList = new List<Ustawa>();

            string apiStr = Resources.DzU_Search;
            StringBuilder sb = new StringBuilder(apiStr);
            foreach (var warunek in conds)
                sb.Append($"&conditions[dziennik_ustaw.{warunek.Key}]={warunek.Value}");

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
                    if(i < (podustawy.Length -1))
                        temp = temp + "}}";
                    podustawy[i] = temp;
                }
                for (int i = 0; i < podustawy.Length; i++)
                {
                    using (StreamWriter sw = File.CreateText($"../../Json/Ustawa_{(i + 1)}.json"))
                    {
                        sw.Write(podustawy[i]);
                    }                   
                }
                for (int i = 0; i < podustawy.Length; i++)
                {
                    wynikiList.Add(new Ustawa(i + 1));
                }
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
        static async Task GetContentAsync(Ustawa ustawa)
        {
            //Ciesz się, Piotrek
            byte counter = 1;
            HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.Accepted);
            Stream trescStream = new FileStream("tresc" + ustawa.Id + ".html", FileMode.CreateNew);
            StringBuilder sb = new StringBuilder(Resources.Base_API2);

            string adres = $"{ustawa.DokumentId}/{ustawa.DokumentId}_";
            sb.Append(adres + counter + ".html");
            try
            {
                responseMessage = await Client.GetAsync(sb.ToString());
            }
            catch (HttpRequestException e)
            {
                Debug.Fail(e.Message);
            }
            //przykład wyszukiwania konkretnej ustawy znając sygnaturę Dz.U (Ustawa o przeciwdziałaniu narkomanii, HTML ma 6 stron treści)
            //https://api-v3.mojepanstwo.pl/dane/dziennik_ustaw?conditions[dziennik_ustaw.poz]=1485&conditions[dziennik_ustaw.nr]=179&conditions[dziennik_ustaw.rok]=2005

            #region DownloadRegion

            while (responseMessage.IsSuccessStatusCode)
            {
                using (Client)
                {
                    try
                    {
                        responseMessage = await Client.GetAsync(sb.ToString());
                        Stream content = await Client.GetStreamAsync(sb.ToString());
                        content.CopyTo(trescStream);

                        //wyrażenia warunkowe na wypadek dwucyfrowej liczby stron
                        sb.Replace(counter.ToString(), (counter + 1).ToString(),
                            counter < 10 ? sb.Length - 6 : sb.Length - 7,
                            counter < 10 ? 1 : 2);
                        counter++;
                    }
                    catch (HttpRequestException hEx)
                    {
                        Debug.Fail(hEx.Message);
                    }
                    catch (Exception e)
                    {
                        Debug.Fail(e.Message);
                    }
                }
                trescStream.Close();
                #endregion
            }
        }
    }
}
