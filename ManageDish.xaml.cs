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

namespace Project_PRN212
{
    /// <summary>
    /// Interaction logic for ManageDish.xaml
    /// </summary>
    public partial class ManageDish : Page
    {
        public ManageDish()
        {
            InitializeComponent();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SignIn());
        }

        private void Staff_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ManageStaff());
        }

        private void UserAccount_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ParentAccount());
        }

        private void Student_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ManageStudent());
        }
    }
}
