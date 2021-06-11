using Admin.Core.Common.Auth;
using FreeSql;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Admin.Core.Repository
{
    public abstract class RepositoryBase<TEntity, TKey> : BaseRepository<TEntity, TKey>, IRepositoryBase<TEntity, TKey> where TEntity : class, new()
    {
        public IUser User { get; set; }

        protected RepositoryBase(IFreeSql freeSql) : base(freeSql, null, null)
        {
        }

        public virtual Task<TDto> GetAsync<TDto>(TKey id)
        {
            return Select.WhereDynamic(id).ToOneAsync<TDto>();
        }

        public virtual Task<TDto> GetAsync<TDto>(Expression<Func<TEntity, bool>> exp)
        {
            return Select.Where(exp).ToOneAsync<TDto>();
        }

        public virtual Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> exp)
        {
            return Select.Where(exp).ToOneAsync();
        }

        public async Task<bool> SoftDeleteAsync(TKey id)
        {
            await UpdateDiy
                .SetDto(new
                {
                    IsDeleted = true,
                    ModifiedUserId = User.Id,
                    ModifiedUserName = User.Name
                })
                .WhereDynamic(id)
                .ExecuteAffrowsAsync();
            return true;
        }

        public async Task<bool> SoftDeleteAsync(Expression<Func<TEntity, bool>> exp, params string[] name)
        {
            await UpdateDiy
                .SetDto(new
                {
                    IsDeleted = true,
                    ModifiedUserId = User.Id,
                    ModifiedUserName = User.Name
                })
                .Where(exp)
                .DisableGlobalFilter(name)
                .ExecuteAffrowsAsync();
            return true;
        }

        public async Task<bool> SoftDeleteAsync(TKey[] ids)
        {
            await UpdateDiy
                .SetDto(new
                {
                    IsDeleted = true,
                    ModifiedUserId = User.Id,
                    ModifiedUserName = User.Name
                })
                .WhereDynamic(ids)
                .ExecuteAffrowsAsync();
            return true;
        }
    }

    public abstract class RepositoryBase<TEntity> : RepositoryBase<TEntity, long> where TEntity : class, new()
    {
        protected RepositoryBase(MyUnitOfWorkManager muowm) : base(muowm.Orm)
        {
            muowm.Binding(this);
        }
    }
}