using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAWCP2.Models.Models;

namespace PAWCP2.Models.ViewModels
{
    public class UserRoleViewModel
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }

        public string? Description { get; set; }

        public virtual Role Role { get; set; } = null!;

        public virtual User User { get; set; } = null!;
    }
}
