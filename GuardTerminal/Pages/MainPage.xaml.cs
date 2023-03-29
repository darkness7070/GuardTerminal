using GuardTerminal.Windows;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace GuardTerminal.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {

        List<Models.AppInfo> applications = new();
        public MainPage()
        {
            InitializeComponent();
            GetData();
        }
        public async Task GetData()
        {
            try
            {
                HttpClient client = new HttpClient();
                var response = await client.GetAsync($"http://localhost:5220/general/apps");
                var jsonObj = await response.Content.ReadAsStringAsync();
                applications = JsonConvert.DeserializeObject<List<Models.AppInfo>>(jsonObj);
            }catch(Exception ex)
            {
                return;
            }
            datagrid.ItemsSource = applications
                .Select(x=>x.Application)
                .ToList();
            cmbDate.ItemsSource = applications
                .Select(x => x.Application)
                .Select(x => x.ArrivalTimeStr)
                .Distinct();
            cmbSubdivisions.ItemsSource = applications
                .Select(x => x.Application)
                .Select(x => x.IdWorkerNavigation.IdSubdivisionNavigation.Name)
                .Distinct();
            cmbTypes.ItemsSource = applications
                .Select(x => x.Application)
                .Select(x => x.IsSingleStr)
                .Distinct();
        }

        private void cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionChanged();
        }
        private void SelectionChanged()
        {
            var types = (string)cmbTypes.SelectedValue;
            var subdivis = (string)cmbSubdivisions.SelectedValue;
            var date = (string)cmbDate.SelectedValue;
            List<Models.Application> itemsSource = new();
            if (String.IsNullOrWhiteSpace(boxSeries.Text) && String.IsNullOrWhiteSpace(boxFullname.Text))
            {
                itemsSource = applications
                    .Select(x => x.Application)
                    .ToList();
            }
            else
            {
                if (!String.IsNullOrWhiteSpace(boxSeries.Text))
                {
                    foreach (var item in applications)
                    {
                        foreach (var visitor in item.Visitors)
                        {
                            string passport = visitor.Series + visitor.Numbers;
                            if (passport.Contains(boxSeries.Text))
                                itemsSource.Add(item.Application);
                        }
                    }
                }
                if (!String.IsNullOrWhiteSpace(boxFullname.Text))
                {
                    foreach (var item in applications)
                    {
                        foreach (var visitor in item.Visitors)
                        {
                            string passport = $"{visitor.Surname} {visitor.Name} {visitor.Patronymic}";
                            if (passport.Contains(boxFullname.Text))
                                itemsSource.Add(item.Application);
                        }
                    }
                }
            }

            itemsSource = types != null ? itemsSource
                .Where(x => x.IsSingleStr.Contains(types))
                .ToList() : itemsSource;
            itemsSource = date != null ? itemsSource
                .Where(x => x.ArrivalTimeStr.Contains(date))
                .ToList() : itemsSource;
            itemsSource = subdivis != null ? itemsSource
                .Where(x => x.IdWorkerNavigation.IdSubdivisionNavigation.Name.Contains(subdivis))
                .ToList() : itemsSource;

            datagrid.ItemsSource = itemsSource;
        }
        private async void btnVerify_Click(object sender, RoutedEventArgs e)
        {
            var item = (Models.Application)datagrid.SelectedValue;
            if (item == null) return;
            btnVerify.IsEnabled = false;
            SelectedApp newWin = new(item.Id);
            newWin.ShowDialog();
            btnVerify.IsEnabled = true;
            await GetData();
            SelectionChanged();
        }

        private async void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            await GetData();
            SelectionChanged();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            cmbTypes.SelectedValue = null;
            cmbSubdivisions.SelectedValue = null;
            cmbDate.SelectedValue = null;
        }

        private void boxSeries_TextChanged(object sender, TextChangedEventArgs e)
        {
           SelectionChanged();
        }

        private void boxFullname_TextChanged(object sender, TextChangedEventArgs e)
        {
            SelectionChanged();
        }
    }
}
