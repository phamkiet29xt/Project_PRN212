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
    /// Interaction logic for ParentAccount.xaml
    /// </summary>
    public partial class ParentAccount : Page
    {
        private readonly ProjectPrn212Context _context = new ProjectPrn212Context();
        public ParentAccount()
        {
            InitializeComponent();
            loadData();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SignIn());
        }

        private void Staff_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ManageStaff());
        }

        private void Student_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ManageStudent());
        }

        private void Dish_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ManageDish());
        }

        private  void loadData()
        {
            var parents = _context.Parents.Select(p => new
            {
                p.Id, p.Name, p.Mobile, 
                p.Email,
                Username = _context.Accounts.Where(a => a.Id == p.AccountId).Select(a => a.Username).FirstOrDefault(),
            }).ToList();
            dgParent.ItemsSource = parents;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtSearch.Text.Trim().ToLower();

            var parentList = _context.Parents
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

            dgParent.ItemsSource = parentList;
        }
    }
}
