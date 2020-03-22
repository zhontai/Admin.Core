
using FreeSql;
using System.Threading.Tasks;
using Admin.Core.Common.Auth;

namespace Admin.Core.Repository
{
    public abstract class RepositoryBase<TEntity,TKey> : BaseRepository<TEntity, TKey> where TEntity : class,new()
    {
        private readonly IUser _user;
        protected RepositoryBase(IFreeSql orm, IUnitOfWork uow, IUser user) : base(orm, null, null)
        {
            uow.Close();
            UnitOfWork = uow;
            _user = user;
        }

        public virtual Task<TDto> GetAsync<TDto>(TKey id)
        {
            return Select.WhereDynamic(id).ToOneAsync<TDto>();
        }

        public async Task<bool> SoftDeleteAsync(TKey id)
        {
            await UpdateDiy
                .SetDto(new { IsDeleted = true, ModifiedUserId = _user.Id, ModifiedUserName = _user.Name })
                .WhereDynamic(id)
                .ExecuteAffrowsAsync();
            return true;
        }

        public async Task<bool> SoftDeleteAsync(TKey[] ids)
        {
            await UpdateDiy
                .SetDto(new { IsDeleted = true, ModifiedUserId = _user.Id, ModifiedUserName = _user.Name })
                .WhereDynamic(ids)
                .ExecuteAffrowsAsync();
            return true;
        }
    }

    public abstract class RepositoryBase<TEntity> : RepositoryBase<TEntity, long> where TEntity : class, new()
    {
        protected RepositoryBase(IFreeSql orm, IUnitOfWork uow, IUser user) : base(orm, uow, user)
        {
        }
    }
}
