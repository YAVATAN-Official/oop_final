using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gis_final.Models
{
    public class YasharDbContext : DbContext
    {
        public YasharDbContext(DbContextOptions<YasharDbContext> options) : base(options)
        {

        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<FieldCourses> FieldCourses { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<StudentConselor> StudentConselors { get; set; }
        public DbSet<StudentDegree> StudentDegrees { get; set; }
        public DbSet<StudentField> StudentFields { get; set; }
        public DbSet<StudentGraduationStatus> StudentGraduationStatuses { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TeacherCourseResearchAssistant> TeacherCourseResearchAssistants { get; set; }
        public DbSet<TeacherField> TeacherFields { get; set; }
        public DbSet<TeacherFieldCourse> TeacherFieldCourses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<UserTags> UserTags { get; set; }
        public DbSet<YearTerm> YearTerms { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRoles>().HasKey(k => new { k.UserId, k.RoleId });

            modelBuilder.Entity<UserRoles>()
                .HasOne(bc => bc.User)
                .WithMany(b => b.UserRoles)
                .HasForeignKey(bc => bc.UserId);

            modelBuilder.Entity<UserRoles>()
                .HasOne(bc => bc.Role)
                .WithMany(c => c.UserRoles)
                .HasForeignKey(bc => bc.RoleId);


            modelBuilder.Entity<StudentConselor>().HasKey(k => new { k.UserId });
            modelBuilder.Entity<User>()
                .HasOne(a => a.StudentConselor)
                .WithOne(b => b.User)
                .HasForeignKey<StudentConselor>(b => b.UserId);

            modelBuilder.Entity<StudentField>().HasKey(k => new { k.UserId, k.FieldId });
            modelBuilder.Entity<TeacherField>().HasKey(k => new { k.UserId, k.FieldId });
            modelBuilder.Entity<FieldCourses>().HasKey(k => new { k.Id, k.CourseId, k.FieldId });
            modelBuilder.Entity<UserTags>().HasKey(k => new { k.UserId, k.TagId });

            modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Email = "yashar", Password = "123" },
                new User { Id = 2, Email = "safak", Password = "123" },
                new User { Id = 3, Email = "nese", Password = "123" },
                new User { Id = 4, Email = "defne", Password = "123" }
                );

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Title = "Admin", Create = true, Delete = true, Confirm = true, View = true, Update = true },
                new Role { Id = 2, Title = "Teacher", Create = true, Delete = true, Confirm = true, View = true, Update = true },
                new Role { Id = 3, Title = "Assistant", Create = true, Delete = true, Confirm = true, View = true, Update = true },
                new Role { Id = 4, Title = "Student", Create = true, Delete = true, Confirm = true, View = true, Update = true }
                );

            modelBuilder.Entity<UserRoles>().HasData(
                new UserRoles { UserId = 1, RoleId = 1, Create = true, Delete = true, Confirm = true, View = true, Update = true },
                new UserRoles { UserId = 2, RoleId = 2, Create = true, Delete = true, Confirm = true, View = true, Update = true },
                new UserRoles { UserId = 3, RoleId = 3, Create = true, Delete = true, Confirm = true, View = true, Update = true },
                new UserRoles { UserId = 4, RoleId = 4, Create = true, Delete = true, Confirm = true, View = true, Update = true }
                );

            modelBuilder.Entity<Tag>().HasData(
                new Tag { Id = 1, Title = "Profesör" },
                new Tag { Id = 2, Title = "Döçent" },
                new Tag { Id = 3, Title = "Dr. Öğr. Üyesi" },
                new Tag { Id = 4, Title = "Araştırma Görevlisi" }
                );
        }
    }
}
