using Supplus.Domain.Services.Tokens;

namespace Supplus.Api.Tokens;

public class HttpContextTokenValue : ITokenProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextTokenValue(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string ObterToken()
    {
        var auhtorization = _httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString();
        return auhtorization["Bearer ".Length..].Trim();
    }
}
