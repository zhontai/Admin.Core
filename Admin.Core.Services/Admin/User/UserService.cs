using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using AutoMapper;
using Admin.Core.Common.Helpers;
using Admin.Core.Common.Auth;
using Admin.Core.Common.Cache;
using Admin.Core.Common.Input;
using Admin.Core.Common.Output;
using Admin.Core.Model.Admin;
using Admin.Core.Repository.Admin;
using Admin.Core.Service.Admin.User.Input;
using Admin.Core.Service.Admin.User.Output;
using Admin.Core.Common.Attributes;
using Admin.Core.Service.Admin.Auth.Output;

namespace Admin.Core.Service.Admin.User
{
    /// <summary>
    /// 用户服务
    /// </summary>	
    public class UserService : IUserService
    {
        private readonly IUser _user;
        private readonly ICache _cache;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;

        public UserService(
            IUser user,
            ICache cache,
            IMapper mapper,
            IUserRepository userRepository,
            IUserRoleRepository userRoleRepository,
            IRolePermissionRepository rolePermissionRepository
        )
        {
            _user = user;
            _cache = cache;
            _mapper = mapper;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _rolePermissionRepository = rolePermissionRepository;
        }

        public async Task<ResponseOutput<AuthLoginOutput>> GetLoginUserAsync(long id)
        {
            var output = new ResponseOutput<AuthLoginOutput>();
            var entityDto = await _userRepository.GetAsync<AuthLoginOutput>(id);
            return output.Ok(entityDto);
        }

        public async Task<ResponseOutput<UserGetOutput>> GetAsync(long id)
        {
            var res = new ResponseOutput<UserGetOutput>();

            var entity = await _userRepository.Select
            .WhereDynamic(id)
            .IncludeMany(a => a.Roles.Select(b => new RoleEntity { Id = b.Id }))
            .ToOneAsync();

            var entityDto = _mapper.Map<UserGetOutput>(entity);
            return res.Ok(entityDto);
        }

        public async Task<IResponseOutput> GetBasicAsync()
        {
            if (!(_user?.Id > 0))
            {
                return ResponseOutput.NotOk("未登录！");
            }

            var data = await _userRepository.GetAsync<UserUpdateBasicInput>(_user.Id);
            return ResponseOutput.Ok(data);
        }

        public async Task<IList<UserPermissionsOutput>> GetPermissionsAsync()
        {
            var key = string.Format(CacheKey.UserPermissions, _user.Id);
            if (await _cache.ExistsAsync(key))
            {
                try
                {
                    return await _cache.GetAsync<IList<UserPermissionsOutput>>(key);
                }
                catch
                {
                    await _cache.DelByPatternAsync("admin:user:{0}:permissions");
                }
            }

            var userPermissoins = await _rolePermissionRepository.Select
                .InnerJoin<UserRoleEntity>((a, b) => a.RoleId == b.RoleId && b.UserId == _user.Id && a.Permission.Type == PermissionType.Api)
                .Include(a => a.Permission.Api)
                .Distinct()
                .ToListAsync(a => new UserPermissionsOutput { HttpMethods = a.Permission.Api.HttpMethods, Path = a.Permission.Api.Path });

            await _cache.SetAsync(key, userPermissoins);

            return userPermissoins;
        }

        public async Task<IResponseOutput> PageAsync(PageInput<UserEntity> input)
        {
            var list = await _userRepository.Select
            .WhereDynamicFilter(input.DynamicFilter)
            .Count(out var total)
            .OrderByDescending(true, a => a.Id)
            .IncludeMany(a => a.Roles.Select(b => new RoleEntity{ Name = b.Name }))
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync();

            var data = new PageOutput<UserListOutput>()
            {
                List = _mapper.Map<List<UserListOutput>>(list),
                Total = total
            };

            return ResponseOutput.Ok(data);
        }

        [Transaction]
        public async Task<IResponseOutput> AddAsync(UserAddInput input)
        {
            if (input.Password.IsNull())
            {
                input.Password = "111111";
            }

            input.Password = MD5Encrypt.Encrypt32(input.Password);

            var entity = _mapper.Map<UserEntity>(input);
            var user = await _userRepository.InsertAsync(entity);

            if (!(user?.Id > 0))
            {
                return ResponseOutput.NotOk();
            }

            if (input.RoleIds != null && input.RoleIds.Any())
            {
                var roles = input.RoleIds.Select(a => new UserRoleEntity { UserId = user.Id, RoleId = a });
                await _userRoleRepository.InsertAsync(roles);
            }

            return ResponseOutput.Ok();
        }

        [Transaction]
        public async Task<IResponseOutput> UpdateAsync(UserUpdateInput input)
        {
            if (!(input?.Id > 0))
            {
                return ResponseOutput.NotOk();
            }

            var user = await _userRepository.GetAsync(input.Id);
            if (!(user?.Id > 0))
            {
                return ResponseOutput.NotOk("用户不存在！");
            }

            _mapper.Map(input, user);
            await _userRepository.UpdateAsync(user);
            await _userRoleRepository.DeleteAsync(a => a.UserId == user.Id);
            if (input.RoleIds != null && input.RoleIds.Any())
            {
                var roles = input.RoleIds.Select(a => new UserRoleEntity { UserId = user.Id, RoleId = a });
                await _userRoleRepository.InsertAsync(roles);
            }

            return ResponseOutput.Ok();
        }

        public async Task<IResponseOutput> UpdateBasicAsync(UserUpdateBasicInput input)
        {
            var entity = await _userRepository.GetAsync(input.Id);
            entity = _mapper.Map(input, entity);
            var result = (await _userRepository.UpdateAsync(entity)) > 0;

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> ChangePasswordAsync(UserChangePasswordInput input)
        {
            if (input.ConfirmPassword != input.NewPassword)
            {
                return ResponseOutput.NotOk("新密码和确认密码不一致！");
            }

            var entity = await _userRepository.GetAsync(input.Id);
            var oldPassword = MD5Encrypt.Encrypt32(input.OldPassword);
            if (oldPassword != entity.Password)
            {
                return ResponseOutput.NotOk("旧密码不正确！");
            }

            input.Password = MD5Encrypt.Encrypt32(input.NewPassword);

            entity = _mapper.Map(input, entity);
            var result = (await _userRepository.UpdateAsync(entity)) > 0;

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> DeleteAsync(long id)
        {
            var result = false;
            if (id > 0)
            {
                result = (await _userRepository.DeleteAsync(m => m.Id == id)) > 0;
            }

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> SoftDeleteAsync(long id)
        {
            var result = await _userRepository.SoftDeleteAsync(id);

            return ResponseOutput.Result(result);
        }

        public async Task<IResponseOutput> BatchSoftDeleteAsync(long[] ids)
        {
            var result = await _userRepository.SoftDeleteAsync(ids);

            return ResponseOutput.Result(result);
        }
    }
}
