using MyFinances.Domain.DTOs.Link;

namespace MyFinances.API.Services.Interfaces;

public interface ILinkService
{
    LinkDTO Generate(string endpointName, object? routeValues, string rel, string method);
}