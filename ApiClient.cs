using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Prawnotron.Properties;

namespace Prawnotron
{
    /// <summary>
    /// Klasa odpowiedzialna za łączność z API poprzez HTTP i pobieranie szczegółów oraz treści ustaw.
    /// </summary>
    public class ApiClient
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
        /// Metoda usuwająca nazwę datasetu z parametrów we wskazanym pliku JSON
        /// </summary>
        /// <param name="jsonPath">ścieżka do pliku JSON</param>
        /// <param name="dataset">nazwa datasetu z API, np. prawo, dziennik_ustaw. Domyślnie "dziennik_ustaw"</param>
        static void RemoveDatasetName(string jsonPath, string dataset = "dziennik_ustaw.")
        {
            string text = File.ReadAllText(jsonPath);
            text = text.Replace(dataset, "");
            File.WriteAllText(jsonPath, text);
        }

        /// <summary>
        /// Deserialuje pliki <code>JSON</code> do klasy <see cref="Ustawa"/>
        /// </summary>
        /// <param name="path">ścieżka do pliku <code>JSON</code></param>
        /// <returns>Deserializowany obiekt klasy Ustawa</returns>
        protected static Ustawa ParseUstawa(string path)
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
        static async Task Getsavejson(int id)
        {
            using (Client)
            {
                string ustawa;
                try
                {
                    HttpResponseMessage response = await
                        Client.GetAsync("https://api-v3.mojepanstwo.pl/dane/dziennik_ustaw/" + id);
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
                    JsonSerializer j = new JsonSerializer {Formatting = Formatting.Indented};
                    j.Serialize(sw, obj);
                    sw.Close();
                }
            }
        }

        /// <summary>
        /// Metoda wyszukująca ustawy w API korzystając z metody ?conditions[].
        /// Pobiera listę par (nazwa pola w GUI, treść pola w GUI),
        ///  a zwraca listę <see cref="Ustawa"/> do wyświetlenia w interfejsie.
        /// </summary>
        /// <param name="conditions">
        ///     Lista <see cref="KeyValuePair{TKey,TValue}"/>:
        ///     Key = Nazwa parametru, np.<see cref="Ustawa.Rok"/>;
        ///     Value = Zawartość <code>TextBox</code>
        /// </param>
        /// <returns>Lista <code>Ustaw</code>do wyświetlenia w GUI <see cref="Ustawa"/> </returns>
        public static async Task<List<Ustawa>> SzukajAsync(List<KeyValuePair<string, string>> conditions)
        {
            //check if null
            if (conditions == null) throw new ArgumentNullException(nameof(conditions));
            //check if empty
            if (conditions.Count == 0)
                throw new ArgumentException(Resources.cannot_be_an_empty_collection, nameof(conditions));

            //przykład wyszukiwania konkretnej ustawy znając sygnaturę Dz.U (Ustawa o przeciwdziałaniu narkomanii, HTML ma 6 stron treści)
            //https://api-v3.mojepanstwo.pl/dane/dziennik_ustaw?conditions[dziennik_ustaw.poz]=1485&conditions[dziennik_ustaw.nr]=179&conditions[dziennik_ustaw.rok]=2005

            string apiStr = Client.BaseAddress.ToString();
            StringBuilder sb = new StringBuilder(apiStr);
            sb.Replace('/', '?', apiStr.Length - 1, 1);
            foreach (KeyValuePair<string, string> warunek in conditions)
            {
                //pod {0} wstawiać nazwę zmiennej, a pod {1} jej wartość

                sb.AppendFormat("&conditions[dziennik_ustaw.{0}]={1}", warunek.Key, warunek.Value);
            }

            HttpResponseMessage responseMessage = await Client.GetAsync(sb.ToString());
            if (responseMessage.IsSuccessStatusCode)
            {
                //TODO: Tutaj wstawić metodę zapisująco - parsującą

            }
            //TODO: return lista ustaw do wyświetlenia w GUI
            return null;
        }

        /// <summary>
        /// Zapisuje treść wybranej ustawy z API do pliku .html do dalszej obróbki.
        /// </summary>
        /// <param name="ustawa"><see cref="Ustawa"/>, której treść pobieramy</param>
        /// <returns>Treść ustawy zapisana w pliku .html</returns>
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
                Debug.WriteLine(e.Message);
                throw;
            }

                while (responseMessage.IsSuccessStatusCode)
                {
                    using (Client)
                    {
                    responseMessage = await Client.GetAsync(sb.ToString());
                    Stream content = await Client.GetStreamAsync(sb.ToString());
                        content.CopyTo(trescStream);
                    //wyrażenia warunkowe na wypadek dwucyfrowej liczby stron
                        sb.Replace(counter.ToString(), (counter + 1).ToString(),
                            (counter<10)?(sb.Length-6):(sb.Length-7),
                            (counter<10)?1:2);
                        counter++;
                    }

                }
                trescStream.Close();
            }
        }
    }
