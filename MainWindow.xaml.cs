using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Nedelyaeva.Data;
using Nedelyaeva.Models;

namespace Nedelyaeva
{
    public partial class MainWindow : Window
    {
        private studContext _context;
        public MainWindow()
        {   
            InitializeComponent();
            _context = App.Context;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var login = LoginTextBox.Text.Trim();
            var pass = PasswordBox.Password;

            var _currentUser = _context.Users
                .Where(x => x.Login == login && x.Password == pass && x.IsDeleted == false)
                .FirstOrDefault();

            if (_currentUser == null)
            {
                MessageBox.Show("Неверные учетные данные.");
                return;
            }

            App.User = _currentUser;

            if (_currentUser.Role == "teacher")
            {
                TeacherWindow twin = new TeacherWindow();
                twin.Show();
                this.Close();
            }
            else
            {
                StudentWindow swin = new StudentWindow();
                swin.Show();
                this.Close();
            }
        }
    }
}