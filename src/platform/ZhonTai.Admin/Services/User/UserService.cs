using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Configs;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Common.Helpers;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Admin.Domain.Api;
using ZhonTai.Admin.Domain.PermissionApi;
using ZhonTai.Admin.Domain.Role;
using ZhonTai.Admin.Domain.RolePermission;
using ZhonTai.Admin.Domain.Tenant;
using ZhonTai.Admin.Domain.User;
using ZhonTai.Admin.Domain.UserRole;
using ZhonTai.Admin.Services.Auth.Dto;
using ZhonTai.Admin.Services.User.Dto;
using ZhonTai.DynamicApi;
using ZhonTai.DynamicApi.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ZhonTai.Admin.Core.Helpers;
using ZhonTai.Admin.Core.Consts;

namespace ZhonTai.Admin.Services.User
{
    /// <summary>
    /// 用户服务
    /// </summary>
    [DynamicApi(Area = "admin")]
    public class UserService : BaseService, IUserService, IDynamicApi
    {
        private readonly AppConfig _appConfig;
        private readonly IUserRepository _userRepository;
        private readonly IRepositoryBase<UserRoleEntity> _userRoleRepository;
        private readonly ITenantRepository _tenantRepository;
        private readonly IApiRepository _apiRepository;

        private IRoleRepository _roleRepository => LazyGetRequiredService<IRoleRepository>();

