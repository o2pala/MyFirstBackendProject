using Microsoft.AspNetCore.Mvc;

namespace TodoApi.ViewModels
{
    public static class APIResultExtensions
    {
        public static APIResponse<T> APIResult<T>(this ControllerBase controller, Enum status, T data)
        {
            return new APIResponse<T>
            {
                status = (int)(object)status,
                data = data
            };
        }
        public static APIResponse APIResult(this ControllerBase controller, Enum status)
        {
            return new APIResponse
            {
                status = (int)(object)status
            };
        }
    }
}
