using SkiServiceApp.Utility;
using SkiServiceApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SkiServiceApp.ModelView
{
    internal class MainWindowModelView : ViewModelBase
    {
        public MainWindow main;
        
        public RelayCommand CmdAktu { get; set; }
        public RelayCommand CmdNeu { get; set; }

        public MainWindowModelView()
        { 
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

        public void Aktu()
        {
           AktuView aktu = new AktuView();
            //main = new MainWindow();
            Content = aktu;           
        }

        public void Neu()
        {
            NeuView neu = new NeuView();
            Content = neu;
        }
    }
}
