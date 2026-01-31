using System.Text.Json.Serialization;

namespace Supplus.Comunicacao.Requests.RefreshTokens;

public record RequestNovoTokenJson(
    [property: JsonIgnore] string RefreshToken, 
    [property: JsonIgnore] string Ip, 
    [property: JsonIgnore] string DispositivoInfo);