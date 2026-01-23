using Microsoft.EntityFrameworkCore;
using Nedelyaeva.Models;

namespace Nedelyaeva.Data {
    public sealed class studContext: DbContext {
        private static readonly studContext _instance = new studContext();

        public static studContext Instance => _instance;

        // DbSets
        public DbSet<Group> Groups { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Grade> Grades { get; set; }

        private studContext() {
        }

        protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder ) {
            if ( !optionsBuilder.IsConfigured ) {
                // Подключение к локальной БД stud, Trusted Connection (Windows auth)
                optionsBuilder.UseSqlServer( "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=stud" );
            }
        }

        protected override void OnModelCreating( ModelBuilder modelBuilder ) {
            modelBuilder.Entity<Group>().ToTable( "Groups" );
            modelBuilder.Entity<User>().ToTable( "Users" );
            modelBuilder.Entity<Subject>().ToTable( "Subjects" );
            modelBuilder.Entity<Grade>().ToTable( "Grades" );

            modelBuilder.Entity<User>()
                .HasOne<Group>()
                .WithMany()
                .HasForeignKey( u => u.GroupId )
                .OnDelete( DeleteBehavior.Restrict );

            modelBuilder.Entity<Subject>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey( s => s.TeacherId )
                .OnDelete( DeleteBehavior.Restrict );

            modelBuilder.Entity<Grade>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey( g => g.StudentId )
                .OnDelete( DeleteBehavior.Restrict );

            modelBuilder.Entity<Grade>()
                .HasOne<Subject>()
                .WithMany()
                .HasForeignKey( g => g.SubjectId )
                .OnDelete( DeleteBehavior.Restrict );

            base.OnModelCreating( modelBuilder );
        }
    }
}