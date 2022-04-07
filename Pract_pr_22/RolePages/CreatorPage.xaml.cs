using Pract_pr_22.CustomMessageWindows;
using Pract_pr_22.RegPages;
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

namespace Pract_pr_22.RolePages
{
    /// <summary>
    /// Логика взаимодействия для CreatorPage.xaml
    /// </summary>
    public partial class CreatorPage : Page
    {
        private static User localUser;

        public CreatorPage(User user)
        {
            InitializeComponent();

            localUser = user;

            WelcomTb.Text += localUser.Name;

            MyRestGrid.ItemsSource = MainWindow.ent.Ownership.Where(c => c.IDUser == localUser.ID).ToList();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            localUser = null;

            NavigationService.Navigate(new AuthPage());
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Ownership own = button.DataContext as Ownership;

            if (own != null)
            {
                NavigationService.Navigate(new AddEditRestPage(localUser, own));
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult res = CustomMessageWindow.Show("Удалить ресторан?", "УДАЛИТЬ", "НАЗАД");

            if (res == System.Windows.Forms.DialogResult.Yes)
            {
                Button button = sender as Button;
                Ownership own = button.DataContext as Ownership;

                if (MainWindow.ent.Booking.Where(c => c.IDRest == own.IDRest).ToList().Count == 0)
                {
                    MainWindow.ent.Ownership.Remove(own);
                    MainWindow.ent.SaveChanges();

                    MyRestGrid.ItemsSource = MainWindow.ent.Ownership.Where(c => c.IDUser == localUser.ID).ToList();
                }
                else
                {
                    MessageBox.Show("Невозможно удалить, на этот ресторан оформлена бронь");
                }
            }
        }
    }
}
