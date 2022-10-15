using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using FreeSql;
using ZhonTai.Admin.Core.Attributes;
using ZhonTai.Admin.Core.Dto;

namespace ZhonTai.Admin.Core.Db.Transaction;

public class TransactionAsyncInterceptor : IAsyncInterceptor
{
    private IUnitOfWork _unitOfWork;
    private readonly UnitOfWorkManagerCloud _unitOfWorkManagerCloud;
    private readonly FreeSqlCloud _freeSqlCloud;
    private readonly IServiceProvider _serviceProvider;

    public TransactionAsyncInterceptor(UnitOfWorkManagerCloud unitOfWorkManagerCloud, FreeSqlCloud freeSqlCloud, IServiceProvider serviceProvider)
    {
        _unitOfWorkManagerCloud = unitOfWorkManagerCloud;
        _freeSqlCloud = freeSqlCloud;
        _serviceProvider = serviceProvider;
    }

    private bool TryBegin(IInvocation invocation)
    {
        var method = invocation.MethodInvocationTarget ?? invocation.Method;
        var attribute = method.GetCustomAttributes(typeof(TransactionAttribute), false).FirstOrDefault();
        if (attribute is TransactionAttribute transaction)
        {
            IsolationLevel? isolationLevel = transaction.IsolationLevel == 0 ? null : transaction.IsolationLevel;

            _unitOfWork = _unitOfWorkManagerCloud.Begin(_freeSqlCloud.GetDbKey(_serviceProvider), transaction.Propagation, isolationLevel);
            return true;
        }

        return false;
    }

    private async Task InternalInterceptAsynchronous(IInvocation invocation)
    {
        //string methodName =
        //    $"{invocation.MethodInvocationTarget.DeclaringType?.FullName}.{invocation.Method.Name}()";
        //int? hashCode = _unitOfWork.GetHashCode();

        invocation.Proceed();

        try
        {
            //处理Task返回一个null值的情况会导致空指针
            if (invocation.ReturnValue != null)
            {
                await (Task)invocation.ReturnValue;
            }
            _unitOfWork.Commit();
        }
        catch (System.Exception)
        {
            _unitOfWork.Rollback();
            throw;
        }
        finally
        {
            _unitOfWork.Dispose();
        }
    }

    private async Task<TResult> InternalInterceptAsynchronous<TResult>(IInvocation invocation)
    {
        TResult result;
        if (TryBegin(invocation))
        {
            try
            {
                invocation.Proceed();
                result = await (Task<TResult>)invocation.ReturnValue;
                if (result is IResultOutput res && !res.Success)
                {
                    _unitOfWork.Rollback();
                }
                else
                {
                    _unitOfWork.Commit();
                }
            }
            catch (System.Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }
        else
        {
            invocation.Proceed();
            result = await (Task<TResult>)invocation.ReturnValue;
        }
        return result;
    }

    /// <summary>
    /// 拦截同步执行的方法
    /// </summary>
    /// <param name="invocation"></param>
    public void InterceptSynchronous(IInvocation invocation)
    {
        if (TryBegin(invocation))
        {
            try
            {
                invocation.Proceed();
                var result = invocation.ReturnValue;
                if (result is IResultOutput res && !res.Success)
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
                throw;
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }
        else
        {
            invocation.Proceed();
        }
    }

    /// <summary>
    /// 拦截返回结果
    /// </summary>
    /// <param name="invocation"></param>
    public void InterceptAsynchronous(IInvocation invocation)
    {
        if (TryBegin(invocation))
        {
            invocation.ReturnValue = InternalInterceptAsynchronous(invocation);
        }
        else
        {
            invocation.Proceed();
        }
    }

    /// <summary>
    /// 拦截返回结果
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="invocation"></param>
    public void InterceptAsynchronous<TResult>(IInvocation invocation)
    {
        invocation.ReturnValue = InternalInterceptAsynchronous<TResult>(invocation);
    }
}