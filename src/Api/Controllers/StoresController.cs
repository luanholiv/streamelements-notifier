using System.Net;
using Api.UseCases.Stores.Add;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("v1/stores")]
public sealed class StoresController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddAsync(
        [FromBody] AddStoreRequest requestBody,
        [FromServices] IAddStoreUseCase useCase
    )
    {
        var response = await useCase.ExecuteAsync(requestBody);

        return response.StatusCode switch
        {
            HttpStatusCode.Created => Created($"v1/stores/{response.Result}", null),
            HttpStatusCode.UnprocessableEntity => UnprocessableEntity(response.Errors),
            _ => Problem(statusCode: response.StatusCode.GetHashCode())
        };
    }
}
