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
            using (var client = new HttpClient())
            {
                var content = await client.GetStringAsync("https://localhost:7153/Registration");
                return JsonConvert.DeserializeObject<List<Registrationen>>(content);
            }
        }

        public static async void Post(Registrationen reg)
        {
            using (var client = new HttpClient())
            {
                var respons = await client.PostAsJsonAsync("https://localhost:7153/Registration", reg);
                string resultContent = await respons.Content.ReadAsStringAsync();
            }
        }
    }
}
