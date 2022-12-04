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

namespace SkiServiceApp.ModelView
{
    public class MainWindowModelView : ViewModelBase
    {
        public MainWindow main;
        
        public RelayCommand CmdAktu { get; set; }
        public RelayCommand CmdNeu { get; set; }

        public Task<List<Registrationen>> lol { get; set; }
        public List<Registrationen> registrations { get; set; }
        public ObservableCollection<Registrationen> Registrationens { get; set; }
        
        public MainWindowModelView()
        {
           
            string hey = "hexy";
            
            

            Registrationens = new ObservableCollection<Registrationen>();
            

             
            CmdAktu = new RelayCommand(param => Aktu() );
            CmdNeu = new RelayCommand(param => Neu());
        }

        private object content;
        public object Content
        {
            get { return content; }
            set
            {
                content = value;
                OnPropertyChanged (nameof( Content));
            }
        }

        private object content1;
        public object Content1
        {
            get { return content1; }
            set
            {
                content1 = value;
                OnPropertyChanged(nameof(Content1));
            }
        }

        public async void Aktu()
        {
            await (lol = Database.Database.Get());
            registrations = lol.Result.ToList();
            AktuView aktu = new AktuView();
            //main = new MainWindow();
            Registrationens = new ObservableCollection<Registrationen>(registrations);
            Content = aktu;
            
        }

        public void Neu()
        {
            NeuView neu = new NeuView();
            Content = neu;
        }
    }
}
