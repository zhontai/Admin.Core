using System;
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

        public async void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            
            if (method.HasAttribute<TransactionAttribute>())
            {
                try
                {
                    var transaction = method.GetAttribute<TransactionAttribute>();
                    _unitOfWork = _unitOfWorkManager.Begin(transaction.Propagation, transaction.IsolationLevel);
                    invocation.Proceed();

                    if (method.IsAsync())
                    {
                        if (invocation.Method.ReturnType == typeof(Task))
                        {
                            try
                            {
                                await (Task)invocation.ReturnValue;
                                _unitOfWork.Commit();
                            }
                            catch (Exception ex)
                            {
                                _unitOfWork.Rollback();
                                throw ex;
                            }
                            finally
                            {
                                _unitOfWork.Dispose();
                            }
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
                                else
                                {
                                    _unitOfWork.Commit();
                                }
                            },
                            ex =>
                            {
                                _unitOfWork.Rollback();
                            },
                            ()=>
                            {
                                _unitOfWork.Dispose();
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
                            else
                            {
                                _unitOfWork.Commit();
                            }
                        }
                        else
                        {
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
