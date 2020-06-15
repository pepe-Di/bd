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
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для AdminItem.xaml
    /// </summary>
    public partial class AdminItem : Window
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetActiveWindow();
        DatabaseVapeEntities en = new DatabaseVapeEntities();
        public Goods item;
        AdminCatalog adminCatalog;
        public AdminItem()
        {
            InitializeComponent();
        }
        private string path = "C:/Users/emiagin/source/repos/WpfApp1/WpfApp1/img/";
        public AdminItem(Goods g, AdminCatalog adminCatalog)
        {
            this.adminCatalog = adminCatalog;
            InitializeComponent();
            DataContext = new ViewModel();
            item = g;
            Name.Text = item.Name;
            Description.Text = item.Description;
            Price.Text = item.Price.ToString();
            Amount.Text = item.Amount.ToString();
            ImagePath.Text = item.Preview;
            Preview.Source = new BitmapImage(new Uri(path+item.Preview, UriKind.RelativeOrAbsolute));
        }
        private void SubmitClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var gd = en.Goods.Where(u => u.Name == item.Name).FirstOrDefault();
                if (gd != null)
                {
                    gd.Name = Name.Text;
                    gd.Description = Description.Text;
                    gd.Price = Price.Text;
                    gd.Amount = int.Parse(Amount.Text);
                    gd.Preview = ImagePath.Text;
                    int value = 2;
                    if (int.Parse(Amount.Text) == 0)
                        value = 2;
                    else if (int.Parse(Amount.Text) > 0)
                    {
                        value = 1;
                    }
                    if (int.Parse(Amount.Text) > 11)
                    {
                        value = 0;
                    }
                    gd.StatusId = value;
                    en.SaveChanges();
                    MessageBox.Show(gd.Name + " changed!");
                    IntPtr active = GetActiveWindow();
                    Window ActiveWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(window => new WindowInteropHelper(window).Handle == active);
                    ActiveWindow.Hide();
                    AdminCatalog ac = new AdminCatalog(adminCatalog.login);
                    ac.Show();
                }
            }
            catch
            {
                MessageBox.Show("error");
            }
        }
        private void LoadClick(object sender, RoutedEventArgs e)
        {
            try 
            {
                Preview.Source = new BitmapImage(new Uri(path+ImagePath.Text, UriKind.RelativeOrAbsolute));
            }
            catch 
            {
                MessageBox.Show("error");
            }
        }
            private void BackClick(object sender, RoutedEventArgs e)
        {
            IntPtr active = GetActiveWindow();
            Window ActiveWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(window => new WindowInteropHelper(window).Handle == active);
            ActiveWindow.Hide();
            adminCatalog.Show();
        }
    }
}
