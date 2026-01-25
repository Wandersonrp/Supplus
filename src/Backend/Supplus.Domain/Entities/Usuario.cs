using Supplus.Domain.Enums;

namespace Supplus.Domain.Entities;

public sealed class Usuario : EntidadeBase
{
    public string Email { get; private set; }
    public string Nome { get; private set; }
    public string Sobrenome { get; private set; }
    public string NomeCompleto { get; }
    public Role Role { get; private set; }

    private string _senha;
    
    public Usuario(string email, string nome, string sobrenome, Role? role = null)
    {
        _senha = string.Empty;
        
        Email = email;
        Nome = nome;
        Sobrenome = sobrenome;
        NomeCompleto = ObterNomeCompleto();        
        Role = role ?? Role.UsuarioComum;
    }

    public void AtribuirSenha(string senha)
    {
        if(!string.IsNullOrEmpty(senha))
            _senha = senha;
    }
    
    public string ObterSenha() => _senha;

    public string ObterNomeCompleto() => $"{Nome} {Sobrenome}";
}
