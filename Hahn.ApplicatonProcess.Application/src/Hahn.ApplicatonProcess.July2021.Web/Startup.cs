using FluentValidation.AspNetCore;
using Hahn.ApplicatonProcess.July2021.Data;
using Hahn.ApplicatonProcess.July2021.Data.Conincap;
using Hahn.ApplicatonProcess.July2021.Data.UnitOfWoork;
using Hahn.ApplicatonProcess.July2021.Domain;
using Hahn.ApplicatonProcess.July2021.Domain.Cache;
using Hahn.ApplicatonProcess.July2021.Domain.Interfaces;
using Hahn.ApplicatonProcess.July2021.Domain.Mediatr;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Context;
using Serilog.Core.Enrichers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Authentication;

namespace Hahn.ApplicatonProcess.July2021.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:5002")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });

            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation()
                .AddNewtonsoftJson();

            services.AddMvc()
                .AddJsonOptions(opt => { opt.JsonSerializerOptions.PropertyNameCaseInsensitive = true; })
                .AddApplicationPart(typeof(Startup).Assembly)
                .AddFluentValidation(fv => {
                    fv.UseFluentValidators();
                    fv.DisableDataAnnotationsValidation = true;
                    fv.ConfigureClientsideValidation(enabled: false);
                    fv.ImplicitlyValidateChildProperties = true;
                });

            services.AddHealthChecks();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Hahn.ApplicatonProcess.July2021.Api",
                    Description = "Hahn API backend",
                    Contact = new OpenApiContact
                    {
                        Name = "Kamran Qadir",
                        Email = "qadir0108@gmail.com",
                        Url = new Uri("https://www.linkedin.com/in/kamranqadir/")
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddMapper(cfg =>
            {
                Bootstraper.AddMaps(cfg);
                //cfg.AddViewModelsMapper();

            });

            services.AddProblemDetails(ConfigureProblemDetails);

            AddSessionCache(services);

            AddMediatR(services);

            AddDb(services);

            AddHttpClient(services);

            services.AddSingleton<IConincapClient, ConincapClient>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICache, RemoteAssetCache>();

        }

        private void AddHttpClient(IServiceCollection services)
        {
            services.AddHttpClient("HahnClient", client => {
                client.DefaultRequestHeaders.Add("User-Agent", "Hahn");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                //client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip");
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; },
            });
        }

        private void AddSessionCache(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSession();
            services.AddHttpContextAccessor();
        }

        private void AddMediatR(IServiceCollection services)
        {
            services.AddMediatR(typeof(Startup));
            services.AddDomainMediatR();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorPipelineBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        }

        private void AddDb(IServiceCollection services)
        {
            //services.AddDbContext<HahnContext>(options => options.UseInMemoryDatabase("sqlserver"));
            services.AddDbContext<HahnContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SqlConnection")));
        }

        private void ConfigureProblemDetails(ProblemDetailsOptions options)
        {
            options.Rethrow<NotSupportedException>();

            options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);
            options.MapToStatusCode<AuthenticationException>(StatusCodes.Status401Unauthorized);
            options.MapToStatusCode<UnauthorizedAccessException>(StatusCodes.Status403Forbidden);
            options.MapToStatusCode<ValidationException>(StatusCodes.Status400BadRequest);
            options.MapToStatusCode<KeyNotFoundException>(StatusCodes.Status404NotFound);

            options.MapToStatusCode<HttpRequestException>(StatusCodes.Status503ServiceUnavailable);

            // Because exceptions are handled polymorphically, this will act as a "catch all" mapping, which is why it's added last.
            // If an exception other than NotImplementedException and HttpRequestException is thrown, this will handle it.
            options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hahn.ApplicatonProcess.July2021.Api v1");
                    c.RoutePrefix = string.Empty;
                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseProblemDetails();

            app.UseSerilogRequestLogging();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapHealthChecks("/healthcheck");
                endpoints.MapControllers();
            });

            app.Use(async (ctx, next) =>
            {
                using (LogContext.Push(
                    new PropertyEnricher("IPAddress", ctx.Connection.RemoteIpAddress),
                    new PropertyEnricher("RequestHost", ctx.Request.Host),
                    new PropertyEnricher("RequestBasePath", ctx.Request.Path),
                    new PropertyEnricher("RequestQueryParams", ctx.Request.QueryString)))
                {
                    await next();
                }
            });


        }
    }
}
