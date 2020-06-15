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
    /// Логика взаимодействия для AddItem.xaml
    /// </summary>
    public partial class AddItem : Window
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetActiveWindow();
        DatabaseVapeEntities en = new DatabaseVapeEntities();
        AdminCatalog adminCatalog;
        public AddItem()
        {
            InitializeComponent();
        }
        private string path = "C:/Users/emiagin/source/repos/WpfApp1/WpfApp1/img/";
        public AddItem(AdminCatalog adminCatalog)
        {
            this.adminCatalog = adminCatalog;
            InitializeComponent();
            DataContext = new ViewModel();
         }
        private void AddClick(object sender, RoutedEventArgs e)
        {
            try 
            {
                var item = en.Goods.Where(u => u.Name == Name.Text).FirstOrDefault();
                if (item == null)
                {
                    int value = 2;
                    if (int.Parse(Amount.Text) > 0) 
                    {
                        value = 1;
                    }
                    if (int.Parse(Amount.Text) > 11)
                    {
                        value = 0;
                    }
                    en.Goods.Add(new Goods()
                    {
                        Name = Name.Text,
                        Description = Description.Text,
                        Price = Price.Text,
                        Amount = int.Parse(Amount.Text),
                        Preview = ImagePath.Text,
                        StatusId = value
                    });
                    en.SaveChanges();
                    MessageBox.Show(Name.Text + " added!");
                    IntPtr active = GetActiveWindow();
                    Window ActiveWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(window => new WindowInteropHelper(window).Handle == active);
                    ActiveWindow.Hide();
                    AdminCatalog ac = new AdminCatalog(adminCatalog.login);
                    ac.Show();
                }
                else
                {
                    MessageBox.Show("This item already exists.");
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

