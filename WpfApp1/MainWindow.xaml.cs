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

        private void add_accounts_Click(object sender, RoutedEventArgs e)
        {
            addAccounts();
        }

        private void M2_Click(object sender, RoutedEventArgs e)
        {

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
            List<Account> packof10 = db.Accounts.
                Where(x => x.id > offset * 10 - 10 && x.id < offset * 10).ToList();
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
                return;
            }
            
            string items = @"<ScrollViewer VerticalScrollBarVisibility='Hidden'
                xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'
                xmlns:x='http://schemas.microsoft.com/winfx/2006/xaml'
                xmlns:d='http://schemas.microsoft.com/expression/blend/2008'
                xmlns:mc='http://schemas.openxmlformats.org/markup-compatibility/2006'>
                <StackPanel MaxHeight='330'>";
            foreach (Account x in packof10)
            {
                items += "<StackPanel Orientation='Horizontal'>" +
                    $"<Expander x:Name='inpack{i}' Header='id:{x.id}; login:{x.Login}' >" +
                        $"<TextBlock x:Name='text{i++}' Text='password:******' />" +
                    "</Expander>" +
                "</StackPanel>";
            }

            items += "</StackPanel></ScrollViewer>";
            UIElement UI = XamlReader.Parse(items) as UIElement;
            main.Items.Add(UI);
        }

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
                string[] x = line.Split(':');
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
                db.SaveChangesAsync();// .SaveChanges();
                MessageBox.Show(i + " accounts were successfully added.");
            }
            catch (Exception e)
            {
                //MessageBox.Show("Error writing to the database.");
                MessageBox.Show($"Error in line #{e.Message}");
            }
            /* Запись в бд */
        }
    }
}
