using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using CommentsApi.Contexts;
using CommentsApi.Services;
using CommentsApi.Repositories;

namespace CommentsApi
{
    public class CommentsApiStartup
    {
        public CommentsApiStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Database context.
            services.AddDbContext<CommentsApiDbContext>();

            // Routing.
            services.AddRouting();

            // Register the authentication strategy.
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.Cookie.HttpOnly = false;
                options.Cookie.Domain = Configuration["HttpContext:Domain"];
            })
            .AddGitHub(options =>
            {
                options.ClientId = "Iv1.d51bc05fcb82e671";
                options.ClientSecret = "0d499b7a2abba271f1c32e4ff1b938c804bb438f";

                options.CallbackPath = "/oauth/callback/github";

                options.CorrelationCookie.HttpOnly = false;
                options.CorrelationCookie.Domain = Configuration["HttpContext:Domain"];
                options.CorrelationCookie.SameSite = SameSiteMode.Unspecified;
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Domain = Configuration["HttpContext:Domain"];
            });

            // Controllers.
            services.AddControllers();

            // Repositories.
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();

            // Http context accessor.
            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();

            // Services.
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICommentService, CommentService>();

            // Register the swagger generator.
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Comments API", Version = "v1" });
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Comments API");
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(enpoints =>
            {
                enpoints.MapControllers();
            });
        }
    }
}
