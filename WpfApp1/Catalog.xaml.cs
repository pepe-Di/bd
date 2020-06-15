using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для Catalog.xaml
    /// </summary>
    public partial class Catalog : Window
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetActiveWindow();
        DatabaseVapeEntities en = new DatabaseVapeEntities();
        public Catalog()
        {
            InitializeComponent();
        }
        public string login;
        public Catalog(string login)
        {
            this.login = login;
            InitializeComponent();
            DataContext = new ViewModel(ItemsView,login);
        }
        private void getSelectedItem(object sender, MouseButtonEventArgs e) 
        {
            IntPtr active = GetActiveWindow();
            Window ActiveWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(window => new WindowInteropHelper(window).Handle == active);
            ActiveWindow.Hide();
            int i = ItemsView.SelectedIndex + 1;
            foreach (Goods g in en.Goods)
            {
                i--;
                if (i == 0)
                {
                    Item it = new Item(g, this);
                    it.Show();
                }
            }
        }
    }
}
