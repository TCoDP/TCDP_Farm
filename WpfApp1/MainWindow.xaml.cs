using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Gameloop.Vdf;
using SteamAuth;
using System.Runtime.InteropServices;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        AppContext db;
        static int paginator = 1;
        List<Account> working_pack;
        static string STEAM = @"C:\Program Files (x86)\Steam\", 
                    CSGO = @"steamapps\common\Counter-Strike Global Offensive\";

        private Manifest manifest;
        private SteamGuardAccount[] allAccounts;
        private string SteamGuardCode = "";
        [DllImport("user32.dll")]
        static extern short VkKeyScan(char ch);
        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        public MainWindow()
        {
            InitializeComponent();
            db = new AppContext();
            showPack(paginator);
            try
            {
                this.manifest = Manifest.GetManifest();
            }
            catch (ManifestParseException)
            {
                MessageBox.Show("Unable to read your settings. Try restating SDA.", "Steam Desktop Authenticator");
                this.Close();
            }
        }

        private void add_accounts_Click(object sender, RoutedEventArgs e)
        {
            addAccounts();
            showPack(paginator);
        }
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        private void run_accounts_Click(object sender, RoutedEventArgs e)
        {
            int count = 0;
            foreach (Account x in working_pack)
            {
                allAccounts = manifest.GetAllAccounts();
                if (allAccounts.Length > 0)
                {
                    for (int m = 0; m < allAccounts.Length; m++)
                    {
                        SteamGuardAccount account = allAccounts[m];
                        if (account.AccountName == x.Login)
                            SteamGuardCode = account.GenerateSteamGuardCode();
                    }
                }

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = @"Launcher.exe",
                        UseShellExecute = false,
                        Arguments = $"{ x.Login } { x.Password } { x.Timestamp } { SteamGuardCode } { (count*350)+20 }",
                        CreateNoWindow = false
                    }
                };

                process.Start();
                count++;
                Thread.Sleep(35000);
            }
        }

        private void M3_Click(object sender, RoutedEventArgs e)
        {
            /*try
            {
                foreach (Process process in ((IEnumerable<Process>)Process.GetProcesses()).Where<Process>((Func<Process, bool>)(pr => pr.ProcessName.ToLower().Equals("steam"))))
                    process.Kill();
                foreach (Process process2 in ((IEnumerable<Process>)Process.GetProcesses()).Where<Process>((Func<Process, bool>)(pr => pr.ProcessName.Equals("Launcher"))))
                    process2.Kill();
            }
            catch (Exception ex)
            {
            }*/
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
                Where(x => x.id > offset * 10 - 10 && x.id <= offset * 10).ToList();
            working_pack = packof10;
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
                string[] x = line.Split(' ');
                Account y = new Account();
                y.Login = x[0];
                y.Password = x[1];
                y.Steamid64 = 0;
                y.Online = false;
                y.Timestamp = 0;
                y.Rank = "q";
                y.Lvl = 1;
                y.Nickname = "q";
                string loginusers = File.ReadAllText(STEAM + @"config\loginusers.vdf");//проверка на существование и ошибки 
                dynamic volvo = VdfConvert.Deserialize(loginusers);//проверка на существование и ошибки 
                foreach (dynamic ItemID in volvo.Value.Children())
                {
                    if (Convert.ToString(ItemID.Value.AccountName) == y.Login)
                    {
                        y.Steamid64 = Convert.ToInt64(ItemID.ToString().Split('"')[1]);
                        y.Nickname = Convert.ToString(ItemID.Value.PersonaName);
                        y.Timestamp = Convert.ToInt32(ItemID.Value.Timestamp.ToString());
                    }
                }

                if (y.Steamid64 == 0)
                {
                    //MessageBox.Show($"The account {y.Login} is missing in the file loginusers.vdf");
                   
                    var process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = STEAM + "steam.exe",
                            UseShellExecute = false,
                            Arguments = $"-login { y.Login.Trim() } { y.Password.Trim() }",
                            RedirectStandardOutput = true
                        }
                    };

                    process.Start();
                    Thread.Sleep(20000);
                    allAccounts = manifest.GetAllAccounts();
                    if (allAccounts.Length > 0)
                    {
                        for (int j = 0; j < allAccounts.Length; j++)
                        {
                            SteamGuardAccount account = allAccounts[j];
                            if (account.AccountName == y.Login)
                                SteamGuardCode = account.GenerateSteamGuardCode();
                        }
                    }
                    if (SteamGuardCode != "")
                    {
                        char[] values = SteamGuardCode.ToCharArray();
                        foreach (char letter in values)
                        {
                            SetForegroundWindow(process.MainWindowHandle);
                            Thread.Sleep(200);
                            PostMessage(process.MainWindowHandle, 0x0100, VkKeyScan(letter), 0);
                        }
                        Thread.Sleep(3000);
                        SetForegroundWindow(process.MainWindowHandle);
                        Thread.Sleep(200);
                        PostMessage(process.MainWindowHandle, 0x0100, 0x0D, 0);
                        SteamGuardCode = "";
                    }
                    Thread.Sleep(15000);
                    process.Kill();

                    loginusers = File.ReadAllText(STEAM + @"config\loginusers.vdf");//проверка на существование и ошибки 
                    volvo = VdfConvert.Deserialize(loginusers);//проверка на существование и ошибки 
                    foreach (dynamic ItemID in volvo.Value.Children())
                    {
                        if (Convert.ToString(ItemID.Value.AccountName) == y.Login)
                        {
                            y.Steamid64 = Convert.ToInt64(ItemID.ToString().Split('"')[1]);
                            y.Nickname = Convert.ToString(ItemID.Value.PersonaName);
                            y.Timestamp = Convert.ToInt32(ItemID.Value.Timestamp.ToString());
                        }
                    }
                }

                /*Создание всех нужных файлов и папок*/
                string targetPath = STEAM + $"steam_{y.Timestamp}.exe";
                File.Copy(STEAM + "steam.exe", targetPath, true);

                DirectoryInfo userdataDir = new DirectoryInfo(STEAM + "userdata");
                DirectoryInfo[] userdataDirs = userdataDir.GetDirectories();
                foreach (DirectoryInfo dir in userdataDirs)
                {
                    if (dir.Name != "ac")
                    {
                        string localconfig = File.ReadAllText(dir.FullName + @"\config\localconfig.vdf");
                        dynamic dvolvo = VdfConvert.Deserialize(localconfig);

                        if (Convert.ToString(dvolvo.Value.friends.PersonaName) == y.Nickname)
                        {
                            Directory.CreateDirectory(dir.FullName + @"\730");
                            Directory.CreateDirectory(dir.FullName + @"\730\local");
                            Directory.CreateDirectory(dir.FullName + @"\730\local\cfg");
                            DirectoryInfo userdata = new DirectoryInfo(@"C:\TCoDP\TCDP_Farm\WpfApp1\Resources\data\userdata");
                            DirectoryInfo cfg = new DirectoryInfo(dir.FullName + @"\730\local\cfg");
                            CopyDirectory(userdata, cfg);
                        }
                    }
                }
                
                DirectoryInfo sourceDir = new DirectoryInfo(@"C:\TCoDP\TCDP_Farm\WpfApp1\Resources\data\game");
                string target = STEAM + CSGO + $"csgo_{y.Timestamp}";
                Directory.CreateDirectory(target);
                DirectoryInfo destinationDir = new DirectoryInfo(target);

                CopyDirectory(sourceDir, destinationDir);
                /*Создание всех нужных файлов и папок*/

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
        static void CopyDirectory(DirectoryInfo source, DirectoryInfo destination)
        {
            if (!destination.Exists) destination.Create();

            FileInfo[] files = source.GetFiles();
            foreach (FileInfo file in files)
            {
                file.CopyTo(Path.Combine(destination.FullName, file.Name), true);
            }

            DirectoryInfo[] dirs = source.GetDirectories();
            foreach (DirectoryInfo dir in dirs)
            {
                string destinationDir = Path.Combine(destination.FullName, dir.Name);

                CopyDirectory(dir, new DirectoryInfo(destinationDir));
            }
        }

    }
}