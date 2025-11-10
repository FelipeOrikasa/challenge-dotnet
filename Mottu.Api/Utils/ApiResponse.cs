using System.Net;

namespace Mottu.Api.Utils
{
    /// <summary>
    /// Classe genérica para padronizar respostas da API (sucesso ou erro).
    /// </summary>
    /// <typeparam name="T">O tipo de dado retornado em caso de sucesso.</typeparam>
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public bool IsSuccess => StatusCode >= 200 && StatusCode < 300;
        public string? ErrorMessage { get; set; }
        public T? Data { get; set; }

        public ApiResponse(int statusCode, T data)
        {
            StatusCode = statusCode;
            Data = data;
        }

        public ApiResponse(int statusCode, string errorMessage)
        {
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
        }
    }
}