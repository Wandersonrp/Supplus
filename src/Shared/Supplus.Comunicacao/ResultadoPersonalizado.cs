namespace Supplus.Exceptions;

/// <summary>
/// Representa o resultado padronizado de uma operação na aplicação.
/// Pode indicar sucesso, contendo um valor do tipo <typeparamref name="T"/>,
/// ou falha, contendo um erro padronizado. Essa classe facilita o tratamento
/// consistente de operações, evitando exceções desnecessárias e permitindo
/// maior clareza na comunicação entre métodos e camadas.
/// </summary>
public class ResultadoPersonalizado<T>
{
    private readonly T? _valor;

    private ResultadoPersonalizado(T valor)
    {
        Valor = valor;
        ESucesso = true;
        Erro = ErroPadronizado.Nenhum;
    }

    private ResultadoPersonalizado(ErroPadronizado erro)
    {
        if (erro == ErroPadronizado.Nenhum)
            throw new ArgumentException("Erro inválido", nameof(erro));

        ESucesso = false;
        Erro = erro;
    }

    public bool ESucesso { get; }
    public bool Falhou => !ESucesso;

    public T Valor
    {
        get
        {
            if (Falhou)
                throw new InvalidOperationException("Nenhum valor para a falha.");

            return _valor!;
        }

        private init => _valor= value;
    }

    public ErroPadronizado Erro { get; }

    public static ResultadoPersonalizado<T> Sucesso(T valor) => new ResultadoPersonalizado<T>(valor);
    public static ResultadoPersonalizado<T> Falha(ErroPadronizado erro) => new ResultadoPersonalizado<T>(erro);
}
