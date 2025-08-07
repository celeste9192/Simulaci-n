using PAWCP2.Models.Models;

namespace PAWCP2.Core.Repositories
{
    public interface IUserRoleRepository : IRepositoryBase<UserRole>
    {
        // Método adicional específico
        Task<IEnumerable<UserRole>> GetAllWithUsersAndRolesAsync();
    }
}
