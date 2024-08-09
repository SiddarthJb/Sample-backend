using Z1.Core.Enums;

namespace Z1.Core
{
    public class BaseResponse<T>
    {
        public BaseResponse()
        {
        }

        public BaseResponse(T data, string message = null)
        {
            this.Data = data;
            this.Message = message;
        }

        public BaseResponse(string error, int status, List<string>? errors)
        {
            this.Status = status;
            this.Message = error;
            this.Errors = errors;
        }

        public BaseResponse(T data, string error, List<string> errors, int status)
        {
            this.Status = status;
            this.Message = error;
            this.Errors = errors;
            this.Data = data;
        }
        public int Status {   get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
        public List<string>? Errors { get; set; } = new List<string>();
    }
}
