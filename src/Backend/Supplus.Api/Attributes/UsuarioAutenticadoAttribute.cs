using Microsoft.AspNetCore.Mvc;
using Supplus.Api.Filters;

namespace Supplus.Api.Attributes;

public class UsuarioAutenticadoAttribute : TypeFilterAttribute
{
    public UsuarioAutenticadoAttribute() : base(typeof(UsuarioAutenticadoFilter))
    {
    }
}
