using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MMLib.SwaggerForOcelot.Configuration;
using Newtonsoft.Json;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ApiGateway
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
            //var Region = Configuration["AWSCognito:Region"];
            //var PoolId = Configuration["AWSCognito:PoolId"];
            //var AppClientId = Configuration["AWSCognito:AppClientId"];

            //Action<JwtBearerOptions> options = o =>
            //{
            //    o.RequireHttpsMetadata = false;
            //    o.SaveToken = true;
            //    o.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
            //        {
            //            // Get JsonWebKeySet from AWS
            //            var json = new WebClient().DownloadString(parameters.ValidIssuer + "/.well-known/jwks.json");
            //            // Serialize the result
            //            return JsonConvert.DeserializeObject<JsonWebKeySet>(json).Keys;
            //        },
            //        ValidateIssuer = true,
            //        ValidIssuer = $"https://cognito-idp.{Region}.amazonaws.com/{PoolId}",
            //        ValidateLifetime = true,
            //        LifetimeValidator = (before, expires, token, param) => expires > DateTime.UtcNow,
            //        ValidateAudience = false,
            //        RequireExpirationTime = true
            //    };
            //};

            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                    builder.SetIsOriginAllowed(_ => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options);

            services.AddSwaggerForOcelot(Configuration);
            services.AddOcelot(Configuration)
                .AddDelegatingHandler<GatewayHeaderDelegateHandler>(true);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsEnvironment("Docker"))
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //app.UseAuthentication();

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwaggerForOcelotUI(opt =>
            {
                opt.PathToSwaggerGenerator = "/swagger/docs";
                opt.RoutePrefix = "";
            });

            app.UseOcelot().Wait();
        }
    }
}
