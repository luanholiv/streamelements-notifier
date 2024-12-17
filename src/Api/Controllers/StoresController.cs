using Api.UseCases;
using System.Net;
using Api.UseCases.Stores.Add;
using Api.UseCases.Stores.Delete;
using Api.UseCases.Stores.Search;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("v1/stores")]
public sealed class StoresController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddAsync(
        [FromBody] AddStoreRequest request,
        [FromServices] IAddStoreUseCase useCase
    )
    {
        var response = await useCase.ExecuteAsync(request);

        return response.StatusCode switch
        {
            HttpStatusCode.Created => Created($"v1/stores/{response.Result}", null),
            HttpStatusCode.UnprocessableEntity => UnprocessableEntity(response.Errors),
            _ => Problem(statusCode: response.StatusCode.GetHashCode())
        };
    }

    [HttpDelete("/{id:guid}")]
    public async Task<IActionResult> DeleteAsync(
        Guid id,
        [FromServices] IDeleteStoreUseCase useCase
    )
    {
        var request = new DeleteStoreRequest(id);
        var response = await useCase.ExecuteAsync(request);

        return response.StatusCode switch
        {
            HttpStatusCode.OK => Ok(),
            HttpStatusCode.UnprocessableEntity => UnprocessableEntity(response.Errors),
            _ => Problem(statusCode: response.StatusCode.GetHashCode())
        };
    }

    [HttpGet]
    public async Task<IActionResult> SearchAsync(
        int pageNumber,
        int pageSize,
        [FromServices] ISearchStoresUseCase useCase
        )
    {
        var request = new PagedRequest(pageNumber, pageSize);
        var response = await useCase.ExecuteAsync(request);
        
        return response.StatusCode switch
        {
            HttpStatusCode.OK => Ok(response.Result),
            HttpStatusCode.UnprocessableEntity => UnprocessableEntity(response.Errors),
            _ => Problem(statusCode: response.StatusCode.GetHashCode())
        };
    }
}
