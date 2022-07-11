
namespace ServIT.Models
{
    public class Role
    {
        public Guid Id { get; set; }
        public Guid RoleName { get; set; }
        public enum Roles { User, Agent, Admin, SuperAdmin }; // predefined roles
        public Roles RoleType { get; set; }
    }
}
