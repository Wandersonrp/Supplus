using Supplus.Comunicacao.Validators;
using Supplus.Exceptions;

namespace Supplus.Application.UseCases;

public class BaseUseCase<TRequest, TValidator> where TValidator : BaseValidator<TRequest>, new()
{
    protected virtual ErroPadronizado Validar(TRequest request)
    {
        var validator = new TValidator();
        var resultado = validator.Validate(request);

        if (!resultado.IsValid)
        {
            var errors = resultado.Errors.Select(e => e.ErrorMessage).ToList();
            return ErroPadronizado.ErroDeValidacaoMensagens(errors);
        }

        return ErroPadronizado.Nenhum;
    }
}
