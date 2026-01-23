USE [stud]
GO
/****** Object:  Table [dbo].[Grades]    Script Date: 21.01.2026 19:20:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Grades](
  [id] [int] IDENTITY(1,1) NOT NULL,
  [student_id] [int] NOT NULL,
  [subject_id] [int] NOT NULL,
  [grade_value] [int] NULL,
  [created_at] [datetime] NOT NULL,
  [is_deleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
  [id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Student_Subject] UNIQUE NONCLUSTERED 
(
  [student_id] ASC,
  [subject_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Groups]    Script Date: 21.01.2026 19:20:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Groups](
  [id] [int] IDENTITY(1,1) NOT NULL,
  [name] [nvarchar](50) NOT NULL,
  [is_deleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
  [id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
  [name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subjects]    Script Date: 21.01.2026 19:20:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subjects](
  [id] [int] IDENTITY(1,1) NOT NULL,
  [name] [nvarchar](100) NOT NULL,
  [teacher_id] [int] NOT NULL,
  [is_deleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
  [id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 21.01.2026 19:20:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
  [id] [int] IDENTITY(1,1) NOT NULL,
  [group_id] [int] NULL,
  [last_name] [nvarchar](100) NOT NULL,
  [first_name] [nvarchar](100) NOT NULL,
  [middle_name] [nvarchar](100) NULL,
  [role] [nvarchar](20) NOT NULL,
  [login] [nvarchar](50) NOT NULL,
  [password] [nvarchar](255) NOT NULL,
  [is_deleted] [bit] NOT NULL,
  [created_at] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
  [id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
  [login] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Grades] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[Grades] ADD  DEFAULT ((0)) FOR [is_deleted]
GO
ALTER TABLE [dbo].[Groups] ADD  DEFAULT ((0)) FOR [is_deleted]
GO
ALTER TABLE [dbo].[Subjects] ADD  DEFAULT ((0)) FOR [is_deleted]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [is_deleted]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[Grades]  WITH CHECK ADD  CONSTRAINT [FK_Grades_Students] FOREIGN KEY([student_id])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[Grades] CHECK CONSTRAINT [FK_Grades_Students]
GO
ALTER TABLE [dbo].[Grades]  WITH CHECK ADD  CONSTRAINT [FK_Grades_Subjects] FOREIGN KEY([subject_id])
REFERENCES [dbo].[Subjects] ([id])
GO
ALTER TABLE [dbo].[Grades] CHECK CONSTRAINT [FK_Grades_Subjects]
GO
ALTER TABLE [dbo].[Subjects]  WITH CHECK ADD  CONSTRAINT [FK_Subjects_Teachers] FOREIGN KEY([teacher_id])
REFERENCES [dbo].[Users] ([id])
GO
ALTER TABLE [dbo].[Subjects] CHECK CONSTRAINT [FK_Subjects_Teachers]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Group] FOREIGN KEY([group_id])
REFERENCES [dbo].[Groups] ([id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Group]
GO
ALTER TABLE [dbo].[Grades]  WITH CHECK ADD CHECK  (([grade_value]>=(1) AND [grade_value]<=(5)))
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD CHECK  (([role]='student' OR [role]='teacher'))
GO
