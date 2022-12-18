using SkiServiceApp.Database;
using SkiServiceApp.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiServiceApp.Model
{
    /// <summary>
    /// Klasse für die Mitarbeiter
    /// </summary>
    public class User : ViewModelBase
    {
        public int Id { get; set; }
        public string _name;
        public string Namen
        {
            get { return _name; }
            set
            {
                _name = value;
                SetProperty<string>(ref _name, value);
                OnPropertyChanged(nameof(Namen));
            }
        }

        public string password { get; set; }
        public int Counter { get; set; }
    }
}
