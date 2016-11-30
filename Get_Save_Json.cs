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

       
    }
}
