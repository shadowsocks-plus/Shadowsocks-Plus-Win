using System;
using System.Diagnostics;
using System.Net;
using Microsoft.Win32;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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
            if (System.IO.File.Exists(@".\config.txt"))
            {
                string[] lines = System.IO.File.ReadLines(".\\config.txt").ToArray();
                textBox.Text = lines[0];
                textBox1.Text = lines[1];
                passwordBox.Password = lines[2];
                textBox2.Text = lines[3];
            }
            string keyName = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyName, true))
            {
                if (key != null)
                {
                    string[] values = key.GetValueNames();
                    if (values.Contains("SSPlus-Proxy"))
                    {
                        checkBox.IsChecked = true;
                    }
                    else
                    {
                        checkBox.IsChecked = false;
                    }
                }
                else
                {
                    MessageBox.Show("No key found!", "Not found", MessageBoxButton.OK, MessageBoxImage.None);
                }
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // download method
        public async Task DownloadFileAsync()
        {
            using (var client = new WebClient())
            {
                await client.DownloadFileTaskAsync(
                    new Uri("https://github.com/shadowsocks-plus/Shadowsocks-Plus-Win/raw/master/Shadowsocks-Plus/ssp.exe"),
                    "ssp.exe"
                    );
            }
        }

        public bool invalid_form()
        {
            if (String.IsNullOrWhiteSpace(textBox.Text) ||
                String.IsNullOrWhiteSpace(passwordBox.Password) ||
                String.IsNullOrWhiteSpace(textBox1.Text) ||
                String.IsNullOrWhiteSpace(textBox2.Text))
            {
                return true;
            }
            else
            {
                IPAddress server_ip;
                int num;
                if(!(IPAddress.TryParse(textBox.Text, out server_ip)) ||
                    !(int.TryParse(textBox1.Text, out num)) ||
                    !(int.TryParse(textBox2.Text, out num)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void read_input()
        {
            string server = IPAddress.Parse(textBox.Text).ToString();
            textBox.Text = server;
            string sport = textBox1.Text;
            string password = passwordBox.Password;
            string lport = textBox2.Text;
            // write into file
            string[] config = { server, sport, password, lport };
            System.IO.File.WriteAllLines(@".\config.txt", config);
        }

        private async void button1_Click(object sender, RoutedEventArgs e)
        {
            // need to check if the proxy is already running
            Process[] pname = Process.GetProcessesByName("ssp");
            if (pname.Length != 0)
            {
                MessageBox.Show("Local proxy is already running!", "Warning: ", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!(invalid_form()))
            {
                read_input();
            }
            string[] lines = System.IO.File.ReadLines(".\\config.txt").ToArray();
            string server = lines[0];
            string sport = lines[1];
            string password = lines[2];
            string lport = lines[3];
            string msg = String.Format("Server IP: {0}\nServer port: {1}\nPassword: {2}\nLocal port: {3}", server, sport, password, lport);

            // check form
            if (invalid_form())
            {
                MessageBox.Show("Fill in all the forms before proceeding!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // need to handle `no`
            MessageBoxResult response = MessageBox.Show(msg, "Proceed?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (response == MessageBoxResult.No)
            {
                return;
            }
            if (!(System.IO.File.Exists("ssp.exe")))
            {
                textBlock.Text = "Be patient while I'm downloading necessary files...";
                await DownloadFileAsync();
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
            System.Threading.Thread.Sleep(1000);
            Process[] is_running = Process.GetProcessesByName("ssp");
            if (is_running.Length != 0)
            {
                textBlock.Text = String.Format("Local proxy started at 127.0.0.1:{0}", lport);
                MessageBox.Show("Local proxy started, feel free to click `Exit`\nRemember to configure your browser to use the proxy shown below", "Success!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                textBlock.Text = "Failed to start proxy, please try again";
                MessageBox.Show("Failed to start local proxy, please make sure the config details you provided are valid", "Failed!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e){}

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e){}

        private void textBox2_TextChanged(object sender, TextChangedEventArgs e){}

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            RegistryKey add = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            add.SetValue("SSPlus-Proxy", "\"" + System.IO.Directory.GetCurrentDirectory() + "\\ssp.exe" + "\"" + 
                String.Format(" -s {0} -p {1} -k {2} -l {3}", textBox.Text, textBox1.Text, passwordBox.Password, textBox2.Text));
            textBlock.Text = "Auto start enabled";
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            string keyName = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(keyName, true))
            {
                if (key != null)
                {
                    key.DeleteValue("SSPlus-Proxy");
                    textBlock.Text = "Auto start disabled";
                }
                else
                {
                    MessageBox.Show("No key found!", "Not found", MessageBoxButton.OK, MessageBoxImage.None);
                }
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process[] ssp = Process.GetProcessesByName("ssp");
                ssp[0].Kill();
                Process[] is_running = Process.GetProcessesByName("ssp");
                textBlock.Text = "Local proxy stopped";
                MessageBox.Show("Local proxy exited", "Exited", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Not running!", "Hey", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://jm33.me");
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (!(System.IO.File.Exists("readme.html")))
            {
                System.Diagnostics.Process.Start("https://github.com/shadowsocks-plus/Shadowsocks-Plus-Win/blob/master/how-to.md");
            }
            else
            {
                System.Diagnostics.Process.Start("readme.html");
            }
        }
    }
}