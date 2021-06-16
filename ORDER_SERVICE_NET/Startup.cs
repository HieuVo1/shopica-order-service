using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ORDER_SERVICE_NET.Hubs;
using ORDER_SERVICE_NET.Models;
using ORDER_SERVICE_NET.Services.CartServices;
using ORDER_SERVICE_NET.Services.NotifyServices;
using ORDER_SERVICE_NET.Services.OrderServices;
using ORDER_SERVICE_NET.Services.ProductServices;
using ORDER_SERVICE_NET.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ORDER_SERVICE_NET
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
            services.AddCors(option =>
            {
                option.AddPolicy("CorsPolicy", builder => builder
                .WithOrigins("http://localhost:4200", "https://shopica.herokuapp.com","https://hieuvo1.github.io")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                );
            });

            services.AddControllers();
            services.AddDbContext<ShopicaContext>(options =>
               options.UseMySQL(Configuration.GetConnectionString(Constant.ConnectionString)));

            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<INotifyService, NotifyService>();

            services.AddSingleton<IUserIdProvider, StoreIdProvider>();

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
          .AddJwtBearer(options =>
          {
              options.RequireHttpsMetadata = false;
              options.SaveToken = true;
              options.TokenValidationParameters = new TokenValidationParameters()
              {
                  ValidateIssuer = false,
                  ValidateAudience = false,
                  ValidateLifetime = true,
                  ValidateIssuerSigningKey = true,
                  ClockSkew = TimeSpan.Zero,
                  IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration.GetSection("SecretKey").Value))
              };

              //hub config
              options.Events = new JwtBearerEvents
              {
                  OnMessageReceived = context =>
                  {
                      var accessToken = context.Request.Query["access_token"];

                      // If the request is for our hub...
                      var path = context.HttpContext.Request.Path;
                      if (!string.IsNullOrEmpty(accessToken) &&
                          (path.StartsWithSegments("/orderNotifys")))
                      {
                          context.Token = accessToken;
                      }
                      return Task.CompletedTask;
                  }
              };
          });

            services.AddHttpClient<IProductService, ProductService>(client =>
            {
                client.BaseAddress = new Uri(Configuration.GetSection("ProductServiceUrl").Value);
            });

            services.AddSignalR();

            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseCors("CorsPolicy");

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHub<NotificationHub>("/orderNotifys");
            });
        }
    }
}
