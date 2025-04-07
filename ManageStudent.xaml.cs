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
    /// Interaction logic for ManageStudent.xaml
    /// </summary>
    public partial class ManageStudent : Page
    {
        private readonly ProjectPrn212Context _context = new ProjectPrn212Context();
        public ManageStudent()
        {
            InitializeComponent();
            loadData();
            loadClass();
        }

        private void loadData()
        {
            var student = _context.Students.Select(x => new 
            { 
                x.Id, 
                x.Name, 
                x.Birthdate,
                x.Gender,
                Class = _context.Classes.Where(c => c.Id == x.ClassId).Select(c => c.Name).FirstOrDefault(),
                Parent = _context.Parents.Where(p => p.Id == x.ParentId).Select(p => p.Name).FirstOrDefault(),
            }).ToList();

            dgStudentList.ItemsSource = student;
        }

        private void loadClass()
        {
            var c = _context.Classes.ToList();
            cbClass.ItemsSource = c;
            cbClass.DisplayMemberPath = "Name";
            cbClass.SelectedValuePath = "Id";
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || dpBirthdate.SelectedDate == null || cbClass.SelectedItem == null)
            {
                MessageBox.Show("Please fill all fields!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newStudent = new Student
            {
                Name = txtName.Text,
                Birthdate = dpBirthdate.SelectedDate.Value,
                Gender = rbMale.IsChecked == true, 
                ClassId = ((Class)cbClass.SelectedItem).Id,
                ParentId = null
            };

            _context.Students.Add(newStudent);
            _context.SaveChanges();
            loadData();
            MessageBox.Show("Add student successfully!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (dgStudentList.SelectedItem is null)
            {
                MessageBox.Show("Please choose a student to update!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            dynamic selectedStudent = dgStudentList.SelectedItem;
            var student = _context.Students.Find(selectedStudent.Id);
            if (student != null)
            {
                student.Name = txtName.Text;
                student.Birthdate = dpBirthdate.SelectedDate.Value;
                student.Gender = rbMale.IsChecked == true;
                student.ClassId = ((Class)cbClass.SelectedItem).Id;
                student.ParentId = null;

                _context.SaveChanges();
                loadData();
                MessageBox.Show("Update successfully!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            txtName.Text = "";
            dpBirthdate.Text = null;
            rbFemale.IsChecked = false;
            rbMale.IsChecked = false;
            cbClass.SelectedIndex = -1;
            txtParent.Text = "";
            loadData();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgStudentList.SelectedItem is null)
            {
                MessageBox.Show("Please choose a student to delete!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            dynamic selectedStudent = dgStudentList.SelectedItem;
            var student = _context.Students.Find(selectedStudent.Id);
            if (student != null)
            {
                if (MessageBox.Show("Are you sure?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    _context.Students.Remove(student);
                    _context.SaveChanges();
                    loadData();
                    MessageBox.Show("Delete successfully!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            var keyword = txtSearch.Text.Trim().ToLower();
            var filteredStudents = _context.Students
                .Where(s => s.Name.ToLower().Contains(keyword))
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.Birthdate,
                    s.Gender,
                    Class = _context.Classes.Where(c => c.Id == s.ClassId).Select(c => c.Name).FirstOrDefault(),
                    Parent = _context.Parents.Where(p => p.Id == s.ParentId).Select(p => p.Name).FirstOrDefault(),
                })
                .ToList();

            dgStudentList.ItemsSource = filteredStudents;
        }

        private void dgStudentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgStudentList.SelectedItem is null) return;

            dynamic student = dgStudentList.SelectedItem;
            txtName.Text = student.Name;
            txtParent.Text = student.Parent;
            dpBirthdate.SelectedDate = student.Birthdate;
            rbMale.IsChecked = student.Gender;
            rbFemale.IsChecked = !student.Gender;
            cbClass.Text = student.Class;
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

        private void Dish_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ManageDish());
        }
    }
}
