using System;
using System.Collections.Generic;
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
using System.Windows.Media.TextFormatting;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для RemoveItem.xaml
    /// </summary>
    public partial class RemoveItem : Window
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetActiveWindow();
        DatabaseVapeEntities en = new DatabaseVapeEntities();
        public Goods goods;
         AdminCatalog adminCatalog;
        public RemoveItem(Goods g, AdminCatalog adminCatalog)
        {
            goods = g;
            this.adminCatalog = adminCatalog;
            InitializeComponent();
            alertImage.Source = new BitmapImage((new Uri("C:/Users/emiagin/source/repos/WpfApp1/WpfApp1/red.png", UriKind.RelativeOrAbsolute)));
        }
        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            var gd = en.Goods.Where(u => u.Name == goods.Name).FirstOrDefault();
            if (gd != null)
                en.Goods.Remove(gd);
            en.SaveChanges();
            this.Hide();
            IntPtr active = GetActiveWindow();
            Window ActiveWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(window => new WindowInteropHelper(window).Handle == active);
            ActiveWindow.Hide();
            AdminCatalog ac = new AdminCatalog(adminCatalog.login);
            ac.Show();
        }
    }
}
