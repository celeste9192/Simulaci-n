﻿using System;
using System.Collections.Generic;


namespace PAWCP2.Models.Models;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<FoodItem> FoodItems { get; set; } = new List<FoodItem>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
