using System;
using System.Threading;
using System.Diagnostics;
using System.Net;
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

namespace Shadowsocks_Plus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            string server = textBox.Text;
            string sport = textBox1.Text;
            string password = passwordBox.Password;
            string lport = textBox2.Text;
            //string msg = String.Format("Server IP is {0}", server);
            string msg = String.Format("Server IP: {0}\nServer port: {1}\nPassword: {2}\nLocal port: {3}", server, sport, password, lport);
            MessageBox.Show(msg, "Proceed?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (!(System.IO.File.Exists("ssp.exe")))
            {
                MessageBox.Show("Be patient while I'm downloading necessary files...", "Please wait...", MessageBoxButton.OK, MessageBoxImage.Information);
                Thread t = new Thread(() => {
                    WebClient wc = new WebClient();
                    wc.DownloadFile("https://github.com/jm33-m0/shadowsocks-plus/releases/download/v0.1/ssp.exe", @".\ssp.exe");
                });
                t.Start();
            }
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e){}

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e){}

        private void textBox2_TextChanged(object sender, TextChangedEventArgs e){}
    }
}