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
using static System.Net.WebRequestMethods;

namespace SkiServiceApp.Database
{
    class Database
    {
        static string _connectionString = Settings.Default.REST_URL;


        public Database() 
        {
           
        }

        public static async Task<List<Registrationen>> Get()
        {            
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(900);
                var content = await client.GetStringAsync(_connectionString);
                return JsonConvert.DeserializeObject<List<Registrationen>>(content);
            }
        }

        public static async void Post(Registrationen reg)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(900);
                var respons = await client.PostAsJsonAsync(_connectionString, reg);
                string resultContent = await respons.Content.ReadAsStringAsync();
            }
        }

        public static async void Put(Registrationen reg)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(900);
                var respons = await client.PutAsJsonAsync($"{_connectionString}/{reg.Id}" ,reg);
            }
        }

        public static async void Delete(Registrationen reg)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(900);
                var respons = await client.DeleteAsync($"{_connectionString}/{reg.Id}");
            }
        }
    }
}
