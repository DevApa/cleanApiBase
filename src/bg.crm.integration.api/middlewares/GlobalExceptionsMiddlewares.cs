using bg.crm.integration.application.dtos.models.execptions;
using bg.crm.integration.application.dtos.responses;
using Newtonsoft.Json;
using Serilog;

namespace bg.crm.integration.api.middlewares
{
    public class GlobalExceptionsMiddlewares : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            MsDtoResponseError responseError = new MsDtoResponseError();
            try
            {
                Log.Information("Request: {@Request}", context.Request);
                if (context.Request.QueryString.HasValue)
                    Log.Information("QueryString: {@QueryString}", context.Request.QueryString.ToString());
                context.Request.EnableBuffering();
                var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
                Log.Information("Request Body: {@RequestBody}", requestBody);
                context.Request.Body.Position = 0;
                await next(context);
            }
            catch (BadRequestException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                responseError = ObtenerMsDtoResponseError(context, ex, StatusCodes.Status400BadRequest, ex.Errros);
                await ResponseCatch(responseError, context);
            }
            catch (UnauthorizedException ex)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                responseError = ObtenerMsDtoResponseError(context, ex, StatusCodes.Status401Unauthorized, ex.Errros);
                await ResponseCatch(responseError, context);
            }
            catch (NotFoundException ex)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                responseError = ObtenerMsDtoResponseError(context, ex, StatusCodes.Status404NotFound, ex.Errros);
                await ResponseCatch(responseError, context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                responseError = ObtenerMsDtoResponseError(context, ex, StatusCodes.Status500InternalServerError);
                await ResponseCatch(responseError, context);
            }
        }

        private MsDtoResponseError ObtenerMsDtoResponseError(HttpContext context, Exception ex, int code, List<string>? errros = null)
        {
            MsDtoResponseError responseError = new MsDtoResponseError
            {
                Code = context.Response.StatusCode,
                TraceId = context.TraceIdentifier.Split(":")[0]!.ToLowerInvariant(),
                Message = "Error en el servicio",
                Errors = new()
            };
            if (errros != null && errros.Count > 0)
            {
                foreach (var error in errros)
                {
                    responseError.Errors.Add(new MsError
                    {
                        Code = code,
                        Message = error
                    });
                }
            }
            else
            {
                responseError.Errors.Add(new MsError
                {
                    Code = code,
                    Message = ex.Message
                });
            }
            return responseError;
        }

        private async Task ResponseCatch(MsDtoResponseError responseError, HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var response = JsonConvert.SerializeObject(responseError, Formatting.Indented);
            await context.Response.WriteAsync(response);
        }
    }
}