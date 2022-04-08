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
using System.Windows.Threading;

namespace Pract_pr_22.RolePages
{
    /// <summary>
    /// Логика взаимодействия для AddBookingPage.xaml
    /// </summary>
    public partial class AddBookingPage : Page
    {
        private static User localUser;
        private static Restourant localRest;

        public AddBookingPage(User user, Restourant restourant)
        {
            InitializeComponent();

            localUser = user;
            localRest = restourant;

            MyAccBtn.Content = user.Name.Substring(0, 2).ToUpper();

            DataContext = localRest;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private async void AcceptBtn_Click(object sender, RoutedEventArgs e)
        {
            TimeTb.BorderBrush = Brushes.White;
            CountTb.BorderBrush = Brushes.White;

            if (int.TryParse(CountTb.Text, out int count) && count > 0)
            {
                int hours = 0;
                int minutes = 0;

                List<string> list = TimeTb.Text.Split(' ', ',', '.', ':', '-').ToList();

                if (list.Count > 1)
                {
                    int.TryParse(list[0], out hours);
                    int.TryParse(list[1], out minutes);
                }
                else if (list.Count == 1)
                {
                    int.TryParse(list[0], out hours);
                }

                if (hours != 0)
                {
                    DateTime bookingDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hours, minutes, 0);

                    Booking booking = new Booking
                    {
                        IDUser = localUser.ID,
                        IDRest = localRest.ID,
                        CountPeople = count,
                        DateTime = bookingDate
                    };

                    MainWindow.ent.Booking.Add(booking);
                    MainWindow.ent.SaveChanges();

                    AcceptBtn.Visibility = Visibility.Hidden;
                    DoneLbl.Visibility = Visibility.Visible;

                    await Task.Delay(1500);

                    NavigationService.Navigate(new MyBookingPage(localUser));

                }
            }
            else
            {
                TimeTb.BorderBrush = Brushes.Red;
                CountTb.BorderBrush = Brushes.Red;
            }
        }
    }
}
