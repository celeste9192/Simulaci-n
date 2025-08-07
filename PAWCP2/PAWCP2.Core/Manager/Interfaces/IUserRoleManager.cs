using PAWCP2.Models.Models;

namespace PAWCP2.Core.Manager.Interfaces
{
    public interface IUserRoleManager
    {
        Task<IEnumerable<UserRole>> GetAllWithUsersAndRolesAsync();
    }
}

