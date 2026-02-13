using FluentValidation;
using Supplus.Comunicacao.Requests.Usuarios;
using Supplus.Exceptions.Mensagens;

namespace Supplus.Comunicacao.Validators.Usuarios;

public class RegistrarUsuarioValidator : BaseValidator<RequestRegistrarUsuarioJson>
{
    public RegistrarUsuarioValidator()
    {
        RuleFor(x => x.PrimeiroNome)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(String.Format(MensagensErro.CAMPO_OBRIGATORIO, nameof(RequestRegistrarUsuarioJson.PrimeiroNome)))
            .MaximumLength(50).WithMessage(String.Format(MensagensErro.TAMANHO_MAXIMO_CAMPO, nameof(RequestRegistrarUsuarioJson.PrimeiroNome), 50));

        RuleFor(x => x.Sobrenome)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(String.Format(MensagensErro.CAMPO_OBRIGATORIO, nameof(RequestRegistrarUsuarioJson.Sobrenome)))
            .MaximumLength(50).WithMessage(String.Format(MensagensErro.TAMANHO_MAXIMO_CAMPO, nameof(RequestRegistrarUsuarioJson.Sobrenome), 50));

        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(String.Format(MensagensErro.CAMPO_OBRIGATORIO, nameof(RequestRegistrarUsuarioJson.Email)))
            .EmailAddress().WithMessage(MensagensErro.EMAIL_INVALIDO)
            .MaximumLength(150).WithMessage(String.Format(MensagensErro.TAMANHO_MAXIMO_CAMPO, nameof(RequestRegistrarUsuarioJson.Email), 150));

            RuleFor(x => x.Role)
            .IsInEnum().WithMessage(String.Format(MensagensErro.VALOR_INVALIDO, nameof(RequestRegistrarUsuarioJson.Role)));
    }
}
