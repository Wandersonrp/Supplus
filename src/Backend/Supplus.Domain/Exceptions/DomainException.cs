namespace Supplus.Domain.Exceptions;

public class DomainException : SystemException
{
    public DomainException(string mensagem) : base(mensagem)
    {
    }

    public static void When(bool temErro, string mensagem)
    {
        if (temErro)
            throw new DomainException(mensagem);
    }
}
