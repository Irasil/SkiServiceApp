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


        /// <summary>
        /// Get Verbindung zu der API
        /// </summary>
        /// <returns> Eine Task-Liste von Registrationen</returns>
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
        /// <summary>
        /// Post Verbindung zu der API
        /// </summary>
        /// <param name="reg">Die Erstellte Registration</param>
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

        /// <summary>
        /// Put Verbindung zu der API
        /// </summary>
        /// <param name="reg">Die Registration mit der Änderung</param>
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

        /// <summary>
        /// Delete Verbindung zu der API
        /// </summary>
        /// <param name="reg">Die Registration die gelöscht werden soll</param>
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

        /// <summary>
        /// Mitarbeiter Login Post Verbindung zu der API, Speichern des JWT in den Settings
        /// </summary>
        /// <param name="user">Der Name des Mitarbeiters</param>
        /// <returns>Die Antwort der API</returns>
        public static async Task<string> Login(User user)
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
                        return resultContent;
                    }
                    else {return resultContent;}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return ex.Message;
            }            
        }

        /// <summary>
        /// Put Verbindung zu der API, um geblockte Mitarbeiter zu entblocken
        /// </summary>
        /// <param name="id">Die Id des zu entblockenden Mitarbeiters</param>
        /// <returns>Die Antwort der API</returns>
        public static async Task<string> PutMember(int? id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Settings.Default.JWT);
                    client.Timeout = TimeSpan.FromSeconds(900);
                    var respons = await client.PutAsJsonAsync($"https://localhost:7153/Mitarbeiter/{id}", id);
                    string resultContent = await respons.Content.ReadAsStringAsync();
                    return resultContent;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return null; }
        }
    }

    /// <summary>
    /// Klasse für die Objektifizierung des JWTs
    /// </summary>
    public class Root
    {
        public object contentType { get; set; }
        public object serializerSettings { get; set; }
        public object statusCode { get; set; }
        public Value value { get; set; }
    }

    /// <summary>
    /// Klasse für den Inhalt der Autentifizierungs Antwort
    /// </summary>
    public class Value
    {
        public string userName { get; set; }
        public string token { get; set; }
    }

    /// <summary>
    /// Klasse Für die Anzeige, ob man Angemeldet oder Abgemeldet ist
    /// </summary>
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

    /// <summary>
    /// Klasse für den Inhalt der Statusbar
    /// </summary>
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

    /// <summary>
    /// Klasse für den API-Link
    /// </summary>
    public class API : ViewModelBase
    {
        public string _api = string.Empty;
        public string Api
        {
            get { return _api; }
            set
            {
                _api = value;
                SetProperty<string>(ref _api, value);
                OnPropertyChanged(nameof(Api));
            }
        }

    }
}
