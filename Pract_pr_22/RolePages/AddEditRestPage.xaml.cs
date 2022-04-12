using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Microsoft.Win32;

namespace Pract_pr_22.RolePages
{
    /// <summary>
    /// Логика взаимодействия для AddEditRestPage.xaml
    /// </summary>
    public partial class AddEditRestPage : Page
    {
        private static User localUser;
        private static Ownership localOwn;

        private static bool localIsEdit;
        private static int counter;

        private int startHours = 0;
        private int endHours = 0;
        private int startMinutes = 0;
        private int endMinutes = 0;

        readonly SolidColorBrush ok = new SolidColorBrush(Color.FromRgb(25, 169, 100));
        readonly SolidColorBrush notOk = new SolidColorBrush(Color.FromRgb(128, 128, 128));

        public AddEditRestPage(User user, Ownership own, bool isEdit)
        {
            InitializeComponent();

            localUser = user;
            localOwn = own;
            localIsEdit = isEdit;

            DataContext = own;

            counter = 0;

            AVGPriceSl.Value = 5000;

            KitchenList.ItemsSource = MainWindow.ent.KitchenType.ToList();

            if (localIsEdit)
            {
                if ((bool)localOwn.Restourant.HaveTeracce)
                {
                    YesRb.IsChecked = true;
                }
                else
                {
                    NoRb.IsChecked= true;
                }

                SaveBtn.Content = "СОХРАНИТЬ";
            }
            else
            {
                SaveBtn.Content = "ОТПРАВИТЬ";
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SetChecks())
            {
                if (localIsEdit)
                {
                    localOwn.Restourant.Name = NameTb.Text;

                    localOwn.Restourant.Description = DescTb.Text;

                    localOwn.Restourant.Address = AddressTb.Text;

                    localOwn.Restourant.PlaceCount = int.Parse(CountTb.Text);

                    localOwn.Restourant.Image = ImageTb.Text;

                    localOwn.Restourant.ContactPhone = PhoneTb.Text;

                    if ((bool)YesRb.IsChecked)
                    {
                        localOwn.Restourant.HaveTeracce = true;
                    }
                    else if ((bool)NoRb.IsChecked)
                    {
                        localOwn.Restourant.HaveTeracce = false;
                    }

                    if (!string.IsNullOrEmpty(StartTb.Text) && !string.IsNullOrEmpty(EndTb.Text))
                    {
                        List<string> startList = StartTb.Text.Split(' ', ',', '.', ':', '-').ToList();
                        List<string> endList = EndTb.Text.Split(' ', ',', '.', ':', '-').ToList();

                        if (startList.Count > 1)
                        {
                            int.TryParse(startList[0], out startHours);
                            int.TryParse(startList[1], out startMinutes);
                        }
                        else if (startList.Count == 1)
                        {
                            int.TryParse(startList[0], out startHours);
                        }

                        if (endList.Count > 1)
                        {
                            int.TryParse(endList[0], out endHours);
                            int.TryParse(endList[1], out endMinutes);
                        }
                        else if (endList.Count == 1)
                        {
                            int.TryParse(startList[0], out endHours);
                        }

                        if (startHours != 0 && endHours != 0)
                        {
                            TimeSpan startTime = new TimeSpan(startHours, startMinutes, 0);
                            TimeSpan endTime = new TimeSpan(endHours, endMinutes, 0);

                            Check8.Background = ok;
                            Check8.BorderBrush = ok;

                            localOwn.Restourant.StartTime = startTime;
                            localOwn.Restourant.EndTime = endTime;
                        }
                    }
                    else
                    {
                        Check8.Background = notOk;
                        Check8.BorderBrush = notOk;
                    }

                    if (!string.IsNullOrEmpty(AVGPriceLbl.Content.ToString()) && decimal.TryParse(AVGPriceLbl.Content.ToString(), out decimal price))
                    {
                        localOwn.Restourant.AVGPrice = price;
                    }
                }
                else
                {
                    Restourant restourant = new Restourant
                    {
                        Name = NameTb.Text,

                        Description = DescTb.Text,

                        Address = AddressTb.Text,

                        PlaceCount = int.Parse(CountTb.Text),

                        Image = ImageTb.Text,

                        ContactPhone = PhoneTb.Text
                    };

                    if ((bool)YesRb.IsChecked)
                    {
                        restourant.HaveTeracce = true;
                    }
                    else if ((bool)NoRb.IsChecked)
                    {
                        restourant.HaveTeracce = false;
                    }

                    if (!string.IsNullOrEmpty(StartTb.Text) && !string.IsNullOrEmpty(EndTb.Text))
                    {
                        List<string> startList = StartTb.Text.Split(' ', ',', '.', ':', '-').ToList();
                        List<string> endList = EndTb.Text.Split(' ', ',', '.', ':', '-').ToList();

                        if (startList.Count > 1)
                        {
                            int.TryParse(startList[0], out startHours);
                            int.TryParse(startList[1], out startMinutes);
                        }
                        else if (startList.Count == 1)
                        {
                            int.TryParse(startList[0], out startHours);
                        }

                        if (endList.Count > 1)
                        {
                            int.TryParse(endList[0], out endHours);
                            int.TryParse(endList[1], out endMinutes);
                        }
                        else if (endList.Count == 1)
                        {
                            int.TryParse(startList[0], out endHours);
                        }

                        if (startHours != 0 && endHours != 0)
                        {
                            TimeSpan startTime = new TimeSpan(startHours, startMinutes, 0);
                            TimeSpan endTime = new TimeSpan(endHours, endMinutes, 0);

                            Check8.Background = ok;
                            Check8.BorderBrush = ok;

                            restourant.StartTime = startTime;
                            restourant.EndTime = endTime;
                        }
                    }
                    else
                    {
                        Check8.Background = notOk;
                        Check8.BorderBrush = notOk;
                    }

                    if (!string.IsNullOrEmpty(AVGPriceLbl.Content.ToString()) && decimal.TryParse(AVGPriceLbl.Content.ToString(), out decimal price))
                    {
                        restourant.AVGPrice = price;
                    }

                    MainWindow.ent.Restourant.Add(restourant);
                    MainWindow.ent.SaveChanges();

                    Ownership ownership = new Ownership
                    {
                        IDRest = restourant.ID,
                        IDUser = localUser.ID
                    };

                    MainWindow.ent.Ownership.Add(ownership);
                }

                MainWindow.ent.SaveChanges();
                NavigationService.Navigate(new CreatorPage(localUser));
            }
        }

