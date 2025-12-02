
using AlphaProject.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Zitadel.Credentials;
using Zitadel.Extensions;

namespace AlphaProject.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddAutoMapper(typeof(Program));
       
            var jwtProfileJson = File.ReadAllText("zitadel_prof.json"); // builder.Configuration["Zitadel:JwtProfile"];
       

            builder.Services
                .AddAuthorization()
                .AddAuthentication()
                .AddZitadelIntrospection(o =>
                   {
                       o.Authority = builder.Configuration["Zitadel:Authority"];
                       // carica il JSON dal configuration store
                       o.JwtProfile = Application.LoadFromJsonString(jwtProfileJson); 
                   });

            builder.Services.AddAuthorization();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
