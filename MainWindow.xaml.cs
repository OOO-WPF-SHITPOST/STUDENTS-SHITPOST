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
        private User _currentUser;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var login = LoginTextBox.Text.Trim();
            var pass = PasswordBox.Password;

            _currentUser = studContext.Instance.Users
                .Where(x => x.Login == login && x.Password == pass && x.IsDeleted == false)
                .FirstOrDefault();

            if (_currentUser == null)
            {
                MessageBox.Show("Неверные учетные данные.");
                return;
            }

            if (_currentUser.Role == "teacher")
            {
                ShowTeacherView();
            }
            else
            {
                ShowStudentView();
            }
        }

        private void ShowTeacherView()
        {
            LoginPanel.Visibility = Visibility.Collapsed;
            StudentPanel.Visibility = Visibility.Collapsed;
            TeacherPanel.Visibility = Visibility.Visible;
            LoadTeacherData();
        }

        private void ShowStudentView()
        {
            LoginPanel.Visibility = Visibility.Collapsed;
            TeacherPanel.Visibility = Visibility.Collapsed;
            StudentPanel.Visibility = Visibility.Visible;
            LoadStudentData();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            _currentUser = null;
            LoginTextBox.Text = "";
            PasswordBox.Password = "";
            TeacherPanel.Visibility = Visibility.Collapsed;
            StudentPanel.Visibility = Visibility.Collapsed;
            LoginPanel.Visibility = Visibility.Visible;
        }

        private void LoadTeacherData()
        {
            // Subjects for which teacher is responsible
            var subjects = studContext.Instance.Subjects
                .Where(s => s.TeacherId == _currentUser.Id)
                .Select(s => new { s.Id, s.Name, s.IsDeleted })
                .ToList();
            SubjectsGrid.ItemsSource = subjects;

            // Students in teacher's groups: we consider students that are in groups of subjects' students
            var students = studContext.Instance.Users
                .Where(u => u.Role == "student")
                .Select(u => new
                {
                    u.Id,
                    u.LastName,
                    u.FirstName,
                    GroupName = studContext.Instance.Groups.Where(g => g.Id == u.GroupId).Select(g => g.Name).FirstOrDefault()
                })
                .ToList();
            StudentsGrid.ItemsSource = students;

            // Grades for subjects of this teacher (including deleted to allow restore)
            var grades = studContext.Instance.Grades
                .Where(g => studContext.Instance.Subjects.Where(s => s.TeacherId == _currentUser.Id).Select(s => s.Id).Contains(g.SubjectId))
                .Select(g => new
                {
                    g.Id,
                    StudentName = studContext.Instance.Users.Where(u => u.Id == g.StudentId).Select(u => u.LastName + " " + u.FirstName).FirstOrDefault(),
                    SubjectName = studContext.Instance.Subjects.Where(s => s.Id == g.SubjectId).Select(s => s.Name).FirstOrDefault(),
                    g.GradeValue,
                    g.CreatedAt,
                    g.IsDeleted
                })
                .ToList();
            GradesGrid.ItemsSource = grades;
        }

        private void LoadStudentData()
        {
            var myGrades = studContext.Instance.Grades
                .Where(g => g.StudentId == _currentUser.Id)
                .Select(g => new
                {
                    SubjectName = studContext.Instance.Subjects.Where(s => s.Id == g.SubjectId).Select(s => s.Name).FirstOrDefault(),
                    g.GradeValue,
                    g.CreatedAt,
                    g.IsDeleted
                })
                .ToList();
            MyGradesGrid.ItemsSource = myGrades;
        }

        private void Teacher_Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadTeacherData();
        }

        private void Student_Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadStudentData();
        }

        private void AddGrade_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new GradeEditor(_currentUser.Id, isEdit: false);
            if (dlg.ShowDialog() == true)
            {
                // при добавлении учитываем уникальность student+subject
                var existing = studContext.Instance.Grades
                    .Where(g => g.StudentId == dlg.ResultStudentId && g.SubjectId == dlg.ResultSubjectId)
                    .FirstOrDefault();

                if (existing != null)
                {
                    existing.GradeValue = dlg.ResultGradeValue;
                    existing.IsDeleted = false;
                    existing.CreatedAt = DateTime.Now;
                    studContext.Instance.SaveChanges();
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
                    studContext.Instance.Grades.Add(grade);
                    studContext.Instance.SaveChanges();
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
            var grade = studContext.Instance.Grades.Where(g => g.Id == gradeId).FirstOrDefault();
            if (grade == null)
            {
                MessageBox.Show("Оценка не найдена.");
                return;
            }

            var dlg = new GradeEditor(_currentUser.Id, isEdit: true, existingGrade: grade);
            if (dlg.ShowDialog() == true)
            {
                grade.GradeValue = dlg.ResultGradeValue;
                grade.IsDeleted = false;
                studContext.Instance.SaveChanges();
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
            var grade = studContext.Instance.Grades.Where(g => g.Id == gradeId).FirstOrDefault();
            if (grade == null)
            {
                MessageBox.Show("Оценка не найдена.");
                return;
            }

            grade.IsDeleted = true;
            studContext.Instance.SaveChanges();
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
            var grade = studContext.Instance.Grades.Where(g => g.Id == gradeId).FirstOrDefault();
            if (grade == null)
            {
                MessageBox.Show("Оценка не найдена.");
                return;
            }

            grade.IsDeleted = false;
            studContext.Instance.SaveChanges();
            LoadTeacherData();
        }
    }
}