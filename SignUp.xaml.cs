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
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Page
    {
        private readonly ProjectPrn212Context _context = new ProjectPrn212Context();
        public SignUp()
        {
            InitializeComponent();
            loadClass();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SignIn());
        }

        private void cbClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbClass.SelectedItem != null)
            {
                cbStudent.Visibility = Visibility.Visible;
                int selectedClass = (int)cbClass.SelectedValue;
                var student = _context.Students.Where(st => st.ClassId == selectedClass).ToList();
                cbStudent.ItemsSource = student;
                cbStudent.DisplayMemberPath = "Name";
                cbStudent.SelectedValuePath = "Id";
            }
            else
            {
                cbStudent.Visibility = Visibility.Hidden;
            }
        }

        private void loadClass()
        {
            var c = _context.Classes.ToList();
            cbClass.ItemsSource = c;
            cbClass.DisplayMemberPath = "Name";
            cbClass.SelectedValuePath = "Id";
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();
            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string mobile = txtMobile.Text.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(mobile))
            {
                MessageBox.Show("Please fill in all fields!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if(cbClass.SelectedValue == null)
            {
                MessageBox.Show("Please choose a class!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (cbStudent.SelectedValue == null)
            {
                MessageBox.Show("Please choose a student!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Invalid email format!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!IsValidPhoneNumber(mobile))
            {
                MessageBox.Show("Invalid mobile number!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool isUsernameExist = _context.Accounts.Any(a => a.Username == username);
            bool isEmailExist = _context.Parents.Any(a => a.Email == email);
            bool isMobileExist = _context.Parents.Any(a => a.Mobile == mobile);
            if (isUsernameExist || isEmailExist)
            {
                MessageBox.Show("Username or email already exists!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (isMobileExist)
            {
                MessageBox.Show("This phone number already exists!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var newAccount = new Account
            {
                Username = username,
                Password = password, 
                Role = "User"
            };
            _context.Accounts.Add(newAccount);
            _context.SaveChanges();

            var newParent = new Parent
            {
                Name = name,
                Email = email,
                Mobile = mobile,
                AccountId = newAccount.Id,
                Students = new List<Student>()
            };
            _context.Parents.Add(newParent);
            _context.SaveChanges();

            var student = _context.Students.FirstOrDefault(s => s.Id == (int)cbStudent.SelectedValue);
            if (student != null && student.ParentId == null)
            {
                newParent.Students.Add(student);
                student.ParentId = newParent.Id;
                _context.Students.Update(student);
                _context.SaveChanges();
            }
            else if (student == null)
            {
                MessageBox.Show("Student not found!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            MessageBox.Show("Sign up successfully!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.Navigate(new SignIn());
        }


        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return phoneNumber.All(char.IsDigit) && phoneNumber.Length >= 10;
        }

    }
}
