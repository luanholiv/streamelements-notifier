using System.Net;

namespace Api.UseCases;

public sealed record HttpUseCaseResponse<T>(
    HttpStatusCode StatusCode,
    T Result,
    IEnumerable<ErrorMessage> Errors);