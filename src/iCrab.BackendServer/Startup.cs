using FluentValidation.AspNetCore;
using iCrabee.BackendServer.Data;
using iCrabee.BackendServer.Data.Entities;
using iCrabee.BackendServer.Extensions;
using iCrabee.BackendServer.IdentityServer;
using iCrabee.BackendServer.Services;
using iCrabee.ViewModels;
using iCrabee.ViewModels.Systems;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;

namespace iCrabee.BackendServer
{
    public class Startup
    {
        private readonly string iCrabeeSpecificOrigins = "iCrabeeSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //var secret = new IdentityServer4.Models.Secret("secret".Sha256());
            //1. Setup entity framework
            services.AddDbContextPool<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            //2. Setup idetntity
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
            .AddInMemoryApiResources(Config.Apis)
            .AddInMemoryClients(Configuration.GetSection("IdentityServer:Clients"))
            //.AddInMemoryClients(Config.Clients)
            .AddInMemoryIdentityResources(Config.Ids)
            .AddAspNetIdentity<User>()
            .AddProfileService<IdentityProfileService>()
            .AddDeveloperSigningCredential();

            services.AddCors(options =>
            {
                options.AddPolicy(iCrabeeSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins(Configuration["AllowOrigins"])
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.Configure<IdentityOptions>(options =>
            {
                // Default Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.User.RequireUniqueEmail = true;
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddControllersWithViews()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<RoleCreateRequestValidator>());

            services.AddAuthentication()
               .AddLocalApi("Bearer", option =>
               {
                   option.ExpectedScope = "api.icrabee";
               });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Bearer", policy =>
                {
                    policy.AddAuthenticationSchemes("Bearer");
                    policy.RequireAuthenticatedUser();
                });
            });

            services.AddRazorPages(options =>
            {
                options.Conventions.AddAreaFolderRouteModelConvention("Identity", "/Account/", model =>
                {
                    foreach (var selector in model.Selectors)
                    {
                        var attributeRouteModel = selector.AttributeRouteModel;
                        attributeRouteModel.Order = -1;
                        attributeRouteModel.Template = attributeRouteModel.Template.Remove(0, "Identity".Length);
                    }
                });
            });
            services.AddTransient<DbInitializer>();
            services.AddTransient<IEmailSender, EmailSenderService>();
            services.AddTransient<ISequenceService, SequenceService>();

            services.AddTransient<IStorageService, FileStorageService>();
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddTransient<IViewRenderService, ViewRenderService>();
            services.AddTransient<ICacheService, DistributedCacheService>();
            services.AddTransient<IOneSignalService, OneSignalService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "iCrabee API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(Configuration["AuthorityUrl"] + "/connect/authorize"),
                            Scopes = new Dictionary<string, string> { { "api.icrabee", "iCrabee API" } }
                        },
                    },
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new List<string>{ "api.icrabee" }
                    }
                });
            });

            services.AddDistributedSqlServerCache(o =>
            {
                o.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
                o.SchemaName = "dbo";
                o.TableName = "CacheTable";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseHsts(hsts => hsts.MaxAge(365).IncludeSubdomains().Preload());

                //app.UseXContentTypeOptions();
                //app.UseReferrerPolicy(opts => opts.NoReferrer());
                //app.UseXXssProtection(options => options.EnabledWithBlockMode());
                //app.UseXfo(options => options.Deny());
            }
            app.UseErrorWrapping();

            //app.UseCsp(opts => opts
            //        .BlockAllMixedContent()
            //        .StyleSources(s => s.Self())
            //        .StyleSources(s => s.UnsafeInline())
            //        .FontSources(s => s.Self())
            //        .FormActions(s => s.Self())
            //        .FrameAncestors(s => s.Self())
            //        .ImageSources(s => s.Self())
            //        .ScriptSources(s => s.Self())
            //    );

            app.UseStaticFiles();

            app.UseIdentityServer();

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(iCrabeeSpecificOrigins);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.OAuthClientId("swagger");
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "iCrabee API V1");
            });
        }
    }
}