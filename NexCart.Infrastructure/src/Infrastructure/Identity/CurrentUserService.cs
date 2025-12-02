using Microsoft.AspNetCore.Http;
using NexCart.Application.Common.Interfaces;
using System.Security.Claims;

namespace NexCart.Infrastructure.Identity;

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => _httpContextAccessor.HttpContext?
        .User?
        .FindFirstValue(ClaimTypes.NameIdentifier);

    public string? Email => _httpContextAccessor.HttpContext?
        .User?
        .FindFirstValue(ClaimTypes.Email);

    public string? Name => _httpContextAccessor.HttpContext?
        .User?
        .FindFirstValue(ClaimTypes.Name);

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?
        .User?
        .Identity?
        .IsAuthenticated ?? false;

    public bool IsInRole(string role)
    {
        if (string.IsNullOrWhiteSpace(role))
            return false;

        return _httpContextAccessor.HttpContext?
            .User?
            .IsInRole(role) ?? false;
    }

    public IEnumerable<string> GetRoles()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user is null)
            return Enumerable.Empty<string>();

        return user.FindAll(ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();
    }
}