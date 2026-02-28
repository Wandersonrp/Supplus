using FluentValidation;
using Supplus.Comunicacao.Requests.Chamados;
using Supplus.Exceptions.Mensagens;

namespace Supplus.Comunicacao.Validators.Chamados;

public class CriarChamadoValidator : BaseValidator<RequestCriarChamadoJson>
{
    public CriarChamadoValidator()
    {
        RuleFor(x => x.Titulo)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(String.Format(MensagensErro.CAMPO_OBRIGATORIO, nameof(RequestCriarChamadoJson.Titulo)))
            .MaximumLength(100).WithMessage(String.Format(MensagensErro.TAMANHO_MAXIMO_CAMPO, nameof(RequestCriarChamadoJson.Titulo), 100));

        RuleFor(x => x.Descricao)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(String.Format(MensagensErro.CAMPO_OBRIGATORIO, nameof(RequestCriarChamadoJson.Descricao)))
            .MaximumLength(1000).WithMessage(String.Format(MensagensErro.TAMANHO_MAXIMO_CAMPO, nameof(RequestCriarChamadoJson.Descricao), 1000));

        RuleFor(x => x.Prioridade)
            .Cascade(CascadeMode.Stop)
            .IsInEnum().WithMessage(String.Format(MensagensErro.VALOR_INVALIDO, nameof(RequestCriarChamadoJson.Prioridade)));

        RuleFor(x => x.IdCategoria)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(String.Format(MensagensErro.CAMPO_OBRIGATORIO, nameof(RequestCriarChamadoJson.IdCategoria)));
    }
}
