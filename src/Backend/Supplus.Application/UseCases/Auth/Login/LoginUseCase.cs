using Supplus.Comunicacao.Requests.Auth;
using Supplus.Comunicacao.Responses.Auth;

namespace Supplus.Application.UseCases.Auth.Login;

public class LoginUseCase
{
    public LoginUseCase()
    {
    }
     
    public ResponseLoginJson Executar(RequestLoginJson request)
    {
        // Lógica de autenticação simulada para o exemplo
        if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Senha))
            throw new ArgumentException("Email e senha são obrigatórios.");

        if (request.Email != "admin@admin.com" || request.Senha != "12345678")
            throw new ArgumentException("E-mail ou senha incorretos.");

        // Simula a geração de tokens
        var accessToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        var refreshToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

        var token = new ResponseTokenJson(accessToken, refreshToken);

        return new ResponseLoginJson(token);
    }
}
