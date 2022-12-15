using SkiServiceApp.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkiServiceApp.Model
{
    public class User : ViewModelBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string password { get; set; }
        public int Counter { get; set; }
    }
}
