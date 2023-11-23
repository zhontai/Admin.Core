using System;
#if NET7_0
using System.Runtime.Serialization;
#endif

namespace ZhonTai.Admin.Core.Exceptions;

/// <summary>
/// 系统异常
/// </summary>
public class AppException : Exception
{
    public string AppMessage { get; set; }
    public string AppCode { get; set; }

    public AppException()
    {
    }

#if NET7_0
    public AppException(SerializationInfo serializationInfo, StreamingContext context)
        : base(serializationInfo, context)
    {
    }
#endif

    public AppException(string message)
        : base(message)
    {
        AppMessage= message;
    }

    public AppException(string message, string code)
     : base(message)
    {
        AppMessage= message;
        AppCode= code;
    }


    public AppException(string message, Exception innerException)
        : base(message, innerException)
    {
        AppMessage= message;
    }

    public AppException(string message, string code, Exception innerException)
        : base(message, innerException)
    {
        AppMessage = message;
        AppCode = code;
    }
}
    