using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Newtonsoft.Json;
using Prometheus;
using Stock.API.Logging;
using Stock.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Stock.API
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
            var Region = Configuration["AWSCognito:Region"];
            var PoolId = Configuration["AWSCognito:PoolId"];
            var AppClientId = Configuration["AWSCognito:AppClientId"];

            #region DynamoDB
            //var AWSDynamoDBAccessKey = Configuration["AWSDynamoDB:AccessKey"];
            //var AWSDynamoDBSecretKey = Configuration["AWSDynamoDB:SecretKey"];
            //var AWSDynamoDBServiceURL = Configuration["AWSDynamoDB:ServiceURL"];

            //var credentials = new BasicAWSCredentials(AWSDynamoDBAccessKey, AWSDynamoDBSecretKey);
            //var config = new AmazonDynamoDBConfig()
            //{
            //    RegionEndpoint = RegionEndpoint.USEast2,
            //    ServiceURL = AWSDynamoDBServiceURL
            //};
            //var client = new AmazonDynamoDBClient(credentials, config);
            //services.AddSingleton<IAmazonDynamoDB>(client);
            //services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
            #endregion

            Action<JwtBearerOptions> options = o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
                    {
                        // Get JsonWebKeySet from AWS
                        var json = new WebClient().DownloadString(parameters.ValidIssuer + "/.well-known/jwks.json");
                        // Serialize the result
                        return JsonConvert.DeserializeObject<JsonWebKeySet>(json).Keys;
                    },
                    ValidateIssuer = true,
                    ValidIssuer = $"https://cognito-idp.{Region}.amazonaws.com/{PoolId}",
                    ValidateLifetime = true,
                    LifetimeValidator = (before, expires, token, param) => expires > DateTime.UtcNow,
                    ValidateAudience = false,
                    RequireExpirationTime = true
                };
            };

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Stock API", Version = "v1" });
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme()
                {
                    Description = @"JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                          {
                              {
                                  new OpenApiSecurityScheme
                                  {
                                      Reference = new OpenApiReference
                                      {
                                          Type = ReferenceType.SecurityScheme,
                                          Id = JwtBearerDefaults.AuthenticationScheme
                                      },
                                      Scheme = "oauth2",
                                      Name = JwtBearerDefaults.AuthenticationScheme,
                                      In = ParameterLocation.Header
                                  },
                                  new string[] {}
                              }
                          });
            });

            services.AddApiVersioning();

            services.AddControllers();

            services.AddHttpContextAccessor();

            services.Configure<StockDatabaseSettings>(
                Configuration.GetSection(nameof(StockDatabaseSettings)));

            services.AddSingleton<IStockDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<StockDatabaseSettings>>().Value);

            services.AddSingleton<IStockService, StockService>();

            //services.AddSingleton<IDynamoDBService, DynamoDBService>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            StockLogEnricher.ServiceProvider = app.ApplicationServices;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseMetricServer();

            app.UseHttpMetrics();

            app.UseRequestMiddleware();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Stock API V1");
                c.RoutePrefix = "";
            });
        }
    }
}
