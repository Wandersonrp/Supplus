using System.Text.Json.Serialization;

namespace Supplus.Comunicacao.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StatusChamado
{
    Aberto = 1,
    EmAndamento = 2,
    AguardandoCliente = 3,
    Resolvido = 4,
    Fechado = 5
}
