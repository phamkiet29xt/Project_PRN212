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
using Microsoft.EntityFrameworkCore;
using Project_PRN212.Models;

namespace Project_PRN212
{
    /// <summary>
    /// Interaction logic for ManageStaff.xaml
    /// </summary>
    public partial class ManageStaff : Page
    {
        private readonly ProjectPrn212Context _context = new ProjectPrn212Context();
        public ManageStaff()
        {
            InitializeComponent();
            loadData();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SignIn());
        }

        private void Student_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ManageStudent());
        }

        private void UserAccount_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ParentAccount());
        }

        private void Dish_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ManageDish());
        }

        private void loadData()
        {
            var staffList = _context.Staff.Select(s => new
            {
                s.Id,
                s.Name,
                s.Mobile,
                s.Email,
                Username = _context.Accounts.Where(a => a.Id == s.AccountId).Select(a => a.Username).FirstOrDefault()
            }).ToList();

            dgStaff.ItemsSource = staffList;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtMobile.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtUsername.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                MessageBox.Show("Please fill all fields!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newAccount = new Account
            {
                Username = txtUsername.Text,
                Password = txtPassword.Password,
                Role = "Staff"
            };

            _context.Accounts.Add(newAccount);
            _context.SaveChanges();

            var newStaff = new Staff
            {
                Name = txtName.Text,
                Mobile = txtMobile.Text,
                Email = txtEmail.Text,
                AccountId = newAccount.Id
            };

            _context.Staff.Add(newStaff);
            _context.SaveChanges();

            MessageBox.Show("Add a staff successfully!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            loadData();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (dgStaff.SelectedItem is null)
            {
                MessageBox.Show("Please choose a staff to update!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            dynamic selectedStaff = dgStaff.SelectedItem;
            var staff = _context.Staff.Find(selectedStaff.Id);
            if (staff != null)
            {
                staff.Name = txtName.Text;
                staff.Mobile = txtMobile.Text;
                staff.Email = txtEmail.Text;

                if (staff.Account != null)
                {
                    staff.Account.Username = txtUsername.Text;
                    staff.Account.Password = txtPassword.Password;
                }

                _context.SaveChanges();
                MessageBox.Show("Update successfully!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                loadData();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgStaff.SelectedItem is null)
            {
                MessageBox.Show("Please choose a staff to delete!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            dynamic selectedStaff = dgStaff.SelectedItem;
            var staff = _context.Staff.Find(selectedStaff.Id);
            if (staff != null)
            {
                if (staff.Account != null)
                {
                    _context.Accounts.Remove(staff.Account);
                }

                _context.Staff.Remove(staff);
                _context.SaveChanges();
                MessageBox.Show("Delete successfully!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                loadData();
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtSearch.Text.Trim().ToLower();

            var staffList = _context.Staff
                .Where(s => s.Name.ToLower().Contains(searchText))
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.Mobile,
                    s.Email,
                    Username = s.Account.Username 
                })
                .ToList();

            dgStaff.ItemsSource = staffList;

            if (staffList.Count == 0)
            {
                MessageBox.Show("The staff not found!", "Warning", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            txtName.Text = "";
            txtMobile.Text = "";
            txtEmail.Text = "";
            txtUsername.Text ="";
            txtPassword.Password = "";
            txtSearch.Text = "";
        }
        private void dgStaff_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgStaff.SelectedItem is null) return;

            dynamic staff = dgStaff.SelectedItem;
            txtName.Text = staff.Name;
            txtMobile.Text = staff.Mobile;
            txtEmail.Text = staff.Email;
        }


    }
}
