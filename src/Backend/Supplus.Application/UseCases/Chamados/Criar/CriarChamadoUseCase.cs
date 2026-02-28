using Supplus.Comunicacao.Requests.Chamados;
using Supplus.Comunicacao.Responses.Chamados;
using Supplus.Comunicacao.Responses.Usuarios;
using Supplus.Comunicacao.Validators.Chamados;
using Supplus.Domain.Entities;
using Supplus.Domain.Enums;
using Supplus.Domain.Repositories;
using Supplus.Domain.Services;
using Supplus.Exceptions;
using Supplus.Exceptions.Mensagens;

namespace Supplus.Application.UseCases.Chamados.Criar;

public class CriarChamadoUseCase : BaseUseCase<RequestCriarChamadoJson, CriarChamadoValidator>, ICriarChamadoUseCase
{
    private readonly IUsuarioAutenticado _usuarioAutenticadoService;
    private readonly IRepository<Chamado> _chamadoRepository;
    private readonly IRepository<Categoria> _categoriaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CriarChamadoUseCase(
        IUsuarioAutenticado usuarioAutenticadoService, 
        IRepository<Chamado> chamadoRepository,
        IRepository<Categoria> categoriaRepository,
        IUnitOfWork unitOfWork)
    {
        _usuarioAutenticadoService = usuarioAutenticadoService;
        _chamadoRepository = chamadoRepository;
        _categoriaRepository = categoriaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResultadoPersonalizado<ResponseChamadoJson>> Executar(RequestCriarChamadoJson request, CancellationToken token)
    {
        var resultado = Validar(request);

        if (resultado != ErroPadronizado.Nenhum)
            return ResultadoPersonalizado<ResponseChamadoJson>.Falha(resultado);

        var usuarioAutenticado = await _usuarioAutenticadoService.ObterUsuarioAutenticadoAsync(token);

        var categoria = await _categoriaRepository.ObterPorIdExternoAsync(request.IdCategoria);

        if (categoria is null)
            return ResultadoPersonalizado<ResponseChamadoJson>
                .Falha(ErroPadronizado.NaoEncontradoErro(String.Format(MensagensErro.NAO_ENCONTRADO, nameof(Categoria), request.IdCategoria)));

        var chamado = new Chamado(
            titulo: request.Titulo, 
            descricao: request.Descricao, 
            prioridade: (Prioridade)request.Prioridade, 
            usuarioAutenticado.Id, 
            idCategoria: categoria.Id);

        await _chamadoRepository.AdicionarAsync(chamado);

        await _unitOfWork.CommitAsync();

        var response = new ResponseChamadoJson(
            chamado.IdExterno, 
            chamado.Titulo, 
            chamado.Descricao, 
            (Comunicacao.Enums.StatusChamado)chamado.StatusChamado, 
            (Comunicacao.Enums.Prioridade) chamado.Prioridade, 
            chamado.AbertoEm);

        return ResultadoPersonalizado<ResponseChamadoJson>.Sucesso(response);
    }
}
