using System;
using System.ComponentModel.DataAnnotations;

namespace ZhonTai.Admin.Core.Validators
{
    /// <summary>
    /// 指定属性、字段、参数必填
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
    public class ValidateRequiredAttribute : ValidationAttribute
    {
        public ValidateRequiredAttribute() : base("{0} 为必填项") { }

        public ValidateRequiredAttribute(string errorMessage) : base(errorMessage) { }

        public override bool IsValid(object value)
        {
            if (value is null)
            {
                return false;
            }

            var valid  = value switch
            {
                Guid guid => guid != Guid.Empty,
                long longValue => longValue > 0,
                int intValue => intValue > 0,
                string strValue => strValue.NotNull(),
                _ => true
            };

            return valid;
        }
    }
}
