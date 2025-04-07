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
    /// Interaction logic for DishList.xaml
    /// </summary>
    public partial class DishList : Page
    {
        private readonly ProjectPrn212Context _context = new ProjectPrn212Context();
        private int staff;
        public DishList(int staff)
        {
            InitializeComponent();
            this.staff = staff;
            loadData();
        }

        private void loadData()
        {
            var dish = _context.Dishes.Select(d => new
            {
                d.Id,
                d.Name,
                d.Price,
                d.Decription,
                CreatedByNavigation = _context.Staff.
                Where(s =>  s.Id == d.CreatedBy).Select(s => s.Name).FirstOrDefault(),
                d.CreatedAt
            }).ToList();
            dgDish.ItemsSource = dish;
        }


        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dgDish.SelectedItem == null)
            {
                return;
            }
            dynamic selectedDish = dgDish.SelectedItem;
            txtName.Text = selectedDish.Name;
            txtPrice.Text = selectedDish.Price.ToString();
            txtDescription.Text = selectedDish.Decription;
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SignIn());
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            txtName.Text = null;
            txtPrice.Text = null;
            txtDescription.Text = null;
            txtSearch.Text = null;
            loadData();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if(txtName.Text == null || txtPrice == null || txtDescription == null)
            {
                MessageBox.Show("Please enter all fields", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var newDish = new Dish
            {
                Name = txtName.Text,
                Decription = txtDescription.Text,
                Price = decimal.Parse(txtPrice.Text),
                CreatedAt = DateTime.Now,
                CreatedBy = staff
            };
            _context.Dishes.Add(newDish);
            _context.SaveChanges();
            MessageBox.Show("Add dish successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            loadData();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if(dgDish.SelectedItem is null)
            {
                MessageBox.Show("Please choose a dish to update", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            dynamic selectedDish = dgDish.SelectedItem;
            var dish = _context.Dishes.Find(selectedDish.Id);
            if (dish != null)
            {
                dish.Name = txtName.Text;
                dish.Decription = txtDescription.Text;
                dish.Price = decimal.Parse(txtPrice.Text);
                _context.SaveChanges();
                MessageBox.Show("Update successfully", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                loadData();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if(dgDish.SelectedItem is null)
            {
                MessageBox.Show("Please choose a dish to delete", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            dynamic selectedDish = dgDish.SelectedItem;
            var dish = _context.Dishes.Find(selectedDish.Id);
            if (dish != null) 
            {
                _context.Dishes.Remove(dish);
                _context.SaveChanges();
                MessageBox.Show("Delete successfully", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                loadData();
            }
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

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ManageMenu(staff));
        }

    }
}
