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

namespace SkiServiceApp.ModelView
{
    public class MainWindowModelView : ViewModelBase
    {
        public MainWindow main;

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


        public Task<string> answerT { get; set; }
        public Task<List<Registrationen>> getTask { get; set; }
        public List<Registrationen> registrations { get; set; } = new List<Registrationen>();
        public ObservableCollection<Registrationen> Registrationens { get; set; }

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
                _regi = new Registrationen();
                
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }

        }

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

        public bool CanAnmeldenSenden()
        {
            if (login.pwb.Password != string.Empty && _user.Name != string.Empty) { return true; } else { return false; }
        }

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

        private async void Deblock()
        {
            DeblockView deblockenView = new DeblockView();
            Content = deblockenView;
        }
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

        private bool CanDeblockSenden()
        {
            return deblock != null;
        }

        private async void Api()
        {
            Apis.Api = Settings.Default.REST_URL;
            APILinkView aPILinkView = new APILinkView();
            Content = aPILinkView;
        }

        private async void ApiSenden()
        {
            Settings.Default.REST_URL = Apis.Api.ToString();
            string lol = Settings.Default.REST_URL;
            Status.Statuse = "Api-Link erfolgreich gespeichert";
            Content = home;
        }
        private bool CanApiSenden()
        {
            return Apis.Api != null && Apis.Api != "" && isloging;
        }
    }
}

