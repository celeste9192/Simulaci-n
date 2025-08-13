using System;
using System.Collections.Generic;

namespace PAWCP2.Models.ViewModels
{
    public class UserRolesViewModel
    {
        public List<UserDto> Users { get; set; } = new List<UserDto>();
        public List<RoleDto> Roles { get; set; } = new List<RoleDto>();
        public List<UserRoleMapDto> Map { get; set; } = new List<UserRoleMapDto>();

        // NUEVA: Id del usuario logeado
        public int LoggedUserId { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class UserRoleMapDto
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
