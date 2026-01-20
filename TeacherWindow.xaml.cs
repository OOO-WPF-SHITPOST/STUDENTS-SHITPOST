using Nedelyaeva.Data;
using Nedelyaeva.Models;
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
    /// Логика взаимодействия для TeacherWindow.xaml
    /// </summary>
    public partial class TeacherWindow : Window
    {
        private studContext _context;
        public TeacherWindow()
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
        private void LoadTeacherData()
        {
            var _currentUser = App.User;
            // Subjects for which teacher is responsible
            var subjects = _context.Subjects
                .Where(s => s.TeacherId == _currentUser.Id)
                .Select(s => new { s.Id, s.Name, s.IsDeleted })
                .ToList();
            SubjectsGrid.ItemsSource = subjects;

            // Students in teacher's groups: we consider students that are in groups of subjects' students
            var students = _context.Users
                .Where(u => u.Role == "student")
                .Select(u => new
                {
                    u.Id,
                    u.LastName,
                    u.FirstName,
                    GroupName = _context.Groups.Where(g => g.Id == u.GroupId).Select(g => g.Name).FirstOrDefault()
                })
                .ToList();
            StudentsGrid.ItemsSource = students;

            // Grades for subjects of this teacher (including deleted to allow restore)
            var grades = _context.Grades
                .Where(g => _context.Subjects.Where(s => s.TeacherId == _currentUser.Id).Select(s => s.Id).Contains(g.SubjectId))
                .Select(g => new
                {
                    g.Id,
                    StudentName = _context.Users.Where(u => u.Id == g.StudentId).Select(u => u.LastName + " " + u.FirstName).FirstOrDefault(),
                    SubjectName = _context.Subjects.Where(s => s.Id == g.SubjectId).Select(s => s.Name).FirstOrDefault(),
                    g.GradeValue,
                    g.CreatedAt,
                    g.IsDeleted
                })
                .ToList();
            GradesGrid.ItemsSource = grades;
        }
        private void Teacher_Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadTeacherData();
        }
        private void AddGrade_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new GradeEditor(App.User.Id, isEdit: false);
            if (dlg.ShowDialog() == true)
            {
                // при добавлении учитываем уникальность student+subject
                var existing = _context.Grades
                    .Where(g => g.StudentId == dlg.ResultStudentId && g.SubjectId == dlg.ResultSubjectId)
                    .FirstOrDefault();

                if (existing != null)
                {
                    existing.GradeValue = dlg.ResultGradeValue;
                    existing.IsDeleted = false;
                    existing.CreatedAt = DateTime.Now;
                    _context.SaveChanges();
                }
                else
                {
                    var grade = new Grade
                    {
                        StudentId = dlg.ResultStudentId,
                        SubjectId = dlg.ResultSubjectId,
                        GradeValue = dlg.ResultGradeValue,
                        CreatedAt = DateTime.Now,
                        IsDeleted = false
                    };
                    _context.Grades.Add(grade);
                    _context.SaveChanges();
                }

                LoadTeacherData();
            }
        }

        private void EditGrade_Click(object sender, RoutedEventArgs e)
        {
            dynamic sel = GradesGrid.SelectedItem;
            if (sel == null)
            {
                MessageBox.Show("Выберите оценку для изменения.");
                return;
            }

            int gradeId = sel.Id;
            var grade = _context.Grades.Where(g => g.Id == gradeId).FirstOrDefault();
            if (grade == null)
            {
                MessageBox.Show("Оценка не найдена.");
                return;
            }

            var dlg = new GradeEditor(App.User.Id, isEdit: true, existingGrade: grade);
            if (dlg.ShowDialog() == true)
            {
                grade.GradeValue = dlg.ResultGradeValue;
                grade.IsDeleted = false;
                _context.SaveChanges();
                LoadTeacherData();
            }
        }

        private void DeleteGrade_Click(object sender, RoutedEventArgs e)
        {
            dynamic sel = GradesGrid.SelectedItem;
            if (sel == null)
            {
                MessageBox.Show("Выберите оценку для удаления.");
                return;
            }

            int gradeId = sel.Id;
            var grade = _context.Grades.Where(g => g.Id == gradeId).FirstOrDefault();
            if (grade == null)
            {
                MessageBox.Show("Оценка не найдена.");
                return;
            }

            grade.IsDeleted = true;
            _context.SaveChanges();
            LoadTeacherData();
        }

        private void RestoreGrade_Click(object sender, RoutedEventArgs e)
        {
            dynamic sel = GradesGrid.SelectedItem;
            if (sel == null)
            {
                MessageBox.Show("Выберите оценку для восстановления.");
                return;
            }

            int gradeId = sel.Id;
            var grade = _context.Grades.Where(g => g.Id == gradeId).FirstOrDefault();
            if (grade == null)
            {
                MessageBox.Show("Оценка не найдена.");
                return;
            }

            grade.IsDeleted = false;
            _context.SaveChanges();
            LoadTeacherData();
        }
    }
}
