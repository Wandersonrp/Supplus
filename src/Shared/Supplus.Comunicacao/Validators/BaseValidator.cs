using FluentValidation;

namespace Supplus.Comunicacao.Validators;

public class BaseValidator<T> : AbstractValidator<T>
{
    public Func<object, string, Task<IEnumerable<string>>> Validar => async (modelo, propriedade) =>
    {
        var result = await ValidateAsync(ValidationContext<T>.CreateWithOptions((T)modelo, x => x.IncludeProperties(propriedade)));

        if (result.IsValid)
            return Array.Empty<string>();

        return result.Errors.Select(e => e.ErrorMessage);
    };
}
