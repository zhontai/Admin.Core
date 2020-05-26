using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using FreeSql;
using Admin.Core.Common.Extensions;
using Admin.Core.Common.Output;
using Admin.Core.Common.Attributes;


namespace Admin.Core.Aop
{
    public class TransactionInterceptor : IInterceptor
    {
        IUnitOfWork _unitOfWork;
        private readonly UnitOfWorkManager _unitOfWorkManager;
        
        public TransactionInterceptor(UnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        public void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            if (method.HasAttribute<TransactionAttribute>())
            {
                InterceptTransaction(invocation, method);
            }
            else
            {
                invocation.Proceed();
            }
        }

        private async void InterceptTransaction(IInvocation invocation, MethodInfo method)
        {
            try
            {
                var transaction = method.GetAttribute<TransactionAttribute>();
                _unitOfWork = _unitOfWorkManager.Begin(transaction.Propagation, transaction.IsolationLevel);
                invocation.Proceed();

                dynamic returnValue = invocation.ReturnValue;
                if (returnValue is Task)
                {
                    returnValue = await returnValue;
                }

                if (returnValue is IResponseOutput res && !res.Success)
                {
                    _unitOfWork.Rollback();
                }
                else
                {
                    _unitOfWork.Commit();
                }
            }
            catch
            {
                _unitOfWork.Rollback();
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }
    }

}
