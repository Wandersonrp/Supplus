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

    public RegistrarUsuarioUseCase(IUsuarioRepository usuarioRepository, IUnitOfWork unitOfWork, IUsuarioAutenticado usuarioAutenticado)
    {
        _usuarioRepository = usuarioRepository;
        _unitOfWork = unitOfWork;
        _usuarioAutenticado = usuarioAutenticado;
    }

    public async Task<ResultadoPersonalizado<ResponseUsuarioJson>> Executar(RequestRegistrarUsuarioJson request, CancellationToken token)
    {
        var usuarioAutenticado = await _usuarioAutenticado.ObterUsuarioAutenticadoAsync();

        // Agente de Suporte só pode criar usuários comuns
        if (!usuarioAutenticado.PodeRegistrarUsuario((Role)request.Role))
            return ResultadoPersonalizado<ResponseUsuarioJson>.Falha(ErroPadronizado.NaoAutorizadoErro(MensagensErro.PERMISSOES_INVALIDAS)); 

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

        await _unitOfWork.CommitAsync();

        var responseUsuario = new ResponseUsuarioJson(usuario.IdExterno,usuario.Nome, usuario.Sobrenome, usuario.Email, (Comunicacao.Enums.Role)usuario.Role);

        return ResultadoPersonalizado<ResponseUsuarioJson>.Sucesso(responseUsuario);
    }
}
