using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gis_final.Models
{
    public class User : BaseEntity
    {
        public enum EnumUserStatus
        {
            registered = 0,
            confirmed = 1,
            banned = 2,
            deleted = 3
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserNumber { get; set; }
        public string IdentityNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public EnumUserStatus UserStatus { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<UserRoles> UserRoles { get; set; }
        public virtual ICollection<UserTags> UserTags { get; set; }
        public virtual ICollection<TeacherField> TeacherFields { get; set; }
        public virtual ICollection<StudentField> StudentFields { get; set; }
        public virtual ICollection<StudentDegree> StudentDegrees { get; set; }

        public virtual StudentConselor StudentConselor { get; set; }
        public virtual StudentGraduationStatus StudentGraduationStatus { get; set; }
    }
}
