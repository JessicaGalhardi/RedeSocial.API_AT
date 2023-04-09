using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RedeSocial.BLL.Configuration;
using RedeSocial.DOMAIN;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Add services to the container.

// DB Connection Instance
builder.Services.AddDbContext<RedeSocialContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("RedeSocialConexion")));


//Configurations for the Identity

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<RedeSocialContext>();

//JWTConfigurations
var jwtSection = builder.Configuration.GetSection("JwtBearerTokenSettings");

builder.Services.Configure<JwtBearerTokenSettings>(jwtSection);

var jwtBearerTokenSettings = jwtSection.Get<JwtBearerTokenSettings>();

var key = Encoding.ASCII.GetBytes(jwtBearerTokenSettings.SecretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = jwtBearerTokenSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtBearerTokenSettings.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Rede Social.API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter with a valid token",
        Name = "Authorize",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
              {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                  {
                          Type=ReferenceType.SecurityScheme,
                          Id="Bearer"
                  }
                },
                new string[]{}
            }
        });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

//Config for use de identity e jwt

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();


app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});



app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.Run();
