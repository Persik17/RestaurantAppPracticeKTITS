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
using Pract_pr_22.CustomMessageWindows;

namespace Pract_pr_22.RolePages
{
    /// <summary>
    /// Логика взаимодействия для MyBookingPage.xaml
    /// </summary>
    public partial class MyBookingPage : Page
    {
        private static User localUser;

        public MyBookingPage(User user)
        {
            InitializeComponent();

            localUser = user;

            MyAccBtn.Content = user.Name.Substring(0, 2).ToUpper();

            MyBookingList.ItemsSource = MainWindow.ent.Booking.Where(c => c.IDUser == localUser.ID).ToList();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ClientPage(localUser));
        }

        private void PhoneBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Booking booking = button.DataContext as Booking;

            MessageBox.Show($"Контактный номер\n{booking.Restourant.ContactPhone}");
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            localUser = null;

            NavigationService.Navigate(new AuthPage());
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult res = CustomMessageWindow.Show("Отменить бронирование?", "ОТМЕНИТЬ", "НАЗАД");
            if (res == System.Windows.Forms.DialogResult.Yes)
            {
                Button button = sender as Button;
                Booking booking = button.DataContext as Booking;

                if (booking != null)
                {
                    MainWindow.ent.Booking.Remove(booking);
                    MainWindow.ent.SaveChanges();

                    MyBookingList.ItemsSource = MainWindow.ent.Booking.Where(c => c.IDUser == localUser.ID).ToList();
                }
            }
        }
    }
}
