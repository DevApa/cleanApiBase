using bg.crm.integration.application.dtos.models.execptions;
using bg.crm.integration.application.dtos.responses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;

namespace bg.crm.integration.infrastructure.extensions
{
    public static class ApplicationExtensions
    {
        public static IApplicationBuilder ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();

                    var _code = context.Response.StatusCode;
                    var _codeApp = 0;
                    var _message = exceptionHandlerPathFeature?.Error.Message;
                    var _stackTrace = string.Empty;
                    try
                    {
                        _codeApp = ((BaseCustomException)((ExceptionHandlerFeature)exceptionHandlerPathFeature!).Error).Code;
                        _message = ((BaseCustomException)((ExceptionHandlerFeature)exceptionHandlerPathFeature).Error).Message;
                        _stackTrace = ((BaseCustomException)((ExceptionHandlerFeature)exceptionHandlerPathFeature).Error).StackTrace;
                        switch (_codeApp)
                        {
                            case 500:
                                context.Response.StatusCode = _codeApp;
                                _code = _codeApp;
                                break;
                            case 401:
                                switch (_stackTrace)
                                {
                                    case "SecurityTokenExpiredException":
                                        context.Response.Headers.Append("Token-Expired", "true");
                                        break;
                                    case "ArgumentException":
                                    default:
                                        context.Response.Headers.Append("Token-Invalid", "true");
                                        break;
                                }
                                context.Response.StatusCode = _codeApp;
                                _code = _codeApp;
                                break;
                            case 204:
                                context.Response.StatusCode = _codeApp;
                                _code = _codeApp;
                                break;
                            default:
                                context.Response.StatusCode = _codeApp;
                                _code = _codeApp;
                                break;
                        }
                    }
                    catch (InvalidCastException ex)
                    {
                        Log.Error(ex, "Error parsing exception message: {Message}", ex.Message);
                    }

                    MsDtoResponseError response = new MsDtoResponseError
                    {
                        Code = _code,
                        Message = _message,
                        Errors = new List<MsError>(){
                            new MsError()
                            {
                                Code = _codeApp,
                                Message = _message
                            }
                        },
                        TraceId = context.TraceIdentifier == null ? string.Empty : context.TraceIdentifier.Split(":")[0].ToLower()
                    };

                    Log.Error(exceptionHandlerPathFeature?.Error, "An unhandled exception occurred: {Message}", _message);
                    var json = JsonConvert.SerializeObject(response);
                    await context.Response.WriteAsync(json);
                    await context.Response.Body.FlushAsync();
                });
            });
            return app;
        }
    }
}