using Castle.DynamicProxy;

namespace Admin.Core.Aop
{
    public class TransactionInterceptor : IInterceptor
    {
        private readonly TransactionAsyncInterceptor _transactionAsyncInterceptor;

        public TransactionInterceptor(TransactionAsyncInterceptor transactionAsyncInterceptor)
        {
            _transactionAsyncInterceptor = transactionAsyncInterceptor;
        }

        public void Intercept(IInvocation invocation)
        {
            _transactionAsyncInterceptor.ToInterceptor().Intercept(invocation);
        }
    }
}
