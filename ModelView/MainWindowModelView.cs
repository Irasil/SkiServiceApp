using SkiServiceApp.Model;
using SkiServiceApp.Database;
using SkiServiceApp.Utility;
using SkiServiceApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows;
using System.Globalization;

namespace SkiServiceApp.ModelView
{
    /// <summary>
    /// Klasse für das MVVM
    /// </summary>
    public class MainWindowModelView : ViewModelBase
    {
        /// <summary>
        ///
        /// </summary>
         
        #region Command Properties
        public RelayCommand CmdAktu { get; set; }
        public RelayCommand CmdNeu { get; set; }
        public RelayCommand CmdNeuEr { get; set; }
        public RelayCommand CmdAendern { get; set; }
        public RelayCommand CmdLoeschen { get; set; }
        public RelayCommand CmdAnmelden { get; set; }
        public RelayCommand CmdAnmeldenSenden { get; set; }
        public RelayCommand CmdAendernSpeicher { get; set; }
        public RelayCommand CmdLoeschenSpeicher { get; set; }
        public RelayCommand CmdSuche { get; set; }
        public RelayCommand CmdDeblock { get; set; }
        public RelayCommand CmdDeblockSenden { get; set; }
        public RelayCommand CmdApi { get; set; }
        public RelayCommand CmdApiSenden { get; set; }
        #endregion

        #region Properties Initialisieren
        public Registrationen _regi = new Registrationen();
        public User _user  = new User();
        public LoginView login { get; set; }
        public Anmelden _anmeld = new Anmelden();
        public Status _status = new Status();
        public API _api = new API();
        public HomeView home = new HomeView();
        public bool isloging { get; set; } = true;
        public bool isfilled { get; set; } = false;
        public int? deblock { get; set; } = null;
        public string abhol { get; set; } = string.Empty;


        public Task<string> answerT { get; set; }
        public Task<List<Registrationen>> getTask { get; set; }
        public List<Registrationen> registrations { get; set; } = new List<Registrationen>();
        public ObservableCollection<Registrationen> Registrationens { get; set; }

        #endregion

        /// <summary>
        /// Konstruktor: Alle Commands Initialisieren und auf ihre Ausführbarkeit prüfen
        /// </summary>
        public MainWindowModelView()
        {
            Content = home;          
           
            Registrationens = new ObservableCollection<Registrationen>();
            
            CmdAktu = new RelayCommand(param => Aktu());
            CmdNeu = new RelayCommand(param => Neu());
            CmdNeuEr = new RelayCommand(param => NeuEr(), param => CanAendernSpeichern());
            CmdAendern = new RelayCommand(param => Aendern(_regi) , param => CanAendern());
            CmdLoeschen = new RelayCommand(param => Loeschen(_regi), param => CanAendern());
            CmdAendernSpeicher = new RelayCommand(param => AendernSpeicher(_regi) , param=> CanAendernSpeichern());
            CmdLoeschenSpeicher = new RelayCommand(param => LoeschenSpeicher());
            CmdAnmelden = new RelayCommand(param => Anmelden());
            CmdAnmeldenSenden = new RelayCommand(param => AnmeldenSenden(), param => CanAnmeldenSenden());
            CmdSuche = new RelayCommand(param => Suche());
            CmdDeblock = new RelayCommand(param => Deblock(), param => CanAendern());
            CmdDeblockSenden = new RelayCommand(param => DeblockSenden(), param => CanDeblockSenden());
            CmdApi = new RelayCommand(param => Api());
            CmdApiSenden = new RelayCommand(param => ApiSenden() , param => CanApiSenden());
        }

        #region Properties OnPropertyChange
        /// <summary>
        /// Property Registrationen
        /// </summary>
        public Registrationen reg
        {
            get { return _regi; }
            set
            {
                _regi = value;
                SetProperty<Registrationen>(ref _regi, value);
                OnPropertyChanged(nameof(reg));
            }
        }

