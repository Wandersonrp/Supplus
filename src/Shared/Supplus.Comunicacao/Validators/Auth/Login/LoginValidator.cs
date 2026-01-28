using FluentValidation;
using Supplus.Comunicacao.Requests.Auth;
using Supplus.Exceptions.Mensagens;

namespace Supplus.Comunicacao.Validators.Auth.Login;

public sealed class LoginValidator : BaseValidator<RequestLoginJson>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(string.Format(MensagensErro.CAMPO_OBRIGATORIO, "Email"))
            .EmailAddress().WithMessage(MensagensErro.EMAIL_INVALIDO);

        RuleFor(x => x.Senha)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(string.Format(MensagensErro.CAMPO_OBRIGATORIO, "Senha"))
            .MinimumLength(8).WithMessage(string.Format(MensagensErro.TAMANHO_MINIMO_CAMPO, "Senha", 8))
            .MaximumLength(20).WithMessage(string.Format(MensagensErro.TAMANHO_MAXIMO_CAMPO, "Senha", 20));            
    }
}
