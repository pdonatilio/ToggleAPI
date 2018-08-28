using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using AutoMapper;
using Swashbuckle.AspNetCore.Swagger;
using ToggleAPI.Models;
using ToggleAPI.Services;
using ToggleAPI.Dtos;
using ToggleAPI.Helpers;

namespace ToggleAPI
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
            // MySql connection
            services.AddDbContextPool<ToogleAPIContext>(
                options => options.UseMySql(Configuration.GetConnectionString("MysqlConnection"),
                    mySqlOptionsAction =>
                    {
                        mySqlOptionsAction.ServerVersion(new Version(8, 0, 12), ServerType.MySql);
                    }

                )
            );

            // Redis connection
            services.AddDistributedRedisCache(
                options =>
                {
                    options.Configuration = "localhost";
                    options.InstanceName = "redis_test";
                }
            );

            // Configure DI for application services
            services.AddScoped<IUserService, UserService>();

            // Configure the AutoMapper
            services.AddAutoMapper();

            // Configure jwt authentication
            var appSettingsSection = Configuration.GetSection("JWTSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        var userId = int.Parse(context.Principal.Identity.Name);
                        var user = userService.Get(userId);
                        if (user == null)
                        {
                            // return unauthorized if user no longer exists
                            context.Fail("Unauthorized");
                        }
                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // Register the Swagger generator
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info 
                { 
                    Title = "Toggle API", 
                    Version = "v1",
                    Description = "A simple example ASP.NET Core 2.1 Web API",
                    Contact = new Contact
                    {
                        Name = "Paulo Donatilio",
                        Email = "pdonatilio@gmail.com",
                        Url = "https://www.linkedin.com/in/pdonatilio/"
                    },
                    License = new License
                    {
                        Name = "Use under Gnu GPL",
                        Url = "https://www.gnu.org/licenses/gpl-3.0.pt-br.html"
                    } 
                });
                c.DescribeAllEnumsAsStrings();
                // JWT-token Swagger authentication by password
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(security);
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
           
            app.UseAuthentication();
            app.UseSwagger();

            // Confirgure the swagger user interface
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Toggle API v1");
                
                c.DocumentTitle = "Title Documentation";
                c.DocExpansion(DocExpansion.None);
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
