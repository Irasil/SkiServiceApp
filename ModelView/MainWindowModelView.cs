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


        public Registrationen _regi = new Registrationen();
        public User _user  = new User();
        public LoginView login { get; set; }
        public Anmelden _anmelden  = new Anmelden();





        public Task<List<Registrationen>> getTask { get; set; }
        public List<Registrationen> registrations { get; set; } = new List<Registrationen>();
        public ObservableCollection<Registrationen> Registrationens { get; set; }

        public MainWindowModelView()
        {
            
            _regi.Service = "Kleiner Service";
            _regi.Priority = "Express";
            Anmeld.status = "Anmelden";
            Registrationens = new ObservableCollection<Registrationen>();
            
            CmdAktu = new RelayCommand(param => Aktu());
            CmdNeu = new RelayCommand(param => Neu());
            CmdNeuEr = new RelayCommand(param => NeuEr());
            CmdAendern = new RelayCommand(param => Aendern(_regi));
            CmdLoeschen = new RelayCommand(param => Loeschen(_regi));
            CmdAendernSpeicher = new RelayCommand(param => AendernSpeicher(_regi));
            CmdLoeschenSpeicher = new RelayCommand(param => LoeschenSpeicher());
            CmdAnmelden = new RelayCommand(param => Anmelden());
            CmdAnmeldenSenden = new RelayCommand(param => AnmeldenSenden());
            CmdSuche = new RelayCommand(param => Suche());

        }

    public Registrationen reg
        {
            get { return _regi; }
            set
            {
                _regi = value;
                SetProperty<Registrationen>(ref _regi, value);
                OnPropertyChanged(nameof(_regi));
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
            get { return _anmelden; }
            set
            {
                _anmelden = value;
                SetProperty<Anmelden>(ref _anmelden, value);
                OnPropertyChanged(nameof(_anmelden));
            }
        }

        private object content;
        public object Content
        {
            get { return content; }
            set
            {
                content = value;
                OnPropertyChanged(nameof(Content));
            }
        }

        public async void Aktu()
        {
            _regi = new Registrationen();
            await (getTask = Database.Database.Get());
            registrations = getTask.Result.ToList();
            
            AktuView aktu = new AktuView();
            Registrationens = new ObservableCollection<Registrationen>(registrations);
            Content = aktu;
            
        }

        public void Neu()
        {
            _regi = new Registrationen();
            _regi.Service = "Kleiner Service";
            _regi.Priority = "Express";
            NeuView neu = new NeuView();
            Content = neu;
        }

        private void NeuEr()
        {
            string lol = string.Empty;
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

        }

        private void Aendern(Registrationen reg)
        {
            if (_regi.Name != null)
            {
                AendernView neu = new AendernView();
                Content = neu;
            }
            else
            {
                Aktu();
            }
        }

        private void Loeschen(Registrationen reg)
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

        public async void AendernSpeicher(Registrationen reg)
        {
            Database.Database.Put(reg);
            await Task.Delay(100);
            Aktu();
        }

        private async void LoeschenSpeicher()
        {
            Database.Database.Delete(_regi);
            await Task.Delay(100);
            Aktu();
        }

        private async void Anmelden()
        {
            User.Name = string.Empty;
            login = new LoginView();
            Content = login;
        }
       
        public async void AnmeldenSenden()
        {
            
            _user.password = login.pwb.Password;
            Task<bool> suc;
            await (suc = Database.Database.Login(_user));
            if (suc != null)
            {
                bool success = suc.Result;
                if(success)
                {
                    ErfolgreichView erfolgreich = new ErfolgreichView();
                    Content = erfolgreich;
                    
                    _anmelden.status = "abmelden";
                    
                }
                else
                {

                }
            }
            else
            {
                Aktu();
            }
            User.Name = string.Empty;
        }

        private async void Suche()
        {
            _regi = new Registrationen();
            await (getTask = Database.Database.Get());
            registrations = getTask.Result.ToList();
            //await Task.Delay(100);
            List<Registrationen> Registries = new List<Registrationen>();
            Registries = registrations.ToList();
            
            if(_user.Name != null) {
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
            //ErfolgreichView erfolgreich = new ErfolgreichView();
            //Content = erfolgreich;
        }
    }
}

