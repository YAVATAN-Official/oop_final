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
            modelBuilder.Entity<StudentField>().HasKey(k => new { k.UserId, k.FieldId });
            modelBuilder.Entity<TeacherField>().HasKey(k => new { k.UserId, k.FieldId });
            modelBuilder.Entity<FieldCourses>().HasKey(k => new { k.CourseId, k.FieldId });
            modelBuilder.Entity<UserTags>().HasKey(k => new { k.UserId, k.TagId });
        }
    }
}
