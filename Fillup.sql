USE [stud]
GO

/* =========================
   Groups
   ========================= */
INSERT INTO dbo.Groups (name, is_deleted)
VALUES
('Group A', 0),
('Group B', 0),
('Group C', 0);

/* =========================
   Users
   ========================= */
/*
id:
1 - Teacher
2 - Student
3 - Student
*/
INSERT INTO dbo.Users
(group_id, last_name, first_name, middle_name, role, login, password, is_deleted, created_at)
VALUES
(NULL, 'Smith', 'John', NULL, 'teacher', 'john.smith', 'password123', 0, GETDATE()),
(1, 'Brown', 'Alice', NULL, 'student', 'alice.brown', 'password123', 0, GETDATE()),
(2, 'Wilson', 'Bob', NULL, 'student', 'bob.wilson', 'password123', 0, GETDATE());

/* =========================
   Subjects
   ========================= */
/*
All subjects are taught by teacher with id = 1
*/
INSERT INTO dbo.Subjects (name, teacher_id, is_deleted)
VALUES
('Mathematics', 1, 0),
('Physics', 1, 0),
('Computer Science', 1, 0);

/* =========================
   Grades
   ========================= */
/*
Unique (student_id, subject_id)
grade_value must be between 1 and 5
*/
INSERT INTO dbo.Grades
(student_id, subject_id, grade_value, created_at, is_deleted)
VALUES
(2, 1, 5, GETDATE(), 0),  -- Alice - Mathematics
(2, 2, 4, GETDATE(), 0),  -- Alice - Physics
(3, 3, 3, GETDATE(), 0);  -- Bob - Computer Science
GO
