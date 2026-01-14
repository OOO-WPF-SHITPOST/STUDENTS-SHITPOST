using System;
using System.Linq;
using System.Windows;
using Nedelyaeva.Models;
using Nedelyaeva.Data;

namespace Nedelyaeva
{
    public partial class GradeEditor : Window
    {
        public int ResultStudentId { get; private set; }
        public int ResultSubjectId { get; private set; }
        public int? ResultGradeValue { get; private set; }

        public GradeEditor(int teacherId, bool isEdit = false, Grade existingGrade = null)
        {
            InitializeComponent();

            // Fill students (all students)
            var students = studContext.Instance.Users
                .Where(u => u.Role == "student")
                .Select(u => new { u.Id, Display = u.LastName + " " + u.FirstName })
                .ToList();
            StudentCombo.ItemsSource = students;

            // Fill subjects for this teacher
            var subjects = studContext.Instance.Subjects
                .Where(s => s.TeacherId == teacherId)
                .Select(s => new { s.Id, s.Name })
                .ToList();
            SubjectCombo.ItemsSource = subjects;

            if (isEdit && existingGrade != null)
            {
                // preselect
                StudentCombo.SelectedItem = students.Where(x => x.Id == existingGrade.StudentId).FirstOrDefault();
                SubjectCombo.SelectedItem = subjects.Where(x => x.Id == existingGrade.SubjectId).FirstOrDefault();
                GradeValueBox.Text = existingGrade.GradeValue?.ToString() ?? "";
                StudentCombo.IsEnabled = false;
                SubjectCombo.IsEnabled = false;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            dynamic student = StudentCombo.SelectedItem;
            dynamic subject = SubjectCombo.SelectedItem;

            if (student == null || subject == null)
            {
                MessageBox.Show("Выберите студента и дисциплину.");
                return;
            }

            if (!int.TryParse(GradeValueBox.Text.Trim(), out int g) || g < 1 || g > 5)
            {
                MessageBox.Show("Оценка должна быть числом от 1 до 5.");
                return;
            }

            ResultStudentId = student.Id;
            ResultSubjectId = subject.Id;
            ResultGradeValue = g;

            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}