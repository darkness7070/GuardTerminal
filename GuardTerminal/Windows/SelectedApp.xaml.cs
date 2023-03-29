using GuardTerminal.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GuardTerminal.Windows
{
    /// <summary>
    /// Логика взаимодействия для SelectedApp.xaml
    /// </summary>
    public partial class SelectedApp : Window
    {
        int id;
        public SelectedApp(int id)
        {
            this.id = id;
            InitializeComponent();
            cmbTime.ItemsSource = new List<string>() 
            {
                "08:00",
                "10:00",
                "12:00",
                "14:00",
                "16:00",
                "18:00"
            };
            GetData(id);
        }
        private bool VerifyData(List<Models.Visitor> visitors)
        {
            foreach(var item in visitors)
            {
                if (item.IsBlacklist)
                    return true;
            }
            return false;
        }
        private async void GetData(int id)
        {
            HttpClient client = new();
            HttpResponseMessage response;
            try
            {
                response = await client.GetAsync($"http://localhost:5220/general/app?id={id}");
            }
            catch
            {
                MessageBox.Show("Сервер не отвечает");
                return;
            }
            var jsonObj = await response.Content.ReadAsStringAsync();
            var item = JsonConvert.DeserializeObject<AppInfo>(jsonObj);
            var application = item.Application;
            txtFullname.Text = application.IdWorkerNavigation.Name;
            txtName.Text += application.Id;
            txtPurpose.Text = application.IdPurposeNavigation.Name;
            txtStatus.Text = application.IdStatusNavigation.Name;
            txtSubdivision.Text = application.IdWorkerNavigation.IdSubdivisionNavigation.Name;
            txtType.Text = application.IsSingleStr;
            txtValidaty.Text = application.ValidatyStr;

            dgVisitors.ItemsSource = item.Visitors;

            if (VerifyData(item.Visitors))
            {
                datepicker.IsEnabled = false;
                cmbTime.IsEnabled = false;
                btnAccept.IsEnabled = false;
                btnReject.IsEnabled = false;
                if (await PutStatus(id, 2))
                {
                    MessageBox.Show("Заявка отклонена Системой");
                }
                else
                {
                    MessageBox.Show("Системе не удалось отправить запрос");
                }

            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void Accept_Click(object sender, RoutedEventArgs e)
        {
            string time = (string)cmbTime.SelectedValue;
            if (!datepicker.SelectedDate.HasValue || String.IsNullOrWhiteSpace(time))
            {
                MessageBox.Show("Укажите дату и время");
                return;
            }
            btnAccept.IsEnabled = false;

            double hours = Convert.ToDouble(time.Split(':')[0]);
            double minutes = Convert.ToDouble(time.Split(':')[1]);

            var date = datepicker.SelectedDate.Value.AddHours(hours).AddMinutes(minutes);

            HttpClient client = new();
            HttpResponseMessage response;
            try
            {
                response = await client.PostAsJsonAsync("http://localhost:5220/general/putarrivaltime", new RequestArrival(id, date));
            }
            catch
            {
                MessageBox.Show("Сервер не отвечает");
                return;
            }

            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("Ошибка сервера");
                return;
            }

            if(await PutStatus(id, 3))
            {
                btnAccept.IsEnabled = true;
                
                Close();
            }
            else
            {
                btnAccept.IsEnabled = true;
                MessageBox.Show("Не успешно");
            }

        }

        private async void Reject_Click(object sender, RoutedEventArgs e)
        {
            btnReject.IsEnabled = false;
            if(await PutStatus(id, 1))
            {
                btnReject.IsEnabled = true;
                Close();
            }
            else
            {
                btnReject.IsEnabled = true;
                MessageBox.Show("Не успешно");
            }
        }
        private async Task<bool> PutStatus(int id,int status)
        {
            HttpClient client = new();
            HttpResponseMessage response;
            try
            {
                response = await client.GetAsync($"http://localhost:5220/general/putstatus?app={id}&status={status}");
            }
            catch
            {
                return false;
            }
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        private void btnPassport_Click(object sender, RoutedEventArgs e)
        {
            //ПДФ
        }
    }
}