        public UserService(
            AppConfig appConfig,
            IUserRepository userRepository,
            IRepositoryBase<UserRoleEntity> userRoleRepository,
            ITenantRepository tenantRepository,
            IApiRepository apiRepository
        )
        {
            _appConfig = appConfig;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _tenantRepository = tenantRepository;
            _apiRepository = apiRepository;
        }

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IResultOutput> GetAsync(long id)
        {
            var entity = await _userRepository.Select
            .WhereDynamic(id)
            .IncludeMany(a => a.Roles.Select(b => new RoleEntity { Id = b.Id }))
            .ToOneAsync();

            var roles = await _roleRepository.Select.ToListAsync(a => new { a.Id, a.Name });

            return ResultOutput.Ok(new { Form = Mapper.Map<UserGetOutput>(entity), Select = new { roles } });
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IResultOutput> GetPageAsync(PageInput input)
        {
            var list = await _userRepository.Select
            .WhereDynamicFilter(input.DynamicFilter)
            .Count(out var total)
            .OrderByDescending(true, a => a.Id)
            .IncludeMany(a => a.Roles.Select(b => new RoleEntity { Name = b.Name }))
            .Page(input.CurrentPage, input.PageSize)
            .ToListAsync();

            var data = new PageOutput<UserListOutput>()
            {
                List = Mapper.Map<List<UserListOutput>>(list),
                Total = total
            };

            return ResultOutput.Ok(data);
        }

        /// <summary>
        /// 查询登录用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResultOutput<AuthLoginOutput>> GetLoginUserAsync(long id)
        {
            var output = new ResultOutput<AuthLoginOutput>();
            var entityDto = await _userRepository.Select.DisableGlobalFilter("Tenant").WhereDynamic(id).ToOneAsync<AuthLoginOutput>();
            if (_appConfig.Tenant && entityDto?.TenantId.Value > 0)
            {
                var tenant = await _tenantRepository.Select.DisableGlobalFilter("Tenant").WhereDynamic(entityDto.TenantId).ToOneAsync(a => new { a.TenantType, a.DataIsolationType });
                if (null != tenant)
                {
                    entityDto.TenantType = tenant.TenantType;
                    entityDto.DataIsolationType = tenant.DataIsolationType;
                }
            }
            return output.Ok(entityDto);
        }

        /// <summary>
        /// 查询下拉数据
        /// </summary>
        /// <returns></returns>
        public async Task<IResultOutput> GetSelectAsync()
        {
            var roles = await _roleRepository.Select.ToListAsync(a => new { a.Id, a.Name });

            return ResultOutput.Ok(new { Select = new { roles } });
        }

        /// <summary>
        /// 查询用户基本信息
        /// </summary>
        /// <returns></returns>
        public async Task<IResultOutput> GetBasicAsync()
        {
            if (!(User?.Id > 0))
            {
                return ResultOutput.NotOk("未登录！");
            }

            var data = await _userRepository.GetAsync<UserUpdateBasicInput>(User.Id);
            return ResultOutput.Ok(data);
        }

        /// <summary>
        /// 查询用户权限信息
        /// </summary>
        /// <returns></returns>
        public async Task<IList<UserPermissionsOutput>> GetPermissionsAsync()
        {
            var key = string.Format(CacheKeys.UserPermissions, User.Id);
            var result = await Cache.GetOrSetAsync(key, async () =>
            {
                return await _apiRepository
                .Where(a => _userRoleRepository.Orm.Select<UserRoleEntity, RolePermissionEntity, PermissionApiEntity>()
                .InnerJoin((b, c, d) => b.RoleId == c.RoleId && b.UserId == User.Id)
                .InnerJoin((b, c, d) => c.PermissionId == d.PermissionId)
                .Where((b, c, d) => d.ApiId == a.Id).Any())
                .ToListAsync<UserPermissionsOutput>();
            });
            return result;
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Transaction]
        public async Task<IResultOutput> AddAsync(UserAddInput input)
        {
            if (input.Password.IsNull())
            {
                input.Password = "111111";
            }

            input.Password = MD5Encrypt.Encrypt32(input.Password);

            var entity = Mapper.Map<UserEntity>(input);
            var user = await _userRepository.InsertAsync(entity);

            if (!(user?.Id > 0))
            {
                return ResultOutput.NotOk();
            }

            if (input.RoleIds != null && input.RoleIds.Any())
            {
                var roles = input.RoleIds.Select(a => new UserRoleEntity { UserId = user.Id, RoleId = a });
                await _userRoleRepository.InsertAsync(roles);
            }

            return ResultOutput.Ok();
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Transaction]
        public async Task<IResultOutput> UpdateAsync(UserUpdateInput input)
        {
            if (!(input?.Id > 0))
            {
                return ResultOutput.NotOk();
            }

            var user = await _userRepository.GetAsync(input.Id);
            if (!(user?.Id > 0))
            {
                return ResultOutput.NotOk("用户不存在！");
            }

            Mapper.Map(input, user);
            await _userRepository.UpdateAsync(user);

            await _userRoleRepository.DeleteAsync(a => a.UserId == user.Id);

            if (input.RoleIds != null && input.RoleIds.Any())
            {
                var roles = input.RoleIds.Select(a => new UserRoleEntity { UserId = user.Id, RoleId = a });
                await _userRoleRepository.InsertAsync(roles);
            }

            return ResultOutput.Ok();
        }

        /// <summary>
        /// 更新用户基本信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IResultOutput> UpdateBasicAsync(UserUpdateBasicInput input)
        {
            var entity = await _userRepository.GetAsync(input.Id);
            entity = Mapper.Map(input, entity);
            var result = (await _userRepository.UpdateAsync(entity)) > 0;

            //清除用户缓存
            await Cache.DelAsync(string.Format(CacheKeys.UserInfo, input.Id));

            return ResultOutput.Result(result);
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IResultOutput> ChangePasswordAsync(UserChangePasswordInput input)
        {
            if (input.ConfirmPassword != input.NewPassword)
            {
                return ResultOutput.NotOk("新密码和确认密码不一致！");
            }

            var entity = await _userRepository.GetAsync(input.Id);
            var oldPassword = MD5Encrypt.Encrypt32(input.OldPassword);
            if (oldPassword != entity.Password)
            {
                return ResultOutput.NotOk("旧密码不正确！");
            }

            input.Password = MD5Encrypt.Encrypt32(input.NewPassword);

            entity = Mapper.Map(input, entity);
            var result = (await _userRepository.UpdateAsync(entity)) > 0;

            return ResultOutput.Result(result);
        }

        /// <summary>
        /// 彻底删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IResultOutput> DeleteAsync(long id)
        {
            var result = false;
            if (id > 0)
            {
                result = (await _userRepository.DeleteAsync(m => m.Id == id)) > 0;
            }

            return ResultOutput.Result(result);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Transaction]
        public async Task<IResultOutput> SoftDeleteAsync(long id)
        {
            var result = await _userRepository.SoftDeleteAsync(id);
            await _userRoleRepository.DeleteAsync(a => a.UserId == id);

            return ResultOutput.Result(result);
        }

        /// <summary>
        /// 批量删除用户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [Transaction]
        public async Task<IResultOutput> BatchSoftDeleteAsync(long[] ids)
        {
            var result = await _userRepository.SoftDeleteAsync(ids);
            await _userRoleRepository.DeleteAsync(a => ids.Contains(a.UserId));

            return ResultOutput.Result(result);
        }

        /// <summary>
        /// 上传头像
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Login]
        public async Task<IResultOutput> AvatarUpload([FromForm] IFormFile file)
        {
            var uploadConfig = LazyGetRequiredService<IOptionsMonitor<UploadConfig>>().CurrentValue;
            var uploadHelper = LazyGetRequiredService<UploadHelper>();
            var config = uploadConfig.Avatar;
            var res = await uploadHelper.UploadAsync(file, config, new { User.Id });
            if (res.Success)
            {
                return ResultOutput.Ok(res.Data.FileRelativePath);
            }

            return ResultOutput.NotOk(res.Msg ?? "上传失败！");
        }
    }
}