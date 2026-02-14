using Supplus.Application.DTOs;
using Supplus.Application.Services.Notificacoes;
using Supplus.Comunicacao.Requests.Usuarios;
using Supplus.Comunicacao.Responses.Usuarios;
using Supplus.Comunicacao.Validators.Usuarios;
using Supplus.Domain.Entities;
using Supplus.Domain.Enums;
using Supplus.Domain.Repositories;
using Supplus.Domain.Services;
using Supplus.Exceptions;
using Supplus.Exceptions.Mensagens;

namespace Supplus.Application.UseCases.Usuarios.Registrar;

public class RegistrarUsuarioUseCase : BaseUseCase<RequestRegistrarUsuarioJson, RegistrarUsuarioValidator>, IRegistrarUsuarioUseCase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUsuarioAutenticado _usuarioAutenticado;
    private readonly IEmailService _emailService;

    public RegistrarUsuarioUseCase(
        IUsuarioRepository usuarioRepository, 
        IUnitOfWork unitOfWork, 
        IUsuarioAutenticado usuarioAutenticado, IEmailService emailService)
    {
        _usuarioRepository = usuarioRepository;
        _unitOfWork = unitOfWork;
        _usuarioAutenticado = usuarioAutenticado;
        _emailService = emailService;
    }

    public async Task<ResultadoPersonalizado<ResponseUsuarioJson>> Executar(RequestRegistrarUsuarioJson request, CancellationToken token)
    {
        var usuarioAutenticado = await _usuarioAutenticado.ObterUsuarioAutenticadoAsync(token);

        // Agente de Suporte só pode criar usuários comuns
        if (!usuarioAutenticado.PodeRegistrarUsuario((Role)request.Role))
            return ResultadoPersonalizado<ResponseUsuarioJson>.Falha(ErroPadronizado.SemPermissaoErro()); 

        var resultado = Validar(request);

        if (resultado != ErroPadronizado.Nenhum)
            return ResultadoPersonalizado<ResponseUsuarioJson>.Falha(resultado);

        var existeUsuario = await _usuarioRepository.ExisteUsuarioComEmailAsync(request.Email, token);

        if (existeUsuario)
            return ResultadoPersonalizado<ResponseUsuarioJson>.Falha(ErroPadronizado.ConflitoErro(String.Format(MensagensErro.CONFLITO, "Usuário")));
        
        var usuario = new Usuario(
            request.Email, 
            request.PrimeiroNome, 
            request.Sobrenome, 
            usuarioAutenticado.Id, 
            (Role)request.Role);

        await _usuarioRepository.AdicionarAsync(usuario);

        await _unitOfWork.CommitAsync();

        var responseUsuario = new ResponseUsuarioJson(
            usuario.IdExterno, 
            usuario.Nome, 
            usuario.Sobrenome, 
            usuario.Email, 
            (Comunicacao.Enums.Role)usuario.Role, 
            usuario.ObterNomeCompleto());

        var emailDto = new EmailDTO(
            Destinatario: request.Email, 
            Assunto: "Bem-vindo ao Supplus!", 
            NomeTemplate: "BoasVindas", 
            new Dictionary<string, string>
            {
                { "NomeUsuario", usuario.Nome },
                { "LinkCadastroSenha", "https://google.com" }
            });

        try
        {
            await _emailService.EnviarAsync(emailDto);
        }
        catch { }
        
        return ResultadoPersonalizado<ResponseUsuarioJson>.Sucesso(responseUsuario);
    }
}
