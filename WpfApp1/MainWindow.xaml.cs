using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
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
        List<Account> working_pack;
        public static readonly string[] all_ranks = {
            "Без ранга",
            "Silver 1",
            "Silver 2",
            "Silver 3",
            "Silver 4",
            "Silver Elite",
            "Silver Elite Master",
            "Gold Nova 1",
            "Gold Nova 2",
            "Gold Nova 3",
            "Gold Nova Master",
            "Master Guardian 1",
            "Master Guardian 2",
            "Master Guardian Elite",
            "Distinguished Master Guardian",
            "Legendary Eagle",
            "Supreme Master First Class",
            "The Global Elite"
        };

        public MainWindow()
        {
            InitializeComponent();
            db = new AppContext();
            showPack(paginator);
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            paginator = 1;
            showPack(paginator);
        }

        private void add_accounts_Click(object sender, RoutedEventArgs e)
        {
            addAccounts();
            showPack(paginator);
        }

        private void run_accounts_Click(object sender, RoutedEventArgs e)
        {
            foreach (Account x in working_pack)
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = @"Launcher.exe",
                        //UseShellExecute = false,
                        Arguments = $"{ x.Login } { x.Password }"
                        //FileName = @"F:\Programs\Steam\steam.exe",
                        //Arguments = $"-login \"{x.Login.Trim()}\" \"{x.Password.Trim()}\" -applaunch 730 -w 640 -h 480 -x 20 -y 20"
                    }
                };

                process.Start();
                Thread.Sleep(1000);
            }
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

        private List<Account> getPack(int offset = 1)
        {
            int online = Online.SelectedIndex;
            int lvl = Lvl.SelectedIndex;
            int rank = Rank.SelectedIndex;

            main.Items.Clear();
            List<Account> packof10 = db.Accounts
                .Where(x => x.id > offset * 10 - 10 && x.id <= offset * 10)
                .Where(x => x.Online != online > 0 ? online == 1 ? false : true : true)
                .Where(x => x.Online != online > 0 ? online == 2 ? true : false : false)
                //.Where(x => x.Online == online == 1 ? true : false)
                //.Where(x => x.Online == online == 2 ? false : true)
                .Where(x => x.Lvl == lvl)
                //.Where(x => x.Rank == Rank.Text)
                .ToList();
            working_pack = packof10;
            return packof10;
        }

        private void showPack(int offset)
        {
            List<Account> packof10 = getPack(offset);
            int i = 1;

            if (packof10.Count() == 0)
            {
                TextBlock TB1 = new TextBlock();
                TB1.Text = "Новых аккаунтов пока что нет.\n" +
                    "Вы можете их добавить, нажав кнопку ниже";

                Button B1 = new Button();
                B1.Content = "Добавить";
                B1.Click += add_accounts_Click;

                main.Items.Add(TB1);
                main.Items.Add(B1);
                paginate_last.IsEnabled = false;
                return;
            }
            
            string items = @"<ScrollViewer CanContentScroll='True'
                VerticalScrollBarVisibility='Hidden'
                xmlns ='http://schemas.microsoft.com/winfx/2006/xaml/presentation'
                xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'
                xmlns:d='http://schemas.microsoft.com/expression/blend/2008'
                xmlns:mc='http://schemas.openxmlformats.org/markup-compatibility/2006'>
                <StackPanel MaxHeight='330'>";
            foreach (Account x in packof10)
            {
                items += $"<StackPanel Orientation='Horizontal'>" +
                        $"<TextBox x:Name='inpack{i}' IsReadOnly='True' Background='Transparent' Text='{x.id}) {x.Login}'>" +
                        /*"<TextBlock.ContextMenu>" +
                            "<ContextMenu>" +
                                "<MenuItem Header='Copy' />" +
                            "</ContextMenu>" +
                        "</TextBlock.ContextMenu>" +
                        "<x:Code><![CDATA[" +
                            "MenuItem Item = new MenuItem();" +
                            $"Item.Name = 'Copy{i}';" +
                            "Item.Header = 'Copy';" +
                            "Item.Click += MenuItem_Click;" +
                            //$"Copy{i}.Click += Clipboard.SetText({working_pack[i-1].Login});" +
                            "MenuItem ContextMenu = (MenuItem)this.MenuItem;" +
                            "ContextMenu.Items.Add(Item);" +
                        "}]]></x:Code> " +*/
                        "</TextBox>" +
                    $"<TextBlock x:Name='text{i++}' Text='password:******'>" +
                        "<TextBlock.ContextMenu>" +
                            "<ContextMenu>" +
                                "<MenuItem Header='Copy' />" +
                            "</ContextMenu>" +
                        "</TextBlock.ContextMenu>" +
                    "</TextBlock>" +
                "</StackPanel>";
            }

            items += "</StackPanel></ScrollViewer>";
            UIElement UI = XamlReader.Parse(items) as UIElement;
            main.Items.Add(UI);
            paginate_last.IsEnabled = true;
        }
        /*"<x:Code><![CDATA[" +
            $"ContextMenu.Click += Copy_Click(this.Name);" +
            "void Copy_Click(object sender, RoutedEventArgs e, string x){" +
            "Clipboard.SetText(x);" +
        "}]]></x:Code> " +*/

        private void addAccounts()
        {
            int i = 0;

            /* Инициализация файла */
            OpenFileDialog file = new OpenFileDialog();
            file.DefaultExt = ".txt";
            file.Filter = "Text files (.txt)|*.txt";
            if (file.ShowDialog() == false) return;
            string name = file.FileName;
            List<Account> forDB = new List<Account>();

            /* Чтение из файла */
            foreach (string line in File.ReadAllText(name)
                .Split(new string[] { "\n" }, StringSplitOptions.None))
            {
                if (line.Length == 0) break;
                string[] x = line.Split(' ');
                Account y = new Account();
                y.Login = x[0];
                y.Password = x[1];
                /*y.Online = false;
                y.Timestamp = i;
                y.Steamid64 = i;
                y.Rank = "q";
                y.Lvl = 1;
                y.Nickname = "q";*/
                forDB.Add(y);
                //db.Accounts.Add(y);
                //db.SaveChanges();
                i++;
            }

            /* Запись в бд */
            try
            {
                db.Accounts.AddRange(forDB);
                db.SaveChangesAsync();
                MessageBox.Show(i + " accounts were successfully added.");
            }
            catch (Exception e)
            {
                MessageBox.Show($"#{e.Message}");
            }
        }


        //        private void ContextMenu_Checked(object sender, RoutedEventArgs e)
        //        {
        //        var item = e.OriginalSource as MenuItem;
        //        MessageBox.Show($"{item.Header} was clicked");
        //    }
    }
}
