using Newtonsoft.Json;
using SkiServiceApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Policy;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SkiServiceApp.Database
{
    class Database
    {
        
        public Database() 
        {
            
        }

        public static async Task<List<Registrationen>> Get()
        {
            //HttpClient get = new HttpClient();
            //get.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //string text = await get.GetStringAsync("https://localhost:7153/Registration");

            //string hey = text.Result.ToString();
            ////Keywordlist gag = JsonConvert.DeserializeObject<Keywordlist>(text);


            //return text;
            using (var client = new HttpClient())
            {
                var content = await client.GetStringAsync("https://localhost:7153/Registration");
                return JsonConvert.DeserializeObject<List<Registrationen>>(content);
            }


        }
    }
}
