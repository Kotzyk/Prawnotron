﻿using System;
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

        public static async Task<List<Ustawa>> szukajAsync(params string[] conditions)
        {
            string apiStr = client.BaseAddress.ToString();
            StringBuilder sb = new StringBuilder(apiStr);
            sb.Replace('/', '?', apiStr.Length - 1, 1);
            foreach (string warunek in conditions)
            {
                //TODO: zrobić tak żeby pod {0} wstawiać nazwę zmiennej, a pod {1} jej wartość
                sb.AppendFormat("&conditions[dziennik_ustaw.{0}]={1}", nameof(warunek), warunek);
            }

            HttpResponseMessage responseMessage = await client.GetAsync(sb.ToString());
            if (responseMessage.IsSuccessStatusCode)
            {
                //TODO: Tutaj wstawić metodę zapisująco - parsującą
                //TODO: return lista ustaw do wyświetlenia w GUI
            }

            return null;
        }

        private static void RemoveDatasetName(string jsonPath, string dataset = "dziennik_ustaw.")
        {
            string text = File.ReadAllText(jsonPath);
            text = text.Replace(dataset, "");
            File.WriteAllText(jsonPath, text);
        }

        private static async Task<Ustawa> GetUstawaAsynch(string path)
        {
            RemoveDatasetName(path);
            //Ustawa ustawa = JsonConvert.DeserializeObject<Ustawa>(File.ReadAllText(path));

            string ust_str = File.ReadAllText("Ustawa_2137.json");
            JObject mp_result = JObject.Parse(ust_str);
            JToken result = mp_result["data"];
            //TO DZIAŁA, MAMY POPRAWNY OBIEKT KLASY USTAWA!!!
            Ustawa ustawa = JsonConvert.DeserializeObject<Ustawa>(result.ToString());


            //TODO: Piotrek, zrób zapis do pliku z
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                //zapis do pliku, obróbka i deserializacja do <Ustawa>
            }
            return ustawa;
        }
    }
}