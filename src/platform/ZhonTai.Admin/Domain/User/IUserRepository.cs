using System.Threading.Tasks;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain.User.Dto;

namespace ZhonTai.Admin.Domain.User;

public interface IUserRepository : IRepositoryBase<UserEntity>
{
    Task<CurrentUserDto> GetCurrentUserAsync();
}