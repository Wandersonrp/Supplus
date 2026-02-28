using System.Text.Json.Serialization;

namespace Supplus.Comunicacao.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Prioridade
{
    Baixa = 1,
    Media = 2,
    Alta = 3,
    Urgente = 4
}
