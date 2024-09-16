namespace navami.Dto
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }

        public ApiResponse()
        {
            Success = true; 
        }

        public ApiResponse(T data)
        {
            Success = true;
            Data = data;
        }

        public ApiResponse(string errorMessage)
        {
            Success = false;
            ErrorMessage = errorMessage;
        }
    }
}
