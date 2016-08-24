using System;
using System.Threading;
using System.Diagnostics;
using System.Net;
using Microsoft.Win32;
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
            // need to check if the proxy is already running
            Process[] pname = Process.GetProcessesByName("ssp");
            if(pname.Length != 0)
            {
                MessageBox.Show("Local proxy is already running!", "Warning: ", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
                //Application.Current.Shutdown();
            }
            string server = textBox.Text;
            //int sport = Int32.Parse(textBox1.Text);
            string sport = textBox1.Text;
            string password = passwordBox.Password;
            //int lport = Int32.Parse(textBox2.Text);
            string lport = textBox2.Text;
            if(String.IsNullOrWhiteSpace(server) || String.IsNullOrWhiteSpace(password) || String.IsNullOrWhiteSpace(sport) || String.IsNullOrWhiteSpace(lport))
            {
                MessageBox.Show("Fill in all the forms before proceeding!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string msg = String.Format("Server IP: {0}\nServer port: {1}\nPassword: {2}\nLocal port: {3}", server, sport, password, lport);
            MessageBox.Show(msg, "Proceed?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            // need to handle `no`
            if (!(System.IO.File.Exists("ssp.exe")))
            {
                textBlock.Text = "Be patient while I'm downloading necessary files...";
                Thread t = new Thread(() => {
                    WebClient wc = new WebClient();
                    wc.DownloadFile("https://github.com/jm33-m0/shadowsocks-plus/releases/download/v0.1/ssp.exe", @".\ssp.exe");
                });
                t.Start();
            }
            // Now start ssp proxy in background
            textBlock.Text = String.Format("Starting local proxy at 127.0.0.1:{0}...", lport);
            string args = String.Format("-s {0} -p {1} -k {2} -l {3}", server, sport, password, lport);
            ProcessStartInfo psi = new ProcessStartInfo("ssp.exe", args)
            {
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            Process process = Process.Start(psi);
            Process[] is_running = Process.GetProcessesByName("ssp");
            if(is_running.Length != 0)
            {
                textBlock.Text = String.Format("Local proxy started at 127.0.0.1:{0}", lport);
                MessageBox.Show("Local proxy started, feel free to click `Exit`", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e){}

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e){}

        private void textBox2_TextChanged(object sender, TextChangedEventArgs e){}

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            RegistryKey add = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            add.SetValue("SSPlus-Proxy", "\"" + System.IO.Directory.GetCurrentDirectory() + "\\Shadowsocks-Plus.exe" + "\"");
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            string keyName = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyName, true))
            {
                if (key != null)
                {
                    key.DeleteValue("SSPlus-Proxy");
                }
            }
        }
    }
}