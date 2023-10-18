using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ShBarcelona.DAL;

namespace ShBarcelona.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            // Recruitment Services


            //services.AddServices(_configuration);
            services.AddDAL(_configuration);
            services.AddSignalR();
            services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "People Api", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()); //This line
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            // ASP.NET Core Services
            AddSecurity(services);
            services.AddControllers().AddNewtonsoftJson(options =>
                            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            services.AddSignalR();
            services.AddCors(o =>
            {
                o.AddPolicy("All", builder => builder.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader());
                o.AddPolicy("CorsPolicy", builder => builder
                                .WithOrigins("http://localhost:4200") // the Angular app url
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials());
            });

            Audit.EntityFramework.Configuration.Setup()
                .ForContext<RecruitmentContext>(config => config
                    .IncludeEntityObjects()
                    .AuditEventType("{RecruitmentContext}:{shbarcelona_dev}"))
                .UseOptOut();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RecruitmentContext recruitmentContext, IHttpContextAccessor httpContextAccessor)
        {
            /*Audit.Core.Configuration.AddCustomAction(ActionType.OnScopeCreated, scope =>
            {
                //scope.SetCustomField("User", httpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value ?? "root");
            });*/

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("CorsPolicy");
            }
            else
                app.UseCors("All");



            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStatusCodePages();

            //app.UseUserInjector();
            //TODO: Uncomment to enable admin restriction
            //app.UseDevAccess();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapHub<ServerHub>("/serverhub");
            });

            if (!env.IsProduction())
            {
                Audit.Core.Configuration.AuditDisabled = true;

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("./v1/swagger.json", "People API V1"); //originally "./swagger/v1/swagger.json"
                });
            }

            recruitmentContext.Database.Migrate();

        }

        private void AddSecurity(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //IMPORTANT! Don't uncomment the following lines because you won't be able to build migrations!
            //string secret = Environment.GetEnvironmentVariable("PEOPLETOKENSECRET");
            //byte[] key = Encoding.ASCII.GetBytes(secret);
            byte[] key = Encoding.ASCII.GetBytes(_configuration["PeopleTokenConfiguration:Secret"]);


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["PeopleTokenConfiguration:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["PeopleTokenConfiguration:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents();
                options.Events.OnAuthenticationFailed += (e) =>
                {
                    return Task.CompletedTask;
                };
                options.Events.OnTokenValidated += (e) =>
                {
                    return Task.CompletedTask;
                };
                options.Events.OnChallenge += (e) =>
                {
                    return Task.CompletedTask;
                };
                options.Events.OnMessageReceived += (e) =>
                {
                    var accessToken = e.Request.Query["access_token"];

                    var path = e.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/serverhub"))
                    {
                        e.Token = accessToken;
                    }

                    return Task.CompletedTask;
                };
            });
        }
    }
}
