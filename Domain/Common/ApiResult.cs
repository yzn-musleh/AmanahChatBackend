using Domain.Enums;

namespace Domain.Common
{
    public class ApiResult<T>
    {
        public ErrorCodeEnum ErrorCode { get; set; } = ErrorCodeEnum.None;

        public ErrorCodeLevelEnum ErrorCodeLevel { get; set; } = ErrorCodeLevelEnum.Success;

        public string Message { get; set; } = "success";

        public T? Result { get; set; }

        public ApiResult<T> CreateSuccess(T? result = default)
        {
            ErrorCode = ErrorCodeEnum.None;
            ErrorCodeLevel = ErrorCodeLevelEnum.Success;
            Result = result;
            Message = "success";
            return this;
        }

        public ApiResult<T> CreateError(ErrorCodeEnum errorCode, string message)
        {
            ErrorCode = errorCode;
            ErrorCodeLevel = ErrorCodeLevelEnum.Error;
            Message = message;
            return this;
        }

        public ApiResult<T> CreateWarnning(ErrorCodeEnum errorCode, string message, T? result = default)
        {
            ErrorCode = errorCode;
            ErrorCodeLevel = ErrorCodeLevelEnum.Warnning;
            Result = result;
            Message = message;
            return this;
        }
    }
}