        /// <summary>
        /// Property User
        /// </summary>
        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
                SetProperty<User>(ref _user, value);
                OnPropertyChanged(nameof(User));
            }
        }

        /// <summary>
        /// Property An- und Abmelden
        /// </summary>
        public Anmelden Anmeld
        {
            get { return _anmeld; }
            set
            {
                _anmeld = value;
                SetProperty<Anmelden>(ref _anmeld, value);
                OnPropertyChanged(nameof(Anmeld));
            }
        }

        /// <summary>
        /// Property Statusbar
        /// </summary>
        public Status Status
        {
            get { return _status; }
            set
            {
                _status = value;
                SetProperty<Status>(ref _status, value);
                OnPropertyChanged(nameof(Status));
            }
        }

        /// <summary>
        /// Property API-Link
        /// </summary>
        public API Apis
        {
            get { return _api; }
            set
            {
                _api = value;
                SetProperty<API>(ref _api, value);
                OnPropertyChanged(nameof(Apis));
            }
        }

        /// <summary>
        /// Property Angezeigter Kontent im MainWindow
        /// </summary>
        private object content;
        public object Content
        {
            get { return content; }
            set
            {
                content = value;
                SetProperty<object>(ref content, value);
                OnPropertyChanged(nameof(Content));
            }
        }
        #endregion


        #region CRUD Registrationen

        /// <summary>
        /// Get Verbindung mit API Prüfen und Anzeigen der Resultate
        /// </summary>
        public async void Aktu()
        {
            try
            {
                _regi = new Registrationen();
                await (getTask = Database.Database.Get());
                if (getTask.Result != null)
                {
                    registrations = getTask.Result.ToList();
                    AktuView aktu = new AktuView();
                    Registrationens = new ObservableCollection<Registrationen>(registrations);
                    Content = aktu;
                    Status.Statuse = "Efolgreiche Verbindung zum Server";
                }
                else
                {
                    Status.Statuse = "Leider konnte keine Verbindung zum Server hergestellt werden";
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        /// <summary>
        /// Anzeige der View, um eine neue Registration zu erstellen
        /// </summary>
        public void Neu()
        {
            try
            {
                _regi = new Registrationen();
                _regi.Service = "Kleiner Service";
                _regi.Priority = "Express";
                NeuView neu = new NeuView();
                Content = neu;
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }            
        }

        /// <summary>
        /// Post Verbindung zu der API und setzen des Abholdatums. Anzeige nach Erfolgreicher Bestellung
        /// </summary>
        private void NeuEr()
        {
            try
            {
                reg.Created_Date = DateTime.Now;
                reg.Status = "Offen";

                if (reg.Priority == "Tief")
                {
                    reg.Pickup_Date = reg.Created_Date.AddDays(12);
                }
                else if (reg.Priority == "Standart")
                {
                    reg.Pickup_Date = reg.Created_Date.AddDays(7);
                }
                else if (reg.Priority == "Express")
                {
                    reg.Pickup_Date = reg.Created_Date.AddDays(5);
                }
                Database.Database.Post(reg);
                ErfolgreichView erfolgreich = new ErfolgreichView();
                Content = erfolgreich;
                Status.Statuse = "Erfolgreich erstellt!";
                abhol = reg.Pickup_Date.ToString().Substring(0,10);
                reg = new Registrationen();           
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        /// <summary>
        /// Anzeige der View, um ein Datansatz zu bearbeiten, falls nichts Ausgewählt ist lade alle Registrationen
        /// </summary>
        /// <param name="reg"></param>
        private void Aendern(Registrationen reg)
        {
            try
            {
                if (_regi.Name == null)
                {
                    Aktu();
                }
                else
                {                    
                    AendernView neu = new AendernView();
                    Content = neu;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        /// <summary>
        /// Überprüfung ob man eingelogt ist
        /// </summary>
        /// <returns></returns>
        private bool CanAendern()
        {
            try
            {
                if (isloging)
                {
                    return true;
                }
                else { return false; }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return false; }
        }

        /// <summary>
        /// Anzeige der View, um einen Datensatz zu löschen
        /// </summary>
        /// <param name="reg"></param>
        private void Loeschen(Registrationen reg)
        {
            try
            {
                if (_regi.Name != null)
                {
                    LoeschenView neu = new LoeschenView();
                    Content = neu;
                }
                else
                {
                    Aktu();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message);}
        }

        /// <summary>
        /// Put Verbindung zu der API, nach erfolgreichem ändern werden alle Datensätze neu geladen
        /// </summary>
        /// <param name="reg"></param>
        public async void AendernSpeicher(Registrationen reg)
        {
            try
            {
                Database.Database.Put(reg);
                await Task.Delay(100);
                Aktu();
                await Task.Delay(100);
                Status.Statuse = "Erfolgreich aktualisiert!";
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        /// <summary>
        /// Prüfen ob eine Änderung gemacht werden darf
        /// </summary>
        /// <returns></returns>
        public bool CanAendernSpeichern()
        {         
            try
            {
                if (_regi == null)
                {
                    return false;
                }
                else
                {
                    return _regi.Name != null && _regi.Email != null && _regi.Phone != null && _regi.Name != "" && _regi.Email != "" && _regi.Phone != "";
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return false; }
        }

        /// <summary>
        /// Dalete Verbindung zu der API, nach erfolgreichem Löschen werden alle Datensätze neu geladen
        /// </summary>
        private async void LoeschenSpeicher()
        {       
            try
            {
                Database.Database.Delete(_regi);
                await Task.Delay(100);
                Aktu();
                await Task.Delay(100);
                Status.Statuse = "Erfolgreich gelöscht!";
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        #endregion

        #region Mitarbeiter Anmeldung
        /// <summary>
        /// Falls ausgelogt, wird die Loging View geladen, ansonsten wird man ausgelogt
        /// </summary>
        public async void Anmelden()
        {        
            try
            {
                if (Anmeld.Status == "Anmelden")
                {
                    User.Name = string.Empty;
                    login = new LoginView();
                    Content = login;
                }
                else if (Anmeld.Status == "Abmelden")
                {
                    isloging = false;
                    Anmeld.Status = "Anmelden";
                    Status.Statuse = "Erfolgreich Abgemeldet";
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
       
        /// <summary>
        /// Post Verbinbdung zu der API, Ausgabe der API Antwort in der Statusbar
        /// </summary>
        public async void AnmeldenSenden()
        {       
            try
            {
                _user.password = login.pwb.Password;
                Task<string> suc;
                await (suc = Database.Database.Login(_user));
                if (suc != null)
                {
                    string success = suc.Result;
                    if (success == "User ist blockiert")
                    {
                        Aktu();
                        await Task.Delay(100);
                        Status.Statuse = success;
                    }
                    else if (success == "User oder Passwort sind falsch")
                    {
                        Anmelden();
                        await Task.Delay(100);                        
                        Status.Statuse = success;
                    }
                    else
                    {
                        Aktu();
                        await Task.Delay(100);
                        isloging = true;
                        Anmeld.Status = "Abmelden";
                        Status.Statuse = "Erfolgreich Angemeldet";
                    }
                }
                else
                {
                    Aktu();
                }
                User.Name = string.Empty;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        /// <summary>
        /// Überprüft ob alle Felder ausgefüllt sind
        /// </summary>
        /// <returns></returns>
        public bool CanAnmeldenSenden()
        {
            if (login.pwb.Password != string.Empty && _user.Name != string.Empty) { return true; } else { return false; }
        }
        
        /// <summary>
        /// Anzeige der View, um ein User zu deblockieren
        /// </summary>
        private async void Deblock()
        {
            DeblockView deblockenView = new DeblockView();
            Content = deblockenView;
        }

        /// <summary>
        /// Put Verbindung zu der API, um einen Mitarbeiter zu entblocken, API Antwort in der Statusbar
        /// </summary>
        private async void DeblockSenden()
        {            
            await (  answerT = Database.Database.PutMember(deblock));
            string answer = answerT.Result.ToString();
            if (answer == "Mitarbeiter wurde wieder freigegeben")
            {
                Aktu();
                await Task.Delay(100);
                Status.Statuse = answer;
            }
            else{ Deblock(); Status.Statuse = answer; }
            Status.Statuse = answer;            
        }

        /// <summary>
        /// Überprüft, ob eine Mitarbeiter Id eingegeben wurde.
        /// </summary>
        /// <returns></returns>
        private bool CanDeblockSenden()
        {
            return deblock != null;
        }

        #endregion
        #region Suche

        /// <summary>
        /// Get Verbindung zu der API, Antwort nach Suchbegriff durchsuchen und alle passenden Registrationen im MainWindow anzeigen.
        /// </summary>
        private async void Suche()
        {
            try
            {
                _regi = new Registrationen();
                await (getTask = Database.Database.Get());
                registrations = getTask.Result.ToList();
                List<Registrationen> Registries = new List<Registrationen>();
                Registries = registrations.ToList();

                if (_user.Name != null)
                {
                    Registries.Clear();
                    foreach (Registrationen anim in registrations)
                    {
                        if (anim.Name.Contains(_user.Name) || anim.Email.Contains(_user.Name) || anim.Service.Contains(_user.Name) || anim.Priority.Contains(_user.Name) || anim.Status.Contains(_user.Name))
                        {
                            Registries.Add(anim);
                        }
                    }
                }
                AktuView aktu = new AktuView();
                Registrationens = new ObservableCollection<Registrationen>(Registries);
                Content = aktu;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        #endregion
        #region API-Link

        /// <summary>
        /// Anzeige der View, um den API-Link zu ändern
        /// </summary>
        private async void Api()
        {
            Apis.Api = Settings.Default.REST_URL;
            APILinkView aPILinkView = new APILinkView();
            Content = aPILinkView;
        }

        /// <summary>
        /// Neuer API-Link in Settings speichern
        /// </summary>
        private async void ApiSenden()
        {
            Settings.Default.REST_URL = Apis.Api.ToString();
            string lol = Settings.Default.REST_URL;
            Status.Statuse = "Api-Link erfolgreich gespeichert";
            Content = home;
        }

        /// <summary>
        /// Überprüfen ob eingelogt und kein lehres Feld.
        /// </summary>
        /// <returns></returns>
        private bool CanApiSenden()
        {
            return Apis.Api != null && Apis.Api != "" && isloging;
        }
        #endregion
    }
}

