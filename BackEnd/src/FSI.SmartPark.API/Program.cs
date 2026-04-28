using FSI.SmartPark.API.Middleware;
using FSI.SmartPark.Application.Services.Comercial;
using FSI.SmartPark.Application.Services.Equipe;
using FSI.SmartPark.Application.Services.Financeiro;
using FSI.SmartPark.Application.Services.Operacional;
using FSI.SmartPark.Application.Services.Suporte;
using FSI.SmartPark.Application.Interfaces.Comercial;
using FSI.SmartPark.Application.Interfaces.Equipe;
using FSI.SmartPark.Application.Interfaces.Financeiro;
using FSI.SmartPark.Application.Interfaces.Operacional;
using FSI.SmartPark.Application.Interfaces.Suporte;
using FSI.SmartPark.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Infrastructure (Dapper + MySQL)
builder.Services.AddInfrastructure();

// Application Services
builder.Services.AddScoped<IMovimentacaoService,   MovimentacaoService>();
builder.Services.AddScoped<IFaturamentoService,    FaturamentoService>();
builder.Services.AddScoped<IUnidadeService,        UnidadeService>();
builder.Services.AddScoped<IClienteService,        ClienteService>();
builder.Services.AddScoped<IContratoMensalistaService, ContratoMensalistaService>();
builder.Services.AddScoped<IPedidoSeloService,     PedidoSeloService>();
builder.Services.AddScoped<IContasAPagarService,   ContasAPagarService>();
builder.Services.AddScoped<IFuncionarioService,    FuncionarioService>();
builder.Services.AddScoped<IControlePontoService,  ControlePontoService>();
builder.Services.AddScoped<IUsuarioService,        UsuarioService>();
builder.Services.AddScoped<IEstoqueService,        EstoqueService>();

// Swagger + Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "SmartPark API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Informe: Bearer {token}",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    });
});

// CORS
builder.Services.AddCors(o => o.AddPolicy("SmartParkPolicy", p =>
    p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));


var app = builder.Build();

// 1. Exception Handler PRIMEIRO - captura todos os erros
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        if (error != null)
        {
            Console.WriteLine($"[ERRO GLOBAL] {error.Error.Message}");
            Console.WriteLine(error.Error.StackTrace);
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(
                System.Text.Json.JsonSerializer.Serialize(new
                {
                    erro = error.Error.Message
                })
            );
        }
    });
});

// 2. Middleware customizado (remova se já estiver tratando no ExceptionHandler acima)
// app.UseMiddleware<ExceptionMiddleware>(); ← comentar por enquanto para não conflitar

// 3. Swagger
app.UseSwagger();
app.UseSwaggerUI();

// 4. CORS antes de tudo que processa requisições
app.UseCors("SmartParkPolicy");

// 5. HTTPS
app.UseHttpsRedirection();

// 6. Auth
app.UseAuthorization();

// 7. Controllers
app.MapControllers();

app.Run();
