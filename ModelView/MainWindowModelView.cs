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
        public RelayCommand CmdAendernSpeicher { get; set; }
        public RelayCommand CmdLoeschenSpeicher { get; set; }
        public Registrationen selectedReg = new Registrationen();
        public Registrationen regi = new Registrationen();


        public Task<List<Registrationen>> getTask { get; set; }
        public List<Registrationen> registrations { get; set; }
        public ObservableCollection<Registrationen> Registrationens { get; set; }

        public MainWindowModelView()
        {
            regi.Service = "Kleiner Service";
            regi.Priority = "Express";
            Registrationens = new ObservableCollection<Registrationen>();


            
            CmdAktu = new RelayCommand(param => Aktu());
            CmdNeu = new RelayCommand(param => Neu());
            CmdNeuEr = new RelayCommand(param => NeuEr());
            CmdAendern = new RelayCommand(param => Aendern(regi));
            CmdLoeschen = new RelayCommand(param => Loeschen(regi));
            CmdAendernSpeicher = new RelayCommand(param => AendernSpeicher(regi));
            CmdLoeschenSpeicher = new RelayCommand(param => LoeschenSpeicher());
        }

        private async void LoeschenSpeicher()
        {
            Database.Database.Delete(regi);
            await Task.Delay(100);
            Aktu();
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
       

        public Registrationen reg
        {
            get { return regi; }
            set
            {
                content = value;
                SetProperty<Registrationen>(ref regi, value);
                OnPropertyChanged(nameof(regi));
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

        private void Aendern(Registrationen reg)
        {
            if (regi.Name != null) 
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
            if (regi.Name != null)
            {
                LoeschenView neu = new LoeschenView();
                Content = neu;
            }
            else
            {
                Aktu();
            }
        }

        public async void Aktu()
        {
            regi = new Registrationen();
            await (getTask = Database.Database.Get());
            registrations = getTask.Result.ToList();
            AktuView aktu = new AktuView();
            Registrationens = new ObservableCollection<Registrationen>(registrations);
            Content = aktu;
            
        }

        public void Neu()
        {
            regi = new Registrationen();
            regi.Service = "Kleiner Service";
            regi.Priority = "Express";
            NeuView neu = new NeuView();
            Content = neu;
        }

        public async void AendernSpeicher(Registrationen reg)
        {
            Database.Database.Put(reg);
            await Task.Delay(100);
            Aktu();
        }
    }
}
