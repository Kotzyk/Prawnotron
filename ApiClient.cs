using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Controls;
using System.Windows.Documents;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Prawnotron.Properties;
using System.Text.RegularExpressions;

namespace Prawnotron
{
    /// <summary>
    /// Łączność z API poprzez <see cref="HttpClient" /> i pobieranie szczegółów oraz treści ustaw. <seealso cref="Ustawa" /><seealso cref="ApiClient.ParseUstawa" />
    /// </summary>
    public sealed class ApiClient
    {
        static readonly HttpClient Client = new HttpClient();

        /// <summary>
        /// Konstruktor przypisujący podstawowe parametry klienta HTTP
        /// </summary>
        public ApiClient()
        {
            Client.BaseAddress = new Uri(Resources.Base_API1);
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Metoda usuwająca nazwę datasetu z parametrów we wskazanym pliku <c>JSON</c>
        /// </summary>
        /// <param name="jsonPath">ścieżka do pliku <c>JSON</c></param><seealso cref="File"/>
        /// <param name="dataset">nazwa datasetu z API, np. prawo, dziennik_ustaw. Domyślnie "dziennik_ustaw"</param>
        static void RemoveDatasetName(string jsonPath, string dataset = "dziennik_ustaw.")
        {
            string text = File.ReadAllText(jsonPath);
            text = text.Replace(dataset, "");
            File.WriteAllText(jsonPath, text);
        }

        /// <summary>
        /// Deserialuje pliki <c>JSON</c> do klasy Ustawa <see cref="Ustawa"/>, korzystając z zamiany na
        /// </summary>
        /// <param name="path">ścieżka do pliku <c>JSON</c> <seealso cref="string"/></param>
        /// <returns>Deserializowany obiekt klasy Ustawa</returns>
        static Ustawa ParseUstawa(string path)
        {
            RemoveDatasetName(path);
            //Ustawa ustawa = JsonConvert.DeserializeObject<Ustawa>(File.ReadAllText(path));

            string ustStr = File.ReadAllText(path);

            JObject mpResult = JObject.Parse(ustStr);
            JToken result = mpResult["data"];

            Ustawa ustawa = JsonConvert.DeserializeObject<Ustawa>(result.ToString());
            return ustawa;
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

                using (StreamWriter sw = File.CreateText("json/ustawa_" + id + ".json"))
                {
                    JsonSerializer j = new JsonSerializer { Formatting = Formatting.Indented };
                    j.Serialize(sw, obj);
                    sw.Close();
                }
            }
        }

        /// <summary>
        /// Wyszukuje ustawy w API korzystając z metody ?conditions[].
        /// Pobiera listę par (nazwa pola w GUI, treść pola w GUI),
        /// a zwraca listę <see cref="Ustawa" /> do wyświetlenia w interfejsie.
        /// </summary>
        /// <param name="conditions">Lista <see cref="KeyValuePair{TKey,TValue}" />:
        /// Key = Nazwa parametru, np.<see cref="Ustawa.Rok" />;
        /// Value = Zawartość <see cref="TextBox" /></param>
        /// <returns>
        /// Lista <see cref="Ustawa" /> do wyświetlenia w GUI
        /// </returns>
        /// <exception cref="System.ArgumentNullException">Jeśli lista jest null</exception>
        /// <exception cref="System.ArgumentException">Jeśli lista argumentów jest pusta</exception>
        /// <seealso cref="System.Windows.Controls" />
        /// <seealso cref="GetSavejson" />
        /// <seealso cref="RemoveDatasetName" />
        public static async Task<List<Ustawa>> SzukajAsync(List<KeyValuePair<string, string>> conditions)
        {
            //check if null
            if (conditions == null) throw new ArgumentNullException(nameof(conditions));
            //check if empty
            if (conditions.Count == 0)
                throw new ArgumentException(Resources.cannot_be_an_empty_collection, nameof(conditions));

            List<Ustawa> wynikiList = new List<Ustawa>();

            string apiStr = Resources.DzU_Search;
            StringBuilder sb = new StringBuilder(apiStr);
            foreach (KeyValuePair<string, string> warunek in conditions)
            {
                //pod {0} wstawiać nazwę zmiennej / pola, a pod {1} wartość
                if (conditions.Count == 1)
                {
                    sb.AppendFormat("conditions[dziennik_ustaw.{0}]={1}", warunek.Key, warunek.Value);
                }
                else if (conditions.Count > 1)
                {
                    sb.AppendFormat("&conditions[dziennik_ustaw.{0}]={1}", warunek.Key, warunek.Value);
                }
            }

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
                    string temp = "";
                    temp = podustawy[i];
                    if(i < (podustawy.Length -1))
                        temp = temp + "}}";
                    podustawy[i] = temp;
                }
                for (int i = 0; i < podustawy.Length; i++)
                {
                    using (StreamWriter sw = new StreamWriter((i+1)+"ustawa.json", false))
                    {
                        sw.Write(podustawy[i]);
                    }
                }
                for (int i = 0; i < podustawy.Length; i++)
                {
                    wynikiList.Add(ParseUstawa((i + 1) + "ustawa.json"));
                }
                //TODO: parsowanie ustaw z listy w API - jak?
                //np. gdy nie wpisze się ID, tylko od razu (...)/dziennik_ustaw/
                //TODO: Użyć GetSaveJson, potem ParseUstawa, wrzucać do listy.
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

            string adres = ustawa.DokumentId + "/" + ustawa.DokumentId + "_";
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
