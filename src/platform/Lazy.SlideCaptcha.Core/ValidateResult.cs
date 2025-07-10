using System;
using System.Collections.Generic;
using System.Text;

namespace Lazy.SlideCaptcha.Core
{
    public class ValidateResult
    {
        public ValidateResultType Result { get; set; }
        public string Message { get; set; }

        public static ValidateResult Success()
        {
            return new ValidateResult { Result = ValidateResultType.Success, Message = "成功" };
        }

        public static ValidateResult Fail()
        {
            return new ValidateResult { Result = ValidateResultType.ValidateFail, Message = "验证失败" };
        }

        public static ValidateResult Timeout()
        {
            return new ValidateResult { Result = ValidateResultType.Timeout, Message = "验证超时" };
        }

        public enum ValidateResultType
        {
            Success = 0,
            ValidateFail = 1,
            Timeout = 2
        }
    }
}
