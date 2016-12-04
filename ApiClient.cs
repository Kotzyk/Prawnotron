using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Prawnotron
{
    public class ApiClient
    {
        static readonly HttpClient Client = new HttpClient();

        public ApiClient()
        {
            Client.BaseAddress = new Uri("https://api-v3.mojepanstwo.pl/dane/dziennik_ustaw/");
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        //@AsiaMotyczynska ogarnij i stosuj wzór
        /// <summary>
        /// Metoda wyszukująca ustawy w API korzystając z metody ?conditions[].
        /// Pobiera listę par (nazwa pola w GUI, treść pola w GUI),
        ///  a zwraca listę <see cref="Ustawa"/> do wyświetlenia w interfejsie.
        /// </summary>
        /// <param name="conditions">
        ///     Lista <see cref="KeyValuePair{TKey,TValue}"/>
        ///     Key = Nazwa <code>TextBox</code>,
        ///     Value = Zawartość <code>TextBox</code>
        /// </param>
        /// <returns>Lista <code>Ustaw</code>do wyświetlenia w GUI <see cref="Ustawa"/> </returns>       
        public static async Task<List<Ustawa>> SzukajAsync(List<KeyValuePair<string,string>> conditions)
        {   
            //check if null
            if (conditions == null) throw new ArgumentNullException(nameof(conditions));
            //check if empty
            if (conditions.Count == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(conditions));
            
            //przykład wyszukiwania konkretnej ustawy znając sygnaturę Dz.U (Ustawa o przeciwdziałaniu narkomanii, HTML ma 6 stron treści)
            //https://api-v3.mojepanstwo.pl/dane/dziennik_ustaw?conditions[dziennik_ustaw.poz]=1485&conditions[dziennik_ustaw.nr]=179&conditions[dziennik_ustaw.rok]=2005

            string apiStr = Client.BaseAddress.ToString();
            StringBuilder sb = new StringBuilder(apiStr);
            sb.Replace('/', '?', apiStr.Length - 1, 1);
            foreach (KeyValuePair<string,string> warunek in conditions)
            {
                //pod {0} wstawiać nazwę zmiennej, a pod {1} jej wartość
                
                sb.AppendFormat("&conditions[dziennik_ustaw.{0}]={1}", warunek.Key, warunek.Value);
            }

            HttpResponseMessage responseMessage = await Client.GetAsync(sb.ToString());
            if (responseMessage.IsSuccessStatusCode)
            {
                //TODO: Tutaj wstawić metodę zapisująco - parsującą
                //TODO: return lista ustaw do wyświetlenia w GUI
            }

            return null;
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

        static async Task<Ustawa> GetUstawaAsynch(string path)
        {
            RemoveDatasetName(path);
            //Ustawa ustawa = JsonConvert.DeserializeObject<Ustawa>(File.ReadAllText(path));

            string ustStr = File.ReadAllText("Ustawa_2137.json");
            JObject mpResult = JObject.Parse(ustStr);
            JToken result = mpResult["data"];
            //TO DZIAŁA, MAMY POPRAWNY OBIEKT KLASY USTAWA!!!
            Ustawa ustawa = JsonConvert.DeserializeObject<Ustawa>(result.ToString());

           
            HttpResponseMessage response = await Client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                //zapis do pliku, obróbka i deserializacja do <Ustawa>
                //TODO: ustawy wielostronnicowe
            }
            return ustawa;
        }
        //TODO: poprawić to i scalić z resztą
        static async Task Getsavejson(int id)
        {
            using (Client)
            {
                string ustawa;
                try
                {
                    HttpResponseMessage response = await Client.GetAsync("https://api-v3.mojepanstwo.pl/dane/dziennik_ustaw/" + id);
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
               
                using (StreamWriter sw = File.CreateText("ustawa_" + id + ".json"))
                {
                    JsonSerializer j = new JsonSerializer {Formatting = Formatting.Indented};
                    j.Serialize(sw, obj);
                }
            }
        }
    
        //zalążek metody
        /*private static async Task GetContentAsync(Ustawa ustawa){
            HttpResponseMessage responseMessage;
            var trescStream = new FileStream("tresc.html", FileMode.CreateNew);
            StringBuilder sb = new StringBuilder("https://docs.mojepanstwo.pl/htmlex/");
            
        }*/
    }
}
