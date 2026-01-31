using Microsoft.AspNetCore.Identity;
using Supplus.Domain.Entities;

namespace UtilitariosCompartilhados.Tests.Builders.Services;

public class PasswordHasherBuilder<TEntidade> where TEntidade : EntidadeBase
{
    public static IPasswordHasher<TEntidade> Build()
    {
        return new PasswordHasher<TEntidade>();
    }
}
