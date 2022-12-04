using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SkiServiceApp.Model
{
    public class Registrationen
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
    public class Keywordlist
    {
        public List<Registrationen> Keywords { get; set; }
    }
}
