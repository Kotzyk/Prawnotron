using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace PobieranieJson
{
    class Program
    {
        //To chciałem zrobić jako osobną funkcję ale wtedy nie mam pojęcia jak zapisać w nazwie pliku id
        /*private static void savejsonfile(string file, string path)
        {
            JObject obj = JObject.Parse(file);
            using (StreamWriter sw = new StreamWriter(path))
            {
                JsonSerializer j = new JsonSerializer();
                j.Formatting = Formatting.Indented;
                j.Serialize(sw, obj);
            }
        }*/

        private static async Task<string> getsavejson(int id)
        {
            string ustawa;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync("https://api-v3.mojepanstwo.pl/dane/dziennik_ustaw/" + id);
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
                    client.Dispose();
                    ustawa = e.Message;
                }
                JObject obj = JObject.Parse(ustawa);
                using (StreamWriter sw = new StreamWriter("ustawa_"+id+".json"))
                {
                    JsonSerializer j = new JsonSerializer();
                    j.Formatting = Formatting.Indented;
                    j.Serialize(sw, obj);
                }
                return ustawa;
            }
        }

        static void Main(string[] args)
        {
            getsavejson(22);
            Console.ReadKey();
        }
    }
}
