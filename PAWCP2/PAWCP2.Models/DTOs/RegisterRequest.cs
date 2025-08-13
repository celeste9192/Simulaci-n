using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAWCP2.Models.DTOs
{
    public class RegisterRequest
    {
       
            public string Username { get; set; } = "";
            public string Email { get; set; } = "";
            public string? FullName { get; set; }
        

    }
}
