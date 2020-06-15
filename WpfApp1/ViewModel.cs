using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    class ViewModel : INotifyPropertyChanged
    {
        public struct Items
        {
            public string Name { get; set; }
            public string Price { get; set; }
            public string Amount { get; set; }
            public string Status { get; set; }
            public BitmapImage Preview { get; set; }
            public string Description { get; set; }
        }
        private RelayCommand regCommand;
        private RelayCommand signINCommand;
        private string log_input;
        private string name_input;
        private string path = "C:/Users/emiagin/source/repos/WpfApp1/WpfApp1/img/";
        public ViewModel() { }
        public ViewModel(ListView ItemsView, string login)
        {
            foreach (Goods g in en.Goods)
            {
                ItemsView.Items.Add(new Items
                {
                    Preview = new BitmapImage(new Uri(path+g.Preview, UriKind.RelativeOrAbsolute)),
                    Name = g.Name.ToString(),
                    Status = g.Status.Status1.ToString()
                });
            }
            Cur_account_name = login+"  ";
        }
        [DllImport("user32.dll")]
        static extern IntPtr GetActiveWindow();
        DatabaseVapeEntities en = new DatabaseVapeEntities();
        private string name_account;
        public string Name_account 
        {
            get
            {
                return this.name_account;
            }
            set
            {
                name_account = value;
                OnPropertyChanged(nameof(Name_account));
            }
        }
        public string Log_input
        {
            get 
            {
                return this.log_input;
            }
            set
            {
                log_input = value;
                OnPropertyChanged(nameof(Log_input));
            }
        }
        public string Name_input
        {
            get
            {
                return this.name_input;
            }
            set
            {
                name_input = value;
                OnPropertyChanged(nameof(Name_input));
            }
        }
        private string cur_account_name;
        public string Cur_account_name 
        {
            get 
            {
                return this.cur_account_name;
            }
            set 
            {
                cur_account_name = value;
                OnPropertyChanged(nameof(Cur_account_name));
            }
        }
        private void SignIN(PasswordBox passwordBox) 
        {
            if (passwordBox.Password.ToString() != "" && log_input != "")
            {
                var user = en.User.Where(u => u.Login == log_input&&u.Password == passwordBox.Password.ToString()).FirstOrDefault();
                if (user != null)
                {
                    IntPtr active = GetActiveWindow();
                    Window ActiveWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(window => new WindowInteropHelper(window).Handle == active);
                    ActiveWindow.Hide();
                    switch (user.RoleId) 
                    {
                        case 2:
                        case 0: {
                                AdminCatalog c = new AdminCatalog(user.Login);
                                c.Show(); break; }
                        default: {  Catalog c = new Catalog(user.Login);
                                c.Show(); break;
                            }
                    }
                    MessageBox.Show("Welcome, " + user.Name + "!");
                }
                else 
                {
                    MessageBox.Show("User not found.");
                }
            }
            else 
            {
                MessageBox.Show("Fill the form.");
            }
            Log_input = "";
            passwordBox.Password = null;
        }
        public ICommand SigningIn
        {
            get
            {
                return new RelayCommand<PasswordBox>(SignIN);
            }
        }
        private bool check;
        public bool Check 
        {
            get
            {
                return this.check;
            }
            set
            {
                check = value;
                OnPropertyChanged(nameof(Check));
            }
        }
        private void AddUser(PasswordBox passwordBox) 
        {
            if (Check)
            {
                if (passwordBox.Password.ToString() != "" && log_input != "" && name_input != "")
                {
                    var user = en.User.Where(u => u.Login == log_input).FirstOrDefault();
                    if (user == null)
                    {
                        en.User.Add(new User()
                        {
                            Login = log_input,
                            Name = name_input,
                            Password = passwordBox.Password.ToString(),
                            RoleId = 1
                        });
                        en.SaveChanges();
                        MessageBox.Show("An user account for " + name_input + " created.");

                    }
                    else
                    {
                        MessageBox.Show("This login already exists.");
                    }
                }
                else
                {
                    MessageBox.Show("Fill the form.");
                }
            }
            else 
            {
                MessageBox.Show("You must agree.");
            }
            Log_input = "";
            Name_input = "";
            passwordBox.Password = null;
        }
        private void TermsANDPrivacy() 
        {
            MessageBox.Show("");
        }
        public ICommand TermsPrivacy
        {
            get
            {
                return new RelayCommand(TermsANDPrivacy);
            }
        }
        public ICommand Register
        {
            get
            {
                return new RelayCommand<PasswordBox>(AddUser);
            }
        }
        private RelayCommand deleteUser;
        public RelayCommand DeleteUser 
        {
            get
            {
                if (deleteUser == null)
                {
                    deleteUser = new RelayCommand(() =>
                    {
                        var user = en.User.Where(u => u.Login == log_input).FirstOrDefault();
                        if (user != null) en.User.Remove(user);
                        en.SaveChanges();
                    });
                }
                return deleteUser;
            }
        }
        public RelayCommand RegCommand
        {
            get
            {
                if (regCommand == null)
                {
                    regCommand = new RelayCommand(() =>
                    {
                        IntPtr active = GetActiveWindow();
                        Window ActiveWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(window => new WindowInteropHelper(window).Handle == active);
                        ActiveWindow.Hide();
                        Reg reg = new Reg();
                        reg.Show();
                    });
                }
                return regCommand;
            }
        }
        public RelayCommand SignINCommand
        {
            get
            {
                if (signINCommand == null)
                {
                    signINCommand = new RelayCommand(() =>
                    {
                        IntPtr active = GetActiveWindow();
                        Window ActiveWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(window => new WindowInteropHelper(window).Handle == active);
                        ActiveWindow.Close();
                        MainWindow win = new MainWindow();
                        win.Show();
                    });
                }
                return signINCommand;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
