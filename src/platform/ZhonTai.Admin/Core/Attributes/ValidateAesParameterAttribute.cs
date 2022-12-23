using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Text;
using ZhonTai.Admin.Core.Dto;
using ZhonTai.Common.Helpers;

namespace ZhonTai.Admin.Core.Attributes;

/// <summary>
/// AES加密输入模型验证
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class ValidateAesParameterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        //参数验证
        string requestBody = "";

        var request = context.HttpContext.Request;
        request.Body.Position = 0;
        StreamReader stream = new StreamReader(request.Body);
        requestBody = stream.ReadToEnd();
        request.Body.Position = 0;

        if (requestBody.Length > 0)
        {
            //object args = context.ActionArguments.FirstOrDefault(x => x.Key == context.ActionDescriptor.Parameters[0].Name).Value;

            //AES解密
            QueryInput queryresult = JsonConvert.DeserializeObject<QueryInput>(requestBody);
            if (queryresult.UserName.IsNull() || queryresult.Message.IsNull())
            {
                context.Result = new JsonResult(ResultOutput.NotOk("参数不正确！"));
                return;
            }
            //获取key
            //var permissionHandler = context.HttpContext.RequestServices.GetService<>();
            string key = "";
            var paramDecrypt = AESEncrypt.DecryptStringFromString(queryresult.Message, key);
            var param = JsonConvert.DeserializeObject(paramDecrypt, context.ActionArguments.FirstOrDefault(x => x.Key == context.ActionDescriptor.Parameters[0].Name).Value.GetType());


            var list = context.ActionDescriptor.Parameters;
            //string obj = context.ActionArguments[list[0].Name].ToString();
            context.ActionArguments[list[0].Name] = param;
        }
    }
}