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
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class ManageMenu : Page
    {
        private readonly ProjectPrn212Context _context = new ProjectPrn212Context();
        private int staff;
        public ManageMenu(int staff)
        {
            InitializeComponent();
            this.staff = staff;
            loadDish();
            loadMealtime();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SignIn());
        }

        private void ManageDish_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DishList(staff));
        }

        private void loadDish()
        {
            var dish = _context.Dishes.ToList();
            dgDish.ItemsSource = dish;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                MessageBox.Show("Please enter the dish to search", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            string keyword = txtSearch.Text.Trim().ToLower();
            var result = _context.Dishes.Where(d => d.Name.ToLower().Contains(keyword)).ToList();
            if (result.Any())
            {
                dgDish.ItemsSource = result;
            }
            else
            {
                MessageBox.Show("The dish not found", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
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

        private void AddDish_Click(object sender, RoutedEventArgs e)
        {
            if(dpDate.SelectedDate == null || cbMealTime.SelectedItem == null)
            {
                MessageBox.Show("Please select a date and meal time before adding a dish", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var selectedDish = (Dish)dgDish.SelectedItem;
            if (selectedDish == null) 
            {
                MessageBox.Show("Please select a dish to add", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (currentMenu == null) 
            {
                currentMenu = new Project_PRN212.Models.Menu
                {
                    Date = DateOnly.FromDateTime(dpDate.SelectedDate.Value),
                    MealTime = cbMealTime.SelectedItem.ToString()
                };
                _context.Menus.Add(currentMenu);
                _context.SaveChanges();
            }
            if (currentMenu.Dishes.Any(d => d.Id == selectedDish.Id))
            {
                MessageBox.Show("This dish is already in the menu.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            currentMenu.Dishes.Add(selectedDish);
            _context.SaveChanges();
            MessageBox.Show("Dish added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            loadDishesForMenu(currentMenu.Id);
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (currentMenu == null)
            {
                MessageBox.Show("No menu to save. Please add dishes first.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _context.SaveChanges();
            MessageBox.Show("Menu saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void DeleteDish_Click(object sender, EventArgs e) 
        {
            if (currentMenu == null)
            {
                MessageBox.Show("No menu selected.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedDish = (Dish)dgMenu.SelectedItem;
            if (selectedDish == null)
            {
                MessageBox.Show("Please select a dish to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var dishMenu = currentMenu.Dishes
                .FirstOrDefault(d => d.Id == selectedDish.Id);

            if (dishMenu != null)
            {
                currentMenu.Dishes.Remove(dishMenu);
                _context.SaveChanges();
                MessageBox.Show("Dish removed from menu.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                loadDishesForMenu(currentMenu.Id);
            }
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            currentMenu = null;
            dpDate.SelectedDate = null;
            cbMealTime.SelectedIndex = -1;
            dgMenu.ItemsSource = null;
        }
    }
}
