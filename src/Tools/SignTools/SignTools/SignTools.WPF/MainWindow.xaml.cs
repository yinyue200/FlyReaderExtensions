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
            rSA.ImportCspBlob(tb1.Text.Split(',').Select(a => byte.Parse(a.Substring(2),System.Globalization.NumberStyles.AllowHexSpecifier)).ToArray());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            rSA = new RSACryptoServiceProvider(4096);
            //rSA.KeySize = 4096;
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
            var DATA = System.Text.Encoding.UTF8.GetBytes(tb1.Text);
            var data = rSA.SignData(DATA, HashAlgorithmName.SHA512,RSASignaturePadding.Pkcs1);
            tb1.Text = Convert.ToBase64String(data);

            rSA.VerifyData(DATA, data, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
        }
    }
}
