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

namespace TourApp
{
    /// <summary>
    /// Логика взаимодействия для HotelsPage.xaml
    /// </summary>
    public partial class HotelsPage : Page
    {
        public HotelsPage()
        {
            InitializeComponent();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as Hotel));
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }

        /// <summary>
        /// метод удаления записи из БД с отелямя
        /// </summary>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var hotelsForRemoving = DGridHotels.SelectedItems.Cast<Hotel>().ToList();
            if (MessageBox.Show($"Вы точно хотите удалить {hotelsForRemoving.Count()} элементов?", "Внимание", 
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    TourBDEntities.GetContext().Hotel.RemoveRange(hotelsForRemoving);
                    TourBDEntities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");

                    DGridHotels.ItemsSource = TourBDEntities.GetContext().Hotel.ToList();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            TourBDEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
            DGridHotels.ItemsSource = TourBDEntities.GetContext().Hotel.ToList();
        }
    }
}
