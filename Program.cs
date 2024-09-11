
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using peliculasWebApi;
using peliculasWebApi.ApiBehavior;
using peliculasWebApi.Filtro;
using peliculasWebApi.Utilidades;
using System.IdentityModel.Tokens.Jwt;
using System.Text;



var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();



builder.Services.AddSingleton(provider =>

    new MapperConfiguration(config =>
    {
        var geometryFactory = provider.GetRequiredService<GeometryFactory>();
        config.AddProfile(new AutoMapperProfiles(geometryFactory));
    }).CreateMapper()
);

builder.Services.AddTransient<IAlmacenadorArchivos, AlmacenadorAzureStorage>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"),
sqlServer => sqlServer.UseNetTopologySuite()));

builder.Services.AddSingleton<GeometryFactory>(NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opciones =>
                opciones.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["llavejwt"])),
                    ClockSkew = TimeSpan.Zero
                });

builder.Services.AddAuthorization(opciones =>
{
    opciones.AddPolicy("EsAdmin", policy =>
    policy.RequireClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "admin"));
});


builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(FiltroDeExcepcion));
    options.Filters.Add(typeof(ParsearBadRequets));
}).ConfigureApiBehaviorOptions(BehaviorBadRequests.Parsear);

builder.Services.AddCors(options =>
{
    var frontendURL = builder.Configuration.GetValue<string>("frontend_url");
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins(frontendURL)
               .AllowAnyMethod()
               .AllowAnyHeader()
               .WithExposedHeaders(new string[] { "cantidadtotalregistros" });
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationInsightsTelemetry(new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions
{
    ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
