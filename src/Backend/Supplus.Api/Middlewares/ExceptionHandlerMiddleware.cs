using Supplus.Comunicacao.Responses;
using Supplus.Exceptions.Mensagens;

namespace Supplus.Api.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }        
        catch(OperationCanceledException ocex)
        {
            await HandleOperationCanceledExceptionAsync(context, ocex);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Trata erros genéricos do sistema, retornando um status 500 (Internal Server Error).
    /// </summary>
    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var errorMessage = new ResponseErroJson(MensagensErro.ERRO_DESCONHECIDO);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        return context.Response.WriteAsJsonAsync(errorMessage);
    }

    /// <summary>
    /// Trata interrupções de requisição causadas pelo cliente ou por timeouts, retornando o status 499 (Client Closed Request).
    /// </summary>
    /// <remarks>
    /// Este cenário ocorre comumente quando o usuário fecha a aba do navegador ou cancela a operação antes da conclusão do processamento.
    /// </remarks>
    private Task HandleOperationCanceledExceptionAsync(HttpContext context, OperationCanceledException exception)
    {
        var errorMessage = new ResponseErroJson(MensagensErro.OPERACAO_CANCELADA);
        context.Response.ContentType = "application/json";

        // Status code 499 para indicar que a operação foi cancelada pelo cliente
        context.Response.StatusCode = StatusCodes.Status499ClientClosedRequest; 
        return context.Response.WriteAsJsonAsync(errorMessage);
    }
}
