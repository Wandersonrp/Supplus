namespace Supplus.Domain.Entities;

public sealed class RefreshToken : EntidadeBase
{
    /// <summary>
    /// Valor do Refresh Token   
    /// </summary>
    public string Token { get; private set; }

    /// <summary>
    /// Informações do dispositivo que gerou o Refresh Token.
    /// </summary>
    public string DispositivoInfo { get; private set; } = string.Empty;

    /// <summary>
    /// Ip do dispositivo que gerou o Refresh Token.
    /// </summary>
    public string Ip { get; private set; }
    public DateTime DataExpiracao { get; private set; }
    
    /// <summary>
    /// Indica se o Refresh Token foi usado e substituído por um novo.
    /// </summary>
    public bool FoiUsado { get; private set; }

    /// <summary>
    /// Indica se o Refresh Token foi revogado.
    /// </summary>
    public bool FoiRevogado { get; private set; }

    /// <summary>
    /// Verifica se o Refresh Token está ativo (não expirado, não revogado e não usado).
    /// </summary>
    public bool EstaAtivo => !FoiRevogado && !FoiUsado && !EstaExpirado();

    public long IdUsuario { get; private set; }
    public Usuario Usuario { get; private set; }

    private RefreshToken() { }

    public RefreshToken(long idUsuario, string dispositivoInfo, string ip) : base(idUsuario)
    {
        Token = Gerar();
        IdUsuario = idUsuario;
        DispositivoInfo = dispositivoInfo;
        DataExpiracao = DateTime.UtcNow.AddHours(RegrasConstants.EXPIRACAO_REFRESH_TOKEN_HORAS);
        FoiRevogado = false;
        FoiUsado = false;
        Ip = ip;        
    }

    /// <summary>
    /// Gera um novo valor para o Refresh Token.
    /// </summary>
    /// <returns></returns>
    public string Gerar() => Convert.ToBase64String(Guid.NewGuid().ToByteArray());

    /// <summary>
    /// Determina se o refresh token está expirado com base na sua data de criação
    /// e no período de expiração configurado.
    /// </summary>
    /// <remarks>
    /// O período de expiração é definido pelo valor de
    /// RegrasConstants.EXPIRACAO_REFRESH_TOKEN_HORAS. Este método utiliza o horário
    /// UTC atual para avaliar a expiração.
    /// </remarks>
    /// <returns>Retorna true se o refresh token estiver expirado; caso contrário, false.</returns>
    public bool EstaExpirado() => DataExpiracao <= DateTime.UtcNow;    

    /// <summary>
    /// Revoga o token (usado para logout).
    /// </summary>
    public void Revogar()
    {
        FoiRevogado = true;
        DefinirAtualizadoEm();        
    }

    /// <summary>
    /// Marca o token como usado (usado na rotação de tokens).
    /// </summary>
    public void MarcarComoUsado()
    {
        FoiUsado = true;
        DefinirAtualizadoEm();
    }
}
