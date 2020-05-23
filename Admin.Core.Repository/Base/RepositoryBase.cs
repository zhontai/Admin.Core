

using System;
using System.Threading.Tasks;
using System.Linq.Expressions;
using FreeSql;
using Admin.Core.Common.Auth;

namespace Admin.Core.Repository
{
    public abstract class RepositoryBase<TEntity,TKey> : BaseRepository<TEntity, TKey> where TEntity : class,new()
    {
        private readonly IUser _user;
        protected RepositoryBase(UnitOfWorkManager uowm, IUser user) : base(uowm.Orm, null, null)
        {
            uowm.Binding(this);
            _user = user;
        }
        
        public virtual Task<TDto> GetAsync<TDto>(TKey id)
        {
            return Select.WhereDynamic(id).ToOneAsync<TDto>();
        }

        public virtual Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> exp)
        {
            return Select.Where(exp).ToOneAsync();
        }

        public virtual Task<TDto> GetAsync<TDto>(Expression<Func<TEntity, bool>> exp)
        {
            return Select.Where(exp).ToOneAsync<TDto>();
        }

        public async Task<bool> SoftDeleteAsync(TKey id)
        {
            await UpdateDiy
                .SetDto(new { 
                    IsDeleted = true, 
                    ModifiedUserId = _user.Id, 
                    ModifiedUserName = _user.Name 
                })
                .WhereDynamic(id)
                .ExecuteAffrowsAsync();
            return true;
        }

        public async Task<bool> SoftDeleteAsync(TKey[] ids)
        {
            await UpdateDiy
                .SetDto(new { 
                    IsDeleted = true, 
                    ModifiedUserId = _user.Id, 
                    ModifiedUserName = _user.Name 
                })
                .WhereDynamic(ids)
                .ExecuteAffrowsAsync();
            return true;
        }
    }

    public abstract class RepositoryBase<TEntity> : RepositoryBase<TEntity, long> where TEntity : class, new()
    {
        protected RepositoryBase(UnitOfWorkManager uowm, IUser user) : base(uowm, user)
        {
        }
    }
}
