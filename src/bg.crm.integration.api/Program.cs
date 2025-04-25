using bg.crm.integration.api.extensions;
using bg.crm.integration.api.middlewares;
using bg.crm.integration.application.ioc;
using bg.crm.integration.infrastructure.extensions;
using bg.crm.integration.infrastructure.ioc;
using bg.crm.integration.infrastructure.observability;
using bg.crm.integration.infrastructure.security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);
builder = builder.ConfigureEnviromentVariables("BGPLANTILLA");
builder.Services.AddTransient<GlobalExceptionsMiddlewares>();
builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var version = builder.Configuration["OpenApi:info:version"];
    var title = builder.Configuration["OpenApi:info:title"];
    var description = builder.Configuration["OpenApi:info:description"];
    var termsOfService = new Uri(builder.Configuration["OpenApi:info:termsOfService"]!);
    var contact = new OpenApiContact
    {
        Name = builder.Configuration["OpenApi:info:contact:name"],
        Url = new Uri(builder.Configuration["OpenApi:info:contact:url"]!),
        Email = builder.Configuration["OpenApi:info:contact:email"]
    };
    var license = new OpenApiLicense
    {
        Name = builder.Configuration["OpenApi:info:License:name"],
        Url = new Uri(builder.Configuration["OpenApi:info:License:url"]!)
    };
    options.SwaggerDoc(builder.Configuration["OpenApi:info:version"], new OpenApiInfo
    {
        Version = builder.Configuration["OpenApi:info:version"],
        Title = title,
        Description = description,
        TermsOfService = termsOfService,
        Contact = contact,
        License = license
    }
    );
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Put **_ONLY_** your JWT Bearer **_token_** on textbox below! \r\n\r\n\r\n Example: \"Value: **12345abcdef**\"",
 
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    options.OperationFilter<RequiredHeaderParameter>();
    List<string> xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly).ToList();
    xmlFiles.ForEach(xmlFile => options.IncludeXmlComments(xmlFile));
    options.ExampleFilters();
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();
builder.Services.RegisterDependencies();
builder.Services.AddTransient<GlobalExceptionsMiddlewares>();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.SetupAuthenticationServices(builder);

var corsOrigins = "_AllowPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsOrigins, builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        {
            string? version = builder.Configuration["Swagger:Version"];
            c.SwaggerEndpoint($"v1/swagger.json", version);
        });
}

app.ConfigureMetricServerApp();
app.ConfigureExceptionHandler();
app.UseAuthorization();
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseMiddleware<GlobalExceptionsMiddlewares>();
app.ConfigureHealthChecksApp();
app.MapControllers();
app.UseCors(corsOrigins);
app.Run();