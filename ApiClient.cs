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
            JsonConverter jsonConverter = new DataSetConverter();
           JsonReader jsonReader = new BsonReader(File.OpenRead("Ustawa_2137.json"));
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                RemoveDatasetName("Ustawa_2137.json");

                jsonConverter.ReadJson(jsonReader,typeof(Ustawa), ustawa, new JsonSerializer());
                ustawa = await response.Content.ReadAsAsync<Ustawa>();
            }
            return ustawa;
        } 
        

      
    }
}
