using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAWCP2.Models.DTOs
{
    public record TokenResponse(string access_token, DateTime expires_at);
}
