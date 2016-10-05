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
using HTTPReqHelper;
using System.IO;
using System.ComponentModel;
using Microsoft.Win32;

namespace WpfApplication1
{

    public class URLList
    {
        public string URL { get; set; }
        public string Status { get; set; }

        public string NewLocation { get; set; } // For 30X redirect
        

    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        public MainWindow()
        {
            InitializeComponent();
            
        }

        public static async void BrowseAsync(string url,ListView listView)
        {
           ResponseInfo resp = await Task.Run(() => WebRequestGetExample.Browse(url));
           URLList web = new URLList();
           web.URL = url;
           web.Status = resp.StatusCode + " " + resp.StatusDesc;
           web.NewLocation = resp.NewLocation;
           //uList.Add(web);
           listView.Items.Add(web);
           
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            listView.Items.Clear();

            List<string> webList = new List<string>();
            List<URLList> uList = new List<URLList>();

            string filename = lblFileName.Content.ToString();

            FileStream fs = new FileStream(filename, FileMode.Open);
            using (StreamReader file = new StreamReader(fs))
            {
                string line = file.ReadLine();
                while (line != null)
                {
                    webList.Add(line);
                    line = file.ReadLine();
                }
            }

            /*webList.Add("http://www.google.com");
            webList.Add("http://www.yahoo.com/123");
            webList.Add("http://www.lowyat.net");
            webList.Add("http://www.youtubexxxmunyau.com/123");
            webList.Add("http://www.faxcebook.com");*/

            foreach (var url in webList)
            {
                BrowseAsync(url,listView);
            }

           
            //listView.ItemsSource = uList;

        }

        private void btnSort_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt";

            if (openFileDialog.ShowDialog() == true)
            {
                if (openFileDialog.FileName != null)
                {
                    lblFileName.Content = openFileDialog.FileName;
                    button.IsEnabled = true;
                } else
                {
                    button.IsEnabled = false;
                }
            }
        }
    }
}
