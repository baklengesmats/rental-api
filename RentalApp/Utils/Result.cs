namespace RentalApp.Utils
{
    public class Result<T>
    {
        public T Value { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
        public int ErrorCode { get; set; }
        public static Result<T> SuccessResult(T value) => new Result<T> { Value = value, Success = true };
        public static Result<T> FailureResult(string error, int? errorCode) => new Result<T> { Success = false, Error = error, ErrorCode =  errorCode ?? 500 };
    }

    public static class Result
    {
        public static Result<T> Success<T>(T value) => Result<T>.SuccessResult(value);
        public static Result<T> Failure<T>(string error, int? errorCode) => Result<T>.FailureResult(error, errorCode);
    }
}
