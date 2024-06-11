using talentX.WebScrapper.Allabolog.Entities;

namespace talentX.WebScrapper.Allabolog.Utils
{
    public static class ResponseUtils
    {

        public static ApiResponseDto<T> GetSuccesfulResponse<T>(T data)
        {
            return new()
            {
                Data = data,
                isSuccess = true,
            };
        }

        public static ApiResponseDto<T> GetBadRequestResponse<T>(T data)
        {
            return new()
            {
                Data = data,
                isSuccess = false,
            };
        }

    }
}
