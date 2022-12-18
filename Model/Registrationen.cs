using SkiServiceApp.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SkiServiceApp.Model
{
    /// <summary>
    /// Klasse für die Registrationen
    /// </summary>
    public class Registrationen : ViewModelBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Pickup_Date { get; set; }

        public string Service { get; set; }
        public string Priority { get; set; }

        public string Status { get; set; }
    }
    /// <summary>
    /// Klasse für die die Listen der Registrationen
    /// </summary>
    public class Keywordlist
    {
        public List<Registrationen> Keywords { get; set; }
    }
}
