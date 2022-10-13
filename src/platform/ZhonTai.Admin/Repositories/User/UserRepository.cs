using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZhonTai.Admin.Core.Db.Transaction;
using ZhonTai.Admin.Core.Repositories;
using ZhonTai.Admin.Domain;
using ZhonTai.Admin.Domain.Org;
using ZhonTai.Admin.Domain.Role;
using ZhonTai.Admin.Domain.User;
using ZhonTai.Admin.Domain.User.Dto;

namespace ZhonTai.Admin.Repositories;

public class UserRepository : RepositoryBase<UserEntity>, IUserRepository
{
    private readonly IOrgRepository _orgRepository;
    private readonly IRepositoryBase<RoleOrgEntity> _roleOrgRepository;
    public UserRepository(
        UnitOfWorkManagerCloud muowm, 
        IOrgRepository orgRepository, 
        IRepositoryBase<RoleOrgEntity> roleOrgRepository
    ) : base(muowm)
    {
        _orgRepository = orgRepository;
        _roleOrgRepository = roleOrgRepository;
    }

    /// <summary>
    /// 获得当前登录用户
    /// </summary>
    /// <returns></returns>
    [NonAction]
    public async Task<CurrentUserDto> GetCurrentUserAsync()
    {
        var user = await Select
        .IncludeMany(a => a.Roles.Select(b => new RoleEntity
        {
            Id = b.Id,
            DataScope = b.DataScope
        }))
        .WhereDynamic(User.Id)
        .ToOneAsync(a => new
        {
            a.OrgId,
            a.Roles
        });

        //数据范围
        DataScope dataScope = DataScope.Self;
        var customRoleIds = new List<long>();
        user.Roles?.ToList().ForEach(role =>
        {
            if (role.DataScope == DataScope.Custom)
            {
                customRoleIds.Add(role.Id);
            }
            else if (role.DataScope <= dataScope)
            {
                dataScope = role.DataScope;
            }
        });

        //部门列表
        var orgIds = new List<long>();
        if (dataScope != DataScope.All)
        {
            //本部门
            if (dataScope == DataScope.Dept)
            {
                orgIds.Add(user.OrgId);
            }
            //本部门和下级部门
            else if (dataScope == DataScope.DeptWithChild)
            {
                orgIds = await _orgRepository
                .Where(a => a.Id == user.OrgId)
                .AsTreeCte()
                .ToListAsync(a => a.Id);
            }

            //指定部门
            if (customRoleIds.Count > 0)
            {
                var customRoleOrgIds = await _roleOrgRepository.Select.Where(a => customRoleIds.Contains(a.RoleId)).ToListAsync(a => a.OrgId);
                orgIds = orgIds.Concat(customRoleOrgIds).ToList();
            }
        }

        return new CurrentUserDto
        {
            OrgId = user.OrgId,
            OrgIds = orgIds.Distinct().ToList(),
            DataScope = dataScope
        };
    }
}