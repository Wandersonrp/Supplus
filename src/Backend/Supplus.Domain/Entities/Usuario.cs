namespace Supplus.Domain.Entities;

public sealed class Usuario : EntidadeBase
{
    public string Email { get; private set; }
    public string Nome { get; private set; }
    public string Sobrenome { get; private set; }
    public string NomeCompleto { get; }

    private string _senha;
    
    public Usuario(string email, string nome, string sobrenome)
    {
        _senha = string.Empty;
        
        Email = email;
        Nome = nome;
        Sobrenome = sobrenome;
        NomeCompleto = ObterNomeCompleto();
    }

    public void AtribuirSenha(string senha)
    {
        if(!string.IsNullOrEmpty(senha))
            _senha = senha;
    }
    
    public string ObterSenha() => _senha;

    public string ObterNomeCompleto() => $"{Nome} {Sobrenome}";
}
