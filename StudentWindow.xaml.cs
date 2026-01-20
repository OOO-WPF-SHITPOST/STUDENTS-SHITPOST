using Nedelyaeva.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Nedelyaeva
{
    /// <summary>
    /// Логика взаимодействия для StudentWindow.xaml
    /// </summary>
    public partial class StudentWindow : Window
    {
        private studContext _context;
        public StudentWindow()
        {
            InitializeComponent();
            _context = App.Context;
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            App.User = null;
            MainWindow lwin = new MainWindow();
            lwin.Show();
            this.Close();
        }
        private void LoadStudentData()
        {
            var myGrades = _context.Grades
                .Where(g => g.StudentId == App.User.Id)
                .Select(g => new
                {
                    SubjectName = _context.Subjects.Where(s => s.Id == g.SubjectId).Select(s => s.Name).FirstOrDefault(),
                    g.GradeValue,
                    g.CreatedAt,
                    g.IsDeleted
                })
                .ToList();
            MyGradesGrid.ItemsSource = myGrades;
        }
        private void Student_Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadStudentData();
        }
    }
}
