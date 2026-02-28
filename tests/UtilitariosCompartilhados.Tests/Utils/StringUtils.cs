using System.Text;

namespace UtilitariosCompartilhados.Tests.Utils;

public static class StringUtils
{
    public static string CriarPalavra(int tamanho = 10)
    {
        var sb = new StringBuilder();        

        var letras = "ABCDEFGHIJKLMENOPQRSTUVWXYZabcdefghijklmnopqrstuvwyzãâáàíêéôõóúû";
        var random = new Random();

        for(int i = 0; i < tamanho; i++)
        {
            var index = random.Next(letras.Length);
            sb.Append(letras[index]);
        }

        return sb.ToString();
    }
}
