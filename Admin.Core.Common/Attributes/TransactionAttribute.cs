using System;
using System.Data;
using FreeSql;

namespace Admin.Core.Common
{
    /// <summary>
    /// 启用事物
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class TransactionAttribute : Attribute
    {
        public Propagation Propagation { get; set; } = Propagation.Requierd;
        public IsolationLevel? IsolationLevel { get; set; }
    }
}
