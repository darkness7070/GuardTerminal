using GuardTerminal.Windows;
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

namespace GuardTerminal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
        }
        public async Task<bool> Auth(string code)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response;
            try
            {
                response = await client.GetAsync($"http://localhost:5220/general/auth?code={code}");
            }
            catch
            {
                MessageBox.Show("Сервер не отвечает");
                return false;
            }
            var jsonObj = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
                return true;
            MessageBox.Show("Неверный логин");
            return false;
        }

        private async void btnAuth_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(boxCode.Text)) return;
            btnAuth.IsEnabled = false;

            if (await Auth(boxCode.Text))
            {
                MainWindow newWin = new();
                Close();
                newWin.ShowDialog();
            }
            else
            {
                btnAuth.IsEnabled = true;
            }
        }
    }
}