        private void ImageBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog()
            {
                Filter = "*.png|*.jpg"
            };

            if (openFile.ShowDialog().GetValueOrDefault())
            {
                localOwn.Restourant.Image = new BitmapImage(new Uri(openFile.FileName)).ToString();
                ImageTb.Text = new BitmapImage(new Uri(openFile.FileName)).ToString();
            }
        }

        private bool SetChecks()
        {
            if (!string.IsNullOrEmpty(NameTb.Text))
            {
                Check1.Background = ok;
                Check1.BorderBrush = ok;
            }
            else
            {
                Check1.Background = notOk;
                Check1.BorderBrush = notOk;

                return false;
            }

            if (!string.IsNullOrEmpty(DescTb.Text))
            {
                Check2.Background = ok;
                Check2.BorderBrush = ok;
            }
            else
            {
                Check2.Background = notOk;
                Check2.BorderBrush = notOk;

                return false;
            }

            if (!string.IsNullOrEmpty(AddressTb.Text))
            {
                Check3.Background = ok;
                Check3.BorderBrush = ok;
            }
            else
            {
                Check3.Background = notOk;
                Check3.BorderBrush = notOk;

                return false;
            }

            int.TryParse(CountTb.Text, out int count);

            if (!string.IsNullOrEmpty(CountTb.Text) && count > 0)
            {
                Check4.Background = ok;
                Check4.BorderBrush = ok;
            }
            else
            {
                Check4.Background = notOk;
                Check4.BorderBrush = notOk;

                return false;
            }

            if (!string.IsNullOrEmpty(ImageTb.Text))
            {
                Check5.Background = ok;
                Check5.BorderBrush = ok;
            }
            else
            {
                Check5.Background = notOk;
                Check5.BorderBrush = notOk;

                return false;
            }

            if (!string.IsNullOrEmpty(PhoneTb.Text) && Regex.Match(PhoneTb.Text, @"^((\+7|7|8)+([0-9]){10})$").Success)
            {
                Check6.Background = ok;
                Check6.BorderBrush = ok;
            }
            else
            {
                Check6.Background = notOk;
                Check6.BorderBrush = notOk;

                return false;
            }

            if ((bool)NoRb.IsChecked)
            {
                Check7.Background = ok;
                Check7.BorderBrush = ok;
            }
            else if ((bool)YesRb.IsChecked)
            {
                Check7.Background = ok;
                Check7.BorderBrush = ok;
            }
            else
            {
                Check7.Background = notOk;
                Check7.BorderBrush = notOk;

                return false;
            }

            if (!string.IsNullOrEmpty(StartTb.Text) && !string.IsNullOrEmpty(EndTb.Text))
            {
                Check8.Background = ok;
                Check8.BorderBrush = ok;
            }
            else
            {
                Check8.Background = notOk;
                Check8.BorderBrush = notOk;

                return false;
            }

            if (!string.IsNullOrEmpty(HotWordsTb.Text))
            {
                Check10.Background = ok;
                Check10.BorderBrush = ok;
            }
            else
            {
                Check10.Background = notOk;
                Check10.BorderBrush = notOk;

                return false;
            }

            return true;
        }

