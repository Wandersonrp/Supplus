using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Supplus.Comunicacao.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Role
{
    [Description("Administrador")]
    Administrador = 1,

    [Description("Agente de Suporte")]
    AgenteSuporte = 2,

    [Description("Usuário Padrão")]
    UsuarioComum = 3
}
