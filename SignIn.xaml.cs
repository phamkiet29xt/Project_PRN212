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
using Project_PRN212.Models;

namespace Project_PRN212
{
    /// <summary>
    /// Interaction logic for SignIn.xaml
    /// </summary>
    public partial class SignIn : Page
    {
        private readonly ProjectPrn212Context _context = new ProjectPrn212Context();
        public SignIn()
        {
            InitializeComponent();
        }
        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SignUp());
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();
            if(string.IsNullOrEmpty(username)  || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill in your username and password!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var user = _context.Accounts.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user == null)
            {
                MessageBox.Show("Your username or password is wrong!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            MessageBox.Show("Login successfully!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            switch (user.Role)
            {
                case "Admin":
                    NavigationService.Navigate(new ManageStudent());
                    break;
                case "Staff":
                    var staff = _context.Staff.FirstOrDefault(s => s.AccountId == user.Id);
                    NavigationService.Navigate(new DishList(staff.Id));
                    break;
                case "User":
                    var parent = _context.Parents.FirstOrDefault(p => p.AccountId == user.Id);
                    NavigationService.Navigate(new Order(parent.Id));
                    break;
            }
        }
    }
}
