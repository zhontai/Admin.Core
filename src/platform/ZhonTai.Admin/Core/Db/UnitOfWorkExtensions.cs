using DotNetCore.CAP;
using DotNetCore.CAP.Transport;
using FreeSql;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ZhonTai.Admin.Core.Db
{
    public class FreeSqlRepositoryPatternTransaction : CapTransactionBase
    {
        public FreeSqlRepositoryPatternTransaction(IDispatcher dispatcher, IUnitOfWork uow) : base(dispatcher)
        {
            Uow = uow;
        }

        public IUnitOfWork Uow { get; }

        public override object? DbTransaction => Uow.GetOrBeginTransaction();

        public override void Commit()
        {
            Uow.Commit();
            Flush();
        }

        public override Task CommitAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override void Rollback()
        {
            Uow.Rollback();
        }

        public override Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            Uow.Dispose();
        }
    }

    public static class UnitOfWorkExtensions
    {
        /// <summary>
        /// 开启Cap分布式事务
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="capPublisher"></param>
        /// <param name="autoCommit"></param>
        /// <remarks>
        /// using var uow = LazyGetRequiredService&lt;UnitOfWorkManagerCloud&gt;().Begin(DbKeys.AppDb);<br/>
        /// using var capTran = uow.BeginCapTran(LazyGetRequiredService&lt;ICapPublisher&gt;(), false);<br/>
        /// capTran.Commit();
        /// </remarks>
        /// <returns></returns>
        public static ICapTransaction BeginCapTran(this IUnitOfWork unitOfWork, ICapPublisher capPublisher, bool autoCommit = false)
        {
            var dispatcher = capPublisher.ServiceProvider.GetRequiredService<IDispatcher>();
            var transaction = new FreeSqlRepositoryPatternTransaction(dispatcher, unitOfWork)
            {
                AutoCommit = autoCommit
            };
            return capPublisher.Transaction.Value = transaction;
        }
    }
}
