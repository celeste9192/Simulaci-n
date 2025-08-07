using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PAWCP2.Models.Models;

namespace PAWCP2.Models.ViewModels
{
    public class RoleViewModel
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; } = null!;

        public string? Description { get; set; }

        public virtual ICollection<FoodItem> FoodItems { get; set; } = new List<FoodItem>();

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
}
