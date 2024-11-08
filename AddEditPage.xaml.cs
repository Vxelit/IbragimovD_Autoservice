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

namespace IbragimovD_Autoservice
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {

        private Service _currentService = new Service();

        public AddEditPage(Service SelectedService)
        {
            InitializeComponent();

            if (SelectedService != null)
            {
                _currentService = SelectedService;
            }

            DataContext = _currentService;
            _currentService.DiscountInt = 0;

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentService.Title))
                errors.AppendLine("Укажите название услуги");

            if (string.IsNullOrWhiteSpace(_currentService.Cost.ToString()) || _currentService.Cost == 0) //changed
                errors.AppendLine("Укажите стоимость услуги");

            if (string.IsNullOrWhiteSpace(_currentService.DiscountInt.ToString())) //changed
                errors.AppendLine("Укажите скидку на услугу");

            if (_currentService.Duration < 1) // changed to 1 from 0
                errors.AppendLine("Длительность не может быть меньше чем 1 минута");

            if (_currentService.Duration > 240)
                errors.AppendLine("Длительность должна быть не больше 240 минут");

            if (string.IsNullOrWhiteSpace(_currentService.Duration.ToString()))
                errors.AppendLine("Укажите длительность услуги");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }



            var allServices = IbragimovD_AutoserviceEntities.GetContext().Service.ToList();
            allServices = allServices.Where(p => p.Title == _currentService.Title).ToList();

            if (allServices.Count == 0)
            {
                if (_currentService.ID == 0)
                    IbragimovD_AutoserviceEntities.GetContext().Service.Add(_currentService);
            }
            else
            {
                MessageBox.Show("Уже существует такая услуга");
            }


            if (_currentService.ID != 0)
            {
                try
                {
                    IbragimovD_AutoserviceEntities.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена");
                    Manager.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }

        }
    }
}
