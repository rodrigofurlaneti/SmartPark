using FSI.SmartPark.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// ─── Infrastructure (Dapper + MySQL) ─────────────────────────────────────────
builder.Services.AddInfrastructure();

// ─── CQRS via MediatR ────────────────────────────────────────────────────────
// Registra automaticamente todos os IRequestHandler<,> do assembly Application.
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(
        typeof(FSI.SmartPark.Application.Commands.Empresa.CreateEmpresaCommand).Assembly));

// ─── Swagger + Controllers ────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "SmartPark API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In          = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Informe: Bearer {token}",
        Name        = "Authorization",
        Type        = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });
});

// ─── CORS ─────────────────────────────────────────────────────────────────────
builder.Services.AddCors(o => o.AddPolicy("SmartParkPolicy", p =>
    p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

// =============================================================================
var app = builder.Build();
// =============================================================================

// 1. Exception Handler global — captura todos os erros não tratados
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        if (error is null) return;

        var (status, mensagem) = error.Error switch
        {
            KeyNotFoundException        => (StatusCodes.Status404NotFound,       error.Error.Message),
            ArgumentException           => (StatusCodes.Status400BadRequest,     error.Error.Message),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized,   error.Error.Message),
            _                           => (StatusCodes.Status500InternalServerError, "Erro interno do servidor.")
        };

        context.Response.StatusCode  = status;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(
            System.Text.Json.JsonSerializer.Serialize(new { erro = mensagem }));
    });
});

// 2. Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

// 3. CORS
app.UseCors("SmartParkPolicy");

// 4. HTTPS Redirect
app.UseHttpsRedirection();

// 5. Auth
app.UseAuthorization();

// 6. Controllers
app.MapControllers();

app.Run();