        private void OpenListBtn_Click(object sender, RoutedEventArgs e)
        {
            counter++;

            if (counter % 2 == 0)
            {
                KitchenList.Visibility = Visibility.Hidden;
                KitchenList.Height = 0;
            }
            else
            {
                KitchenList.Visibility = Visibility.Visible;
                KitchenList.Height = 100;
            }
        }

        private void CurrentKitchenCb_Unchecked(object sender, RoutedEventArgs e)
        {
            if (localOwn.Restourant.ID != 0)
            {
                CheckBox cb = sender as CheckBox;

                if (cb.DataContext is KitchenType type)
                {
                    Restournat_Kitchen restournat_Kitchen = MainWindow.ent.Restournat_Kitchen.ToList().Find(c => c.IDKitchen == type.ID && c.IDRest == localOwn.Restourant.ID);

                    if (restournat_Kitchen != null)
                    {
                        MainWindow.ent.Restournat_Kitchen.Remove(restournat_Kitchen);
                        MainWindow.ent.SaveChanges();
                    }
                }
            }
        }

        private void CurrentKitchenCb_Checked(object sender, RoutedEventArgs e)
        {
            if (localOwn.Restourant.ID != 0)
            {
                CheckBox cb = sender as CheckBox;

                if (cb.DataContext is KitchenType type)
                {
                    Restournat_Kitchen restournat_Kitchen = MainWindow.ent.Restournat_Kitchen.ToList().Find(c => c.IDKitchen == type.ID && c.IDRest == localOwn.Restourant.ID);

                    if (restournat_Kitchen == null)
                    {
                        Restournat_Kitchen new_rest_Kitchen = new Restournat_Kitchen
                        {
                            IDKitchen = type.ID,
                            IDRest = localOwn.Restourant.ID
                        };

                        MainWindow.ent.Restournat_Kitchen.Add(new_rest_Kitchen);
                        MainWindow.ent.SaveChanges();
                    }
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SetChecks();
        }
    }
}
