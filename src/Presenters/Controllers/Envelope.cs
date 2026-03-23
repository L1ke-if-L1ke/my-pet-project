using System.Net;
namespace Presenters.Common;

/// <summary>
/// Универсальная обёртка для всех ответов API.
/// </summary>
/// <typeparam name="T">Тип данных в результате</typeparam>
public class Envelope<T>
{
    public int Status { get; set; }
    public T? Result { get; set; }
    public string? Error { get; set; }

    public Envelope(T? result, int status = 200)
    {
        Status = status;
        Result = result;
    }

    public Envelope(int status, string error)
    {
        Status = status;
        Error = error;
    }

    public static Envelope<T> Ok(T? data) => new(data, (int)HttpStatusCode.OK);
    public static Envelope<T> ErrorResponse(HttpStatusCode status, string message) => new((int)status, message);
}

/// <summary>
/// Non-generic обёртка для ответов без данных
/// </summary>
public class Envelope : Envelope<object?>
{
    public Envelope(object? result, int status = 200) : base(result, status) { }
    public Envelope(int status, string error) : base(status, error) { }

    public new static Envelope Ok(object? data) => new(data, (int)HttpStatusCode.OK);
    public new static Envelope ErrorResponse(HttpStatusCode status, string message) => new ((int)status, message);
}