using PAWCP2.Models.Models;
using PAWCP2.Core.Repositories;
using PAWCP2.Core.Manager.Interfaces;

namespace PAWCP2.Core.Manager
{
    public class UserRoleManager : IUserRoleManager
    {
        private readonly IUserRoleRepository _userRoleRepository;

        public UserRoleManager(IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        public async Task<IEnumerable<UserRole>> GetAllWithUsersAndRolesAsync()
        {
            return await _userRoleRepository.GetAllWithUsersAndRolesAsync();
        }
    }
}
