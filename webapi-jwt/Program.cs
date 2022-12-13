using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(x =>
 {
     x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
     x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
 }).AddJwtBearer(x =>
 {
     x.RequireHttpsMetadata = false;
     x.SaveToken = true;
     Console.WriteLine("masuk ga");
     x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
     {
         ValidateIssuerSigningKey = true,
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(State.Secret)),
         ValidateIssuer = false,
         ValidateAudience = false
     };
 });
builder.Services.AddSwaggerGen(c =>
       {
           c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthService", Version = "v1" });
           var securitySchema = new OpenApiSecurityScheme
           {

               Description = "JWT Authorization header menggunakan bearer. \nPlease enter into field the word 'Bearer' following by space and JWT",
               Name = "Authorization",
               In = ParameterLocation.Header,
               Type = SecuritySchemeType.Http,
               Scheme = "bearer",
               Reference = new OpenApiReference
               {
                   Type = ReferenceType.SecurityScheme,
                   Id = "Bearer"
               }
           };
           c.AddSecurityDefinition("Bearer", securitySchema);
           c.AddSecurityRequirement(new OpenApiSecurityRequirement
           {
                {securitySchema, new[]{ "Bearer"} }
           });
       });

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapControllers();
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
app.UseMiddleware<JwtMiddleware>();
app.UseAuthorization();
app.Run();
