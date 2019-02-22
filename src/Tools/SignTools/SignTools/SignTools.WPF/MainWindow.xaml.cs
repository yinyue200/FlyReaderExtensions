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
using System.Security.Cryptography;

namespace SignTools.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RSACryptoServiceProvider rSA;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            rSA = new RSACryptoServiceProvider();
            rSA.KeySize = 4096;
            rSA.ImportCspBlob(tb1.Text.Split(',').Select(a => byte.Parse(a.Substring(2),System.Globalization.NumberStyles.AllowHexSpecifier)).ToArray());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            rSA = new RSACryptoServiceProvider();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            tb1.Text = string.Join(",", rSA.ExportCspBlob(true).Select(a => "0x" + a.ToString("X2")));
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            tb1.Text = string.Join(",", rSA.ExportCspBlob(false).Select(a => "0x" + a.ToString("X2")));
        }
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            using (var sha512 = SHA512.Create())
            {
                var hash=sha512.ComputeHash(System.Text.Encoding.UTF8.GetBytes(tb1.Text));
                var data = rSA.Encrypt(hash, false);
                tb1.Text = Convert.ToBase64String(data);
            }
        }
    }
}
