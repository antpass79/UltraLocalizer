﻿using AutoMapper;
using FluentValidation.AspNetCore;
using MyLabLocalizer.IdentityDashboard.Server.Data;
using MyLabLocalizer.IdentityDashboard.Server.Models;
using MyLabLocalizer.IdentityDashboard.Server.Options;
using MyLabLocalizer.IdentityDashboard.Server.Repositories;
using MyLabLocalizer.IdentityDashboard.Server.Services;
using MyLabLocalizer.IdentityDashboard.Server.UnitOfWorks;
using Globe.Identity.Middlewares;
using Globe.Identity.Options;
using Globe.Identity.Security;
using Globe.Identity.Services;
using Globe.Identity.Servicess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Reflection;

namespace MyLabLocalizer.IdentityDashboard.Server
{
    public class Startup
    {
        readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Options
            services
                .AddOptions()
                .Configure<DatabaseOptions>(options =>
                {
                    _configuration.GetSection(nameof(DatabaseOptions)).Bind(options);
                    options.DefaultSqlServerConnection = _configuration.GetConnectionString("DefaultSqlServerConnection");
                    options.DefaultSqliteConnection = _configuration.GetConnectionString("DefaultSqliteConnection");
                })
                .Configure<JwtAuthenticationOptions>(options => _configuration.GetSection(nameof(JwtAuthenticationOptions)).Bind(options))
                .Configure<UserSettingsOptions>(options => _configuration.GetSection(nameof(UserSettingsOptions)).Bind(options));

            // Database
            services.AddDbContext<ApplicationDbContext, ApplicationDbContext>(
                ServiceLifetime.Transient);

            // Identity
            services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 3;
            })
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer();

            // Application
            services
                .AddScoped<IAsyncStyleService, StyleService>()
                .AddScoped<IAsyncApplicationService, ApplicationService>();

            // Repositories
            services
                .AddScoped<IAsyncUserRepository, UserManagerRepository>()
                .AddScoped<IAsyncRoleRepository, RoleManagerRepository>();

            // Unit of Works
            services
                .AddScoped<IAsyncUserUnitOfWork, UserUnitOfWork>()
                .AddScoped<IAsyncRoleUnitOfWork, RoleUnitOfWork>();

            // Services
            services
                .AddScoped<IAsyncUserService, UserService>()
                .AddScoped<IAsyncRoleService, RoleService>()
                .AddScoped<IAsyncRegisterService, RegisterService<ApplicationUser>>()
                .AddScoped<IAsyncLoginService, LoginService<ApplicationUser>>();

            // Security
            services
                .AddScoped<IJwtTokenEncoder<ApplicationUser>, UserRolesJwtTokenEncoder<ApplicationUser>>()
                .AddSingleton<ISigningCredentialsBuilder, SigningCredentialsBuilder>();
            services
                .AddSingleton<IConfigureOptions<JwtAuthenticationOptions>, ConfigureJwtAuthenticationOptions>()
                .AddSingleton<IPostConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();

            services.AddAutoMapper(typeof(Startup));

            services
                .AddControllersWithViews()
                .AddFluentValidation(options =>
                {
                    options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                });

            // Logging
            var logger = new LoggerConfiguration()
                .WriteTo.Console()
                //.WriteTo.File(Path.Combine(Directory.GetCurrentDirectory(), "ultralocalizer.txt"))
                .CreateLogger();
            services
                .AddSingleton(typeof(ILogger), logger);
        }

        public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider, IWebHostEnvironment env, ILogger logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                //app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = new JsonExceptionHandler(logger).Invoke
            });

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });

            //app.ApplyMigrationsAsync().Wait();
            //serviceProvider.CreateAdminAsync().Wait();
        }
    }
}
