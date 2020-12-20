using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AppContext db;
        static int paginator = 1;
        public MainWindow()
        {
            InitializeComponent();
            db = new AppContext();
            showPack(paginator);
        }
        private void M1_Click(object sender, RoutedEventArgs e)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = @"C:\TCoDP\TCDP_Farm\Launcher.exe",
                    Arguments = "frosbolboocastio3808 c8B5h79sg9"
                }
            };

            process.Start();
            // while (!process.StandardOutput.EndOfStream)
            // {
            //string line = process.StandardOutput.ReadLine();
            // }
        }

        private void M2_Click(object sender, RoutedEventArgs e)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = @"C:\TCoDP\TCDP_Farm\Launcher.exe",
                    Arguments = "erprivicatgas2622 Slrs1o4tuai"
                }
            };

            process.Start();
        }

        private void M3_Click(object sender, RoutedEventArgs e)
        {

        }

        private void M4_Click(object sender, RoutedEventArgs e)
        {

        }

        private void paginate_prew_Click(object sender, RoutedEventArgs e)
        {
            if (paginator == 1)
                paginator = 2;
            showPack(--paginator);
        }

        private void paginate_last_Click(object sender, RoutedEventArgs e)
        {
            showPack(++paginator);
        }
        private void showPack(int offset = 1)
        {
            main.Items.Clear();
            // https://www.cyberforum.ru/wpf-silverlight/thread428098.html
            string items = @"<ScrollViewer VerticalScrollBarVisibility='Hidden' " +
                "xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' " +
                "xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml' " +
                "xmlns:d='http://schemas.microsoft.com/expression/blend/2008' " +
                "xmlns:mc='http://schemas.openxmlformats.org/markup-compatibility/2006'>" +
                "<StackPanel MaxHeight='330'>";
            List<Account> packof10 = db.Accounts.
                Where(x => x.id < offset * 10 && x.id > offset * 10 - 10).ToList();
            int i = 1;
            foreach (Account x in packof10)
            {
                items += "<StackPanel Orientation='Horizontal'>" +
                        $"<Expander x:Name='inpack{i}' Header='id:{x.id}; login:{x.Login}' >" +
                            $"<TextBlock x:Name='text{i++}' Text='password:{x.Password}' />" +
                        "</Expander>" +
                        "</StackPanel>";
            }
            items += "</StackPanel></ScrollViewer>";
            var UI = XamlReader.Parse(items) as UIElement;
            main.Items.Add(UI);
        }
    }
}
