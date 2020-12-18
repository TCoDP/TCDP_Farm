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

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            /*Binding MainWindow = new Binding();

            MainWindow.ElementName = "main";
            MainWindow.Path = new PropertyPath("Text"); // свойство элемента-источника
            myTextBlock.SetBinding(TextBlock.TextProperty, binding); // установка привязки для элемента-приемника
*/
        }
        private void M1_Click(object sender, RoutedEventArgs e)
        {

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

        }

        private void paginate_last_Click(object sender, RoutedEventArgs e)
        {
            // MessageBox.Show(ListBox.NameProperty.Name);
        }

    }
}
