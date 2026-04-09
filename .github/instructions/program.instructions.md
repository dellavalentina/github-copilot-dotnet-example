---
applyTo: "**/Program.cs"
---
# Padrões do Program.cs (<Projeto>.Api)

## Responsabilidade
Composição e inicialização da aplicação: registro de serviços, configuração de middlewares e definição do pipeline HTTP.

## Estrutura obrigatória do `Program.cs`

```csharp
using System.Text;
using <Projeto>.Ioc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// 1. Serviços do IoC (DI + DbContext + AutoMapper)
builder.Services.AddInjecoesDependencia();
builder.Services.AddAutoMapper();
builder.Services.AddDbContext(builder.Configuration);

// 2. Controllers
builder.Services.AddControllers();

// 3. Autenticação JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

// 4. Swagger com suporte a Bearer JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "<Projeto> API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Informe o token JWT"
    });

    options.AddSecurityRequirement(document =>
    {
        var requirement = new OpenApiSecurityRequirement();
        var schemeRef = new OpenApiSecuritySchemeReference("Bearer", document);
        requirement.Add(schemeRef, new List<string>());
        return requirement;
    });
});

// 5. CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// 6. Pipeline — ordem obrigatória
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();              // ← antes de auth
app.UseAuthentication();    // ← antes de authorization
app.UseAuthorization();
app.MapControllers();

app.Run();
```

## Ordem de registro dos serviços (seção `builder.Services`)

| Ordem | O que registrar |
|-------|----------------|
| 1 | `AddInjecoesDependencia()` — repositórios, serviços de domínio e de aplicação |
| 2 | `AddAutoMapper()` — perfis AutoMapper |
| 3 | `AddDbContext(configuration)` — contexto EF Core com SQL Server |
| 4 | `AddControllers()` |
| 5 | `AddAuthentication(...).AddJwtBearer(...)` |
| 6 | `AddEndpointsApiExplorer()` + `AddSwaggerGen(...)` |
| 7 | `AddCors(...)` |

## Ordem dos middlewares (seção `app.Use...`)

| Ordem | Middleware | Motivo |
|-------|-----------|--------|
| 1 | `UseSwagger()` + `UseSwaggerUI()` | Somente em Development |
| 2 | `UseCors()` | **Deve vir antes de auth** |
| 3 | `UseAuthentication()` | Antes de authorization |
| 4 | `UseAuthorization()` | Após authentication |
| 5 | `MapControllers()` | Roteia as requisições |

## Estrutura obrigatória do `appsettings.json`

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": ""
  },
  "Jwt": {
    "Key": "",
    "Issuer": "",
    "Audience": "",
    "ExpiracaoHoras": "8"
  }
}
```

> ⛔ Os valores de `ConnectionStrings:DefaultConnection`, `Jwt:Key`, `Jwt:Issuer` e `Jwt:Audience` **nunca devem ser preenchidos** no arquivo `appsettings.json` commitado. Devem ser configurados via `dotnet user-secrets` em desenvolvimento ou variáveis de ambiente em produção.

## Regras
- ✅ Manter a ordem de registro e de middlewares descrita acima
- ✅ `UseCors()` sempre antes de `UseAuthentication()`
- ✅ Swagger habilitado apenas em `Development`
- ❌ NÃO adicionar lógica de negócio no `Program.cs`
- ❌ NÃO registrar serviços fora do `ConfiguracoesInjecoesDependencia` — centralize no IoC
