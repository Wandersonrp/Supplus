using Microsoft.AspNetCore.Identity;
using Supplus.Domain.Entities;

namespace UtilitariosCompartilhados.Tests.Builders.Services;

public class PasswordHasherBuilder
{
    public static IPasswordHasher<Usuario> Build()
    {
        return new PasswordHasher<Usuario>();
    }
}
