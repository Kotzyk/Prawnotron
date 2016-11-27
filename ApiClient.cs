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
    class ApiClient  
    {
        static HttpClient client = new HttpClient();
        public ApiClient()
        {
            client.BaseAddress = new Uri("https://api-v3.mojepanstwo.pl/dane/dziennik_ustaw/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static void RemoveDatasetName(string jsonPath, string dataset = "dziennik_ustaw.")
        {
            string text = File.ReadAllText(jsonPath);
            text = text.Replace(dataset, "");
            File.WriteAllText(jsonPath, text);
        }
        //(Jeszcze) Nie do końca wiem co tu robię. Trochę ctrl+v z różnych rzeczy
        //
        static async Task<Ustawa> GetUstawaAsynch(string path)
        {
            Ustawa ustawa = null;
            RemoveDatasetName("Ustawa_2137.json");
            string ust_str = File.ReadAllText("Ustawa_2137.json");
            JObject mp_result = JObject.Parse(ust_str);
            JToken result = mp_result["data"];
            ustawa = JsonConvert.DeserializeObject<Ustawa>(result.ToString());
            
            //poniżej do czytania z HTTP, powyżej do czytania z pliku Ustawa_2137.json
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                
                ustawa = await response.Content.ReadAsAsync<Ustawa>();
            }
            return ustawa;
        } 
        

      
    }
}
