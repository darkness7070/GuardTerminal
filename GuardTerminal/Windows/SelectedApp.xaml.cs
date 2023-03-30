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
            GetData(id);
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
            txtArrivalTime.Text = application.ArrivalTime.Value.ToString("G");
            List<string> time = new List<string>();
            foreach ( var t in new List<string>()
            {
                "08:00",
                "10:00",
                "12:00",
                "14:00",
                "16:00",
                "18:00"
            })
            {
                if (Convert.ToInt32(t.Split(':')[0]) > application.ArrivalTime.Value.Hour)
                    time.Add(t);
            }
            cmbLeavingTime.ItemsSource = time;
            dgVisitors.ItemsSource = item.Visitors;

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
            string time = (string)cmbLeavingTime.SelectedValue;
            if (String.IsNullOrWhiteSpace(time))
            {
                MessageBox.Show("Укажите дату и время");
                return;
            }
            btnAccept.IsEnabled = false;

            double hours = Convert.ToDouble(time.Split(':')[0]);
            double minutes = Convert.ToDouble(time.Split(':')[1]);

            var date = Convert.ToDateTime(txtArrivalTime.Text)
            .AddHours(hours)
            .AddMinutes(minutes);

            HttpClient client = new();
            HttpResponseMessage response;
            try
            {
                response = await client.GetAsync($"http://localhost:5220/general/acceptapp?id={id}");
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
            else
            {
                Close();
            }
        }

        private void btnPassport_Click(object sender, RoutedEventArgs e)
        {
            //ПДФ
        }
    }
}
