using Castle.DynamicProxy;

namespace ZhonTai.Plate.Admin.Domain
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