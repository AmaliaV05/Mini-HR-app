using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Mini_HR_app.BusinessLogic.Interfaces;
using Mini_HR_app.BusinessLogic.Services;
using Mini_HR_app.Data;
using Mini_HR_app.Data.Models.Constants.Utils;
using Mini_HR_app.Extensions;
using Mini_HR_app.Helpers;
using Mini_HR_app.Services;
using System;
using System.IO;
using System.Reflection;

namespace Mini_HR_app
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.AllowAnyOrigin()
                                             .AllowAnyHeader()
                                             .AllowAnyMethod();
                                  });
            });
            
            services.AddAutoMapper(typeof(MappingProfile));

            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString("DefaultConnection")));

            services.AddHttpContextAccessor();

            services.AddAuthentication();
            services.AddIdentityService();
            services.AddJwtToken(Configuration);
            services.AddAuthorization(options =>
            {
                var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                    JwtBearerDefaults.AuthenticationScheme);

                defaultAuthorizationPolicyBuilder =
                    defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();

                options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();

                options.AddPolicy("Admin_Manager", policy =>
                    policy.RequireRole(UserRole.ADMIN, UserRole.MANAGER));

                options.AddPolicy("Admin_Manager_Member", policy =>
                    policy.RequireRole(UserRole.ADMIN, UserRole.MANAGER, UserRole.MEMBER));
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Mini HR application",
                    Description = "Application used to manage employees' information for multiple companies"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",                    
                    Description = "Enter JWT Bearer token"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { UserRole.ADMIN, UserRole.MANAGER, UserRole.MEMBER }
                    }
                });
                // FIXME: fix AllowAnonymousOperationFilter to exclude [AllowAnonymous] endpoints from requiring auth
                
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);                
            });

            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));         

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAccountManagementService, AccountManagementService>();
            services.AddScoped<ICompaniesService, CompaniesService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IEmployeesService, EmployeesService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddSingleton<AuditTrailFactory>();
            services.AddSingleton<AuditTrailService>();            
            services.AddScoped<ICurrentUserService, CurrentUserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mini HR app v1"));
            }

            app.UseCustomException();

            app.UseHttpsRedirection();
            
            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });            
        }
    }
}
