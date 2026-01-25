using Microsoft.AspNetCore.Identity;
using Supplus.Comunicacao.Requests.Auth;
using Supplus.Comunicacao.Responses.Auth;
using Supplus.Comunicacao.Validators.Auth.Login;
using Supplus.Domain.Entities;
using Supplus.Domain.Repositories;
using Supplus.Exceptions;

namespace Supplus.Application.UseCases.Auth.Login;

public class LoginUseCase : BaseUseCase<RequestLoginJson, LoginValidator>, ILoginUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IPasswordHasher<Usuario> _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public LoginUseCase(
        IUsuarioRepository usuarioRepository, 
        IPasswordHasher<Usuario> passwordHasher, 
        IUnitOfWork unitOfWork)
    {
        _usuarioRepository = usuarioRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }
     
    public async Task<ResultadoPersonalizado<ResponseLoginJson>> Executar(RequestLoginJson request)
    {        
        var resultado = Validar(request);

        if(resultado != ErroPadronizado.Nenhum)
            return ResultadoPersonalizado<ResponseLoginJson>.Falha(resultado);        

        var usuario = await _usuarioRepository.ObterPorEmailAsync(request.Email!);

        if(usuario is null)
            return ResultadoPersonalizado<ResponseLoginJson>.Falha(ErroPadronizado.CredencialInvalidaErro());

        var resultadoSenha = _passwordHasher.VerifyHashedPassword(usuario, usuario.ObterSenha(), request.Senha!);

        if(resultadoSenha == PasswordVerificationResult.Failed)
            return ResultadoPersonalizado<ResponseLoginJson>.Falha(ErroPadronizado.CredencialInvalidaErro());                

        // Simula a geração de tokens
        var accessToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        var refreshToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

        var token = new ResponseTokenJson(accessToken, refreshToken);

        await _unitOfWork.CommitAsync();

        return ResultadoPersonalizado<ResponseLoginJson>.Sucesso(new ResponseLoginJson(token));
    }
}
