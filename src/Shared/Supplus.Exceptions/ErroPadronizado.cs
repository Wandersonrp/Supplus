using Supplus.Exceptions.Mensagens;

namespace Supplus.Exceptions;

/// <summary>
/// Representa um erro padronizado na aplicação.
/// Contém um código identificador, uma mensagem opcional e uma lista de mensagens adicionais,
/// permitindo uniformizar o tratamento de erros e facilitar a comunicação entre camadas.
/// </summary>
/// <param name="Codigo">
/// Código único que identifica o tipo de erro (ex.: NaoEncontrado, Conflito, ErroDeValidacao).
/// </param>
/// <param name="Mensagem">
/// Mensagem descritiva opcional que fornece detalhes adicionais sobre o erro.
/// </param>
/// <param name="Mensagens">
/// Lista opcional de mensagens adicionais, geralmente utilizada para agrupar múltiplos erros de validação.
/// </param>
public sealed record class ErroPadronizado(string Codigo, string? Mensagem = null, List<string>? Mensagens = null)
{
    private static readonly string NaoEncontrado = CodigosErro.NaoEncontrado;
    private static readonly string Conflito = CodigosErro.Conflito;
    private static readonly string ErroDeValidacao = CodigosErro.ErroDeValidacao;
    private static readonly string CredencialInvalida = CodigosErro.CredencialInvalida;
    private static readonly string NaoAutorizado = CodigosErro.NaoAutorizado;
    private static readonly string ErroInternoServidor = CodigosErro.ErroIternoServidor;

    public static readonly ErroPadronizado Nenhum = new(string.Empty, string.Empty, new List<string>());

    public static ErroPadronizado NaoEncontradoErro(string mensagem) => new ErroPadronizado(NaoEncontrado, mensagem);

    public static ErroPadronizado ConflitoErro(string mensagem) => new ErroPadronizado(Conflito, mensagem);

    public static ErroPadronizado CredencialInvalidaErro()
    {
        return new ErroPadronizado(CredencialInvalida, Mensagem: MensagensErro.CREDENCIAIS_INVALIDAS);
    }

    public static ErroPadronizado ErroDeValidacaoMensagens(List<string>? mensagens)
    {
        return new ErroPadronizado(ErroDeValidacao, Mensagens: mensagens);
    }

    public static ErroPadronizado NaoAutorizadoErro(string? mensagem = null) => new ErroPadronizado(NaoAutorizado, Mensagem: mensagem);

    public static ErroPadronizado ErroInternoServidorErro() => new ErroPadronizado(ErroInternoServidor, Mensagem: MensagensErro.ERRO_DESCONHECIDO);
}

