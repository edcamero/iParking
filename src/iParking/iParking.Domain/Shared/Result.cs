namespace iParking.Domain.Shared
{
    /// <summary>
    /// Representa el resultado de una operación que puede fallar.
    /// Implementa el patrón Result para manejo elegante de errores.
    /// </summary>
    public class Result<T>
    {
        public bool IsSuccess { get; private set; }
        public bool IsFailure => !IsSuccess;
        public T? Data { get; private set; }
        public string Error { get; private set; } = string.Empty;
        public int Code { get; private set; }

        private Result(bool isSuccess, T? data, string error, int code)
        {
            IsSuccess = isSuccess;
            Data = data;
            Error = error;
            Code = code;
        }

        public static Result<T> Success(T data, int code = 200)
            => new Result<T>(true, data, string.Empty, code);

        public static Result<T> Failure(string error, int code = 400)
            => new Result<T>(false, default, error, code);

        public static Result<T> NotFound(string error = "Recurso no encontrado", int code = 404)
            => new Result<T>(false, default, error, code);

        public static Result<T> Conflict(string error, int code = 409)
            => new Result<T>(false, default, error, code);
    }

    /// <summary>
    /// Clase base para resultados sin datos.
    /// </summary>
    public class Result
    {
        public bool IsSuccess { get; private set; }
        public bool IsFailure => !IsSuccess;
        public string Error { get; private set; } = string.Empty;
        public int Code { get; private set; }

        protected Result(bool isSuccess, string error, int code)
        {
            IsSuccess = isSuccess;
            Error = error;
            Code = code;
        }

        public static Result Success(int code = 200)
            => new Result(true, string.Empty, code);

        public static Result Failure(string error, int code = 400)
            => new Result(false, error, code);

        public static Result NotFound(string error = "Recurso no encontrado", int code = 404)
            => new Result(false, error, code);

        public static Result Conflict(string error, int code = 409)
            => new Result(false, error, code);
    }
}
