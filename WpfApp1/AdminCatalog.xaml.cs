using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для AdminCatalog.xaml
    /// </summary>
    public partial class AdminCatalog : Window
    {
        public string login;
        [DllImport("user32.dll")]
        static extern IntPtr GetActiveWindow();
        DatabaseVapeEntities en = new DatabaseVapeEntities();
        public AdminCatalog()
        {
            InitializeComponent();
        }
        public AdminCatalog(string login)
        {
            this.login = login;
            InitializeComponent();
            DataContext = new ViewModel(ItemsView, login);
        }
        private void RemoveClick(object sender, RoutedEventArgs e) 
        {
            int i = ItemsView.SelectedIndex + 1;
            foreach (Goods g in en.Goods)
            {
                i--;
                if (i == 0)
                {
                    RemoveItem it = new RemoveItem(g, this);
                    it.Show();
                }
            }
        }
        private void AddClick(object sender, RoutedEventArgs e)
        {
            IntPtr active = GetActiveWindow();
            Window ActiveWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(window => new WindowInteropHelper(window).Handle == active);
            ActiveWindow.Hide();
            AddItem i = new AddItem(this);
            i.Show();
        }
        private void getSelectedItem(object sender, MouseButtonEventArgs e)
        {
            IntPtr active = GetActiveWindow();
            Window ActiveWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(window => new WindowInteropHelper(window).Handle == active);
            ActiveWindow.Hide();
            int i = ItemsView.SelectedIndex+1;
            foreach (Goods g in en.Goods) 
            {
                i--;
                if (i == 0) 
                {
                    AdminItem it = new AdminItem(g, this);
                    it.Show();
                }
            }
        }
    }
}

