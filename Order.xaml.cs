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
    /// Interaction logic for Order.xaml
    /// </summary>
    public partial class Order : Page
    {
        private readonly ProjectPrn212Context _context = new ProjectPrn212Context();
        private int parent;
        public Order(int parent)
        {
            InitializeComponent();
            this.parent = parent;
            loadMealtime();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SignIn());
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e) 
        {
            NavigationService.Navigate(new ChangePassword(parent));
        }

        private void loadMealtime()
        {
            cbMealTime.Items.Add("Breakfast");
            cbMealTime.Items.Add("Lunch");
            cbMealTime.Items.Add("Afternoon");
        }

        private Project_PRN212.Models.Menu? currentMenu = null;

        private void loadDishesForMenu(int menuId)
        {
            var menu = _context.Menus
                .Include(m => m.Dishes)
                .FirstOrDefault(m => m.Id == menuId);

            if (menu != null)
            {
                dgMenu.ItemsSource = menu.Dishes.ToList();
            }
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            currentMenu = null;
            dpDate.SelectedDate = null;
            cbMealTime.SelectedIndex = -1;
            dgMenu.ItemsSource = null;
        }

        private void View_Click(object sender, EventArgs e)
        {
            if (dpDate.SelectedDate == null || cbMealTime.SelectedItem == null)
            {
                MessageBox.Show("Please select a date and meal time to view a menu.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedDate = dpDate.SelectedDate.Value;
            var selectedMealTime = cbMealTime.SelectedItem.ToString();

            // Lấy menu theo ngày và meal time
            var menu = _context.Menus.FirstOrDefault(m => m.Date == DateOnly.FromDateTime(selectedDate) && m.MealTime == selectedMealTime);
            if (menu == null)
            {
                MessageBox.Show("No menu found for the selected date and meal time.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            currentMenu = menu;
            loadDishesForMenu(menu.Id);
        }

        private void Add_Click(object sender, EventArgs e)
        {
            if (dgMenu.SelectedItem == null)
            {
                MessageBox.Show("Please select a dish to add.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var pt = _context.Parents.Include(p => p.Students).FirstOrDefault(p => p.Id == parent);
            if (pt == null || !pt.Students.Any())
            {
                MessageBox.Show("No students found for this parent.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var registration = new Registration
            {
                RegistrationDate = DateOnly.FromDateTime(DateTime.Now),
                Status = "Pending",
                StudentId = pt.Students.First().Id,  
                MenuId = currentMenu.Id
            };

            _context.Registrations.Add(registration);
            _context.SaveChanges();
            MessageBox.Show("Dish added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            LoadRegistrations();
        }

        private void LoadRegistrations()
        {
            var registrations = _context.Registrations
                .Include(r => r.Menu)
                .ThenInclude(m => m.Dishes) 
                .Where(r => r.StudentId == parent)
                .Select(r => r.Menu.Dishes.Select(d => new
                {
                    Name = d.Name,
                    Price = d.Price
                }))
                .ToList();

            dgRegistration.ItemsSource = registrations;
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (dgMenu.SelectedItem == null)
            {
                MessageBox.Show("Please select a dish to remove.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var registration = _context.Registrations
                .FirstOrDefault(r => r.StudentId == parent && r.MenuId == currentMenu.Id);

            if (registration != null)
            {
                _context.Registrations.Remove(registration);
                _context.SaveChanges();
                LoadRegistrations();
                MessageBox.Show("Registration removed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("No registration found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
