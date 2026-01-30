namespace Supplus.Comunicacao.Responses;

public record ResponseErroJson
{
    public List<string> Mensagens { get; private set; }    

    public ResponseErroJson(List<string> mensagens)
    {
        Mensagens = mensagens;
    }

    public ResponseErroJson(string mensagem)
    {
        Mensagens = [mensagem];
    }
}
