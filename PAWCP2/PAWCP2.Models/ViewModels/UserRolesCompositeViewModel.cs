using System.Collections.Generic;

namespace PAWCP2.Models.ViewModels
{
    public class UserRolesCompositeViewModel
    {
        public List<UserViewModel> Users { get; set; } = new List<UserViewModel>();
        public List<RoleViewModel> Roles { get; set; } = new List<RoleViewModel>();
        public List<UserRoleViewModel> UserRoles { get; set; } = new List<UserRoleViewModel>();
    }
}
