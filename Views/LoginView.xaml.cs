using SkiServiceApp.Model;
using SkiServiceApp.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SkiServiceApp.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        MainWindowModelView modelView = new MainWindowModelView();

        public LoginView()
        {
            InitializeComponent();           
            
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    string password = pwb.Password;
        //    modelView._user.password = password;
        //    modelView.AnmeldenSenden();
        //}
    }
}
