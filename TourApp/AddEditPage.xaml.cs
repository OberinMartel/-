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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Hotel _currentHotel = new Hotel();
        public AddEditPage(Hotel selectedHotel)
        {
            InitializeComponent();

            if (selectedHotel != null)
                _currentHotel = selectedHotel;

            DataContext = _currentHotel;
            ComboCountries.ItemsSource = TourBDEntities.GetContext().Country.ToList();
        }

        /// <summary>
        /// Методы добавления и редактирования записи таблицы с отелями
        /// </summary>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrEmpty(_currentHotel.Name))
                errors.AppendLine("Укажите название отеля.");
            if (_currentHotel.CountOfStars < 1 || _currentHotel.CountOfStars > 5)
                errors.AppendLine("Укажите количество звёзд от 1 до 5.");
            if (_currentHotel.Country == null)
                errors.AppendLine("Выберите страну.");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentHotel.Id == 0)
                TourBDEntities.GetContext().Hotel.Add(_currentHotel);

            try
            {
                TourBDEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена!");
                Manager.MainFrame.GoBack();
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
