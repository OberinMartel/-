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
    /// Логика взаимодействия для TourPage.xaml
    /// </summary>
    public partial class TourPage : Page
    {
        public TourPage()
        {
            InitializeComponent();

            var allTypes = TourBDEntities.GetContext().Type.ToList();
            allTypes.Insert(0, new Type
            {
                Name = "Все типы"
            });
            ComboType.ItemsSource = allTypes;

            CheckActual.IsChecked = true;
            ComboType.SelectedIndex = 0;

            UpdateTours();
        }

        /// <summary>
        /// метод обновления списка туров с учетом параметров сортировки
        /// </summary>
        private void UpdateTours()
        {
            var currentTours = TourBDEntities.GetContext().Tour.ToList();

            if(ComboType.SelectedIndex > 0)
                currentTours = currentTours.Where(p => p.Type.Contains(ComboType.SelectedItem as Type)).ToList();

            currentTours = currentTours.Where(p => p.Name.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

            if (CheckActual.IsChecked.Value)
                currentTours = currentTours.Where(p => p.IsActual).ToList();

            if (CheckOrderBy.IsChecked.Value)
                currentTours = currentTours.OrderBy(p => p.Price).ToList();
            else
                currentTours = currentTours.OrderByDescending(u => u.Price).ToList();

            LViewTour.ItemsSource = currentTours;

            decimal globalCost = 0;
            foreach(Tour t in currentTours)
            {
                globalCost += t.TicketCount * t.Price;
            }
            TBlockCost.Text = $"Общая стоимсоть туров: {globalCost:N2} руб.";
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTours();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateTours();
        }

        private void CheckActual_Checked(object sender, RoutedEventArgs e)
        {
            UpdateTours();
        }

        private void CheckOrderBy_Checked(object sender, RoutedEventArgs e)
        {
            UpdateTours();
        }
    }
}
