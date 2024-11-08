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
    /// Логика взаимодействия для SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Page
    {
        private Service _currentService = new Service();
        public SignUpPage(Service SelectedService)
        {
            InitializeComponent();
            if (SelectedService != null)
                this._currentService = SelectedService;

            DataContext = _currentService;

            var _currentClient = IbragimovD_AutoserviceEntities.GetContext().Client.ToList();

            ComboClient.ItemsSource = _currentClient;
        }

        private ClientService _currentClientService = new ClientService();
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (ComboClient.SelectedItem == null)
                errors.AppendLine("Укажите ФИО клиента");

            if (StartDate.Text == "")
                errors.AppendLine("Укажите дату услуги");

            if (TBStart.Text == "") // проверка  написана самостоятельно
                errors.AppendLine("Укажите время начала услуги");
            else
            {
                string s = TBStart.Text;

                string[] start = s.Split(new char[] { ':' });

                if (start.Length == 1)
                {
                    errors.AppendLine("Строка время начала услуги введена некорректно");
                } else
                {
                    try
                    {
                        int startHour = Convert.ToInt32(start[0].ToString());
                        int startMin = Convert.ToInt32(start[1].ToString());

                        if (startHour > 24 || startHour < 0)
                        {
                            errors.AppendLine("Неправильно введен час начала услуги");
                        }
                        if (startMin > 59 || startMin < 0)
                        {
                            errors.AppendLine("Неправильно введены минуты начала услуги");
                        }
                    }
                    catch
                    {
                        errors.AppendLine("Строка время начала услуги введена некорректно");
                    }
                }

            }


            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            _currentClientService.ClientID = ComboClient.SelectedIndex + 1;
            _currentClientService.ServiceID = _currentService.ID;
            _currentClientService.StartTime = Convert.ToDateTime(StartDate.Text + " " + TBStart.Text);

            
            if (_currentClientService.ID == 0)
                IbragimovD_AutoserviceEntities.GetContext().ClientService.Add(_currentClientService);

            try
            {
                IbragimovD_AutoserviceEntities.GetContext().SaveChanges();
                MessageBox.Show("информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void TBStart_TextChanged(object sender, TextChangedEventArgs e)
        {
            string s = TBStart.Text;

            if (s.Length < 5 || !s.Contains(':')) // changed from 3 to 5
                TBEnd.Text = "";
            else
            {
                string[] start = s.Split(new char[] { ':' });
                int startHour = Convert.ToInt32(start[0].ToString()) * 60;
                int startMin = Convert.ToInt32(start[1].ToString());

                int sum = startHour + startMin + _currentService.Duration;

                int EndHour = (sum / 60) % 24;
                int EndMin = sum % 60;


                s = EndHour.ToString() + ":" + EndMin.ToString();
                TBEnd.Text = s;
            }
        }
    }
}
