using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    /// Логика взаимодействия для Item.xaml
    /// </summary>
    public partial class Item : Window
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetActiveWindow();
        private string path = "C:/Users/emiagin/source/repos/WpfApp1/WpfApp1/img/";
        DatabaseVapeEntities en = new DatabaseVapeEntities();
        private Goods item;
        Catalog catalog;
        public Item() { }
        public Item(Goods g,Catalog catalog)
        {
            this.catalog = catalog;
            InitializeComponent();
            DataContext = new ViewModel();
            item = g;
            Name.Text = item.Name;
            Description.Text = item.Description;
            Price.Text = item.Price.ToString()+" $";
            Amount.Text = item.Amount.ToString();
            Preview.Source = new BitmapImage(new Uri(path+item.Preview, UriKind.RelativeOrAbsolute));
        }
        private void BuyClick(object sender, RoutedEventArgs e) 
        {
            try 
            {
                var gd = en.Goods.Where(u => u.Name == item.Name).FirstOrDefault();
                if (gd != null)
                {
                    if (int.TryParse(gd.Amount.ToString(), out int value))
                    {
                        if (gd.Amount > 0)
                        {
                            gd.Amount -= 1;
                            Amount.Text = gd.Amount.ToString();
                            MessageBox.Show("sold.");
                            if (gd.Amount > 11)
                            {
                                gd.StatusId = 0;
                            }
                            if (gd.Amount < 11)
                            {
                                gd.StatusId = 1;
                            }
                            if (gd.Amount == 0)
                            {
                                gd.StatusId = 2;
                            }
                            en.SaveChanges();
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("catch");
            }
        }
        private void BackClick(object sender, RoutedEventArgs e)
        {
            IntPtr active = GetActiveWindow();
            Window ActiveWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(window => new WindowInteropHelper(window).Handle == active);
            ActiveWindow.Hide();
            Catalog c = new Catalog(catalog.login);
            c.Show();
        }
    }
}
