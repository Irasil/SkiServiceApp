using Newtonsoft.Json;
using SkiServiceApp.Model;
using SkiServiceApp.ModelView;
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
using System.Windows;
using static System.Net.WebRequestMethods;

namespace SkiServiceApp.Database
{
    class Database : ViewModelBase
    {
        static string _connectionString = Settings.Default.REST_URL;
        static MainWindowModelView view = new MainWindowModelView();    

        public Database() {}

        public static async Task<List<Registrationen>?> Get()
        {
            try
            {
                using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(900);
                var content = await client.GetStringAsync(_connectionString);
                return JsonConvert.DeserializeObject<List<Registrationen>>(content);
            }

            }catch{return null;}
            
        }

        public static async void Post(Registrationen reg)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(900);
                    var respons = await client.PostAsJsonAsync(_connectionString, reg);
                }
            }
            catch (Exception ex){MessageBox.Show(ex.Message);}
        }       

        public static async void Put(Registrationen reg)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Default.JWT);
                    client.Timeout = TimeSpan.FromSeconds(900);
                    var respons = await client.PutAsJsonAsync($"{_connectionString}{reg.Id}", reg);
                }
            }
            catch (Exception ex){MessageBox.Show(ex.Message);}            
        }

        public static async void Delete(Registrationen reg)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Default.JWT);
                    client.Timeout = TimeSpan.FromSeconds(900);
                    var respons = await client.DeleteAsync($"{_connectionString}{reg.Id}");
                }
            }
            catch (Exception ex){MessageBox.Show(ex.Message);}            
        }

        public static async Task<bool> Login(User user)
        {

            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(900);
                    var respons = await client.PostAsJsonAsync($"https://localhost:7153/Mitarbeiter", user);
                    string resultContent = await respons.Content.ReadAsStringAsync();
                    if (respons.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(resultContent);
                        Settings.Default.JWT = myDeserializedClass.value.token;
                        Settings.Default.Save();
                        return true;
                    }
                    else {return false;}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }            
        }
    }

    public class Root
    {
        public object contentType { get; set; }
        public object serializerSettings { get; set; }
        public object statusCode { get; set; }
        public Value value { get; set; }
    }

    public class Value
    {
        public string userName { get; set; }
        public string token { get; set; }
    }

    public class Anmelden : ViewModelBase
    {
        public string status = "Anmelden";
        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                SetProperty<string>(ref status, value);
                OnPropertyChanged(nameof(Status));
            }
        }
    }

    public class Status : ViewModelBase
    {
        public string status = string.Empty;
        public string Statuse
        {
            get { return status; }
            set
            {
                status = value;
                SetProperty<string>(ref status, value);
                OnPropertyChanged(nameof(Statuse));
            }
        }
    }
}
