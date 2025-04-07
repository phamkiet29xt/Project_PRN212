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
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Page
    {
        private readonly ProjectPrn212Context _context = new ProjectPrn212Context();
        private int id;
        public ChangePassword(int id)
        {
            InitializeComponent();
            this.id = id;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(txtOldPassword.Password) || string.IsNullOrEmpty(txtNewPassword.Password) || string.IsNullOrEmpty(txtConfirmPassword.Password))
            {
                MessageBox.Show("Please enter all fields", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var user = _context.Parents.FirstOrDefault(u => u.Id == id);
            var acc = _context.Accounts.FirstOrDefault(a => a.Id == user.AccountId);
            if (acc.Password != txtOldPassword.Password)
            {
                MessageBox.Show("Old password is incorrect!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (txtNewPassword.Password != txtConfirmPassword.Password)
            {
                MessageBox.Show("New password and confirm password do not match!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            acc.Password = txtNewPassword.Password;
            _context.SaveChanges();
            MessageBox.Show("Password changed successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
