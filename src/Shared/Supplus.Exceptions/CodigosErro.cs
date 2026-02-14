namespace Supplus.Exceptions;

/// <summary> 
/// Fornece um conjunto centralizado de códigos de erro utilizados em toda a aplicação. 
/// Esses códigos representam cenários comuns de falha, como recurso não encontrado, 
/// conflitos, erros de validação, credenciais inválidas, acesso não autorizado,
/// erros internos do servidor e sem permissão de acesso. 
/// </summary>
public class CodigosErro
{
    public static string NaoEncontrado = "NaoEncontrado";
    public static string Conflito = "Conflito";
    public static string ErroDeValidacao = "ErroDeValidacao";
    public static string CredencialInvalida = "CredencialInvalida";
    public static string NaoAutorizado = "NaoAutorizado";
    public static string ErroIternoServidor = "ErroIternoServidor";
    public static string SemPermissao = "SemPermissao";
}
