using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PAWCP2.Models.Models;

namespace PAWCP2.Api.Jwt
{
    public interface IJwtTokenService
    {
        string Create(User user, IEnumerable<string> roles, out DateTime expiresAt);
    }
}