using System;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using FreeSql;
using Admin.Core.Common;
using Admin.Core.Extensions;
using Admin.Core.Model.Output;


namespace Admin.Core.Aop
{
    public class TransactionInterceptor : IInterceptor
    {
        private readonly IUnitOfWork _unitOfWork;
        public TransactionInterceptor(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            if (method.HasAttribute<TransactionAttribute>())
            {
                try
                {
                    _unitOfWork.Open();
                    invocation.Proceed();

                    if (method.IsAsync())
                    {
                        if (invocation.Method.ReturnType == typeof(Task))
                        {
                            await (Task)invocation.ReturnValue;
                            _unitOfWork.Commit();
                        }
                        else
                        {
                            AopHelper.CallGenericMethod(
                            invocation,
                            res => 
                            {
                                if (res == null)
                                {
                                    return;
                                }

                                var responseOutput = res as IResponseOutput;
                                if (responseOutput != null && !responseOutput.Success)
                                {
                                    _unitOfWork.Rollback();
                                }
                                _unitOfWork.Commit();
                            },
                            ex =>
                            {
                                _unitOfWork.Rollback();
                            });
                        }
                    }
                    else
                    {
                        if (invocation.Method.ReturnType != typeof(void))
                        {
                            var responseOutput = invocation.ReturnValue as Task<IResponseOutput>;
                            if (responseOutput != null && !responseOutput.Result.Success)
                            {
                                _unitOfWork.Rollback();
                            }
                            _unitOfWork.Commit();
                        }
                    }
                }
                catch (Exception ex)
                {
                    _unitOfWork.Rollback();
                    throw ex;
                }
            }
            else
            {
                invocation.Proceed();
            }
        }
    }

}
