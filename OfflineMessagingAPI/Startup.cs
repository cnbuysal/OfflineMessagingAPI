using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OfflineMessagingAPI.DataAccess;
using OfflineMessagingAPI.Entities.Entities;
using OfflineMessagingAPI.DataAccess.Abstract;
using OfflineMessagingAPI.DataAccess.Concrete;
using OfflineMessagingAPI.Business.Abstract;
using OfflineMessagingAPI.Business.Concrete;

namespace OfflineMessagingAPI
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
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IBlockRepository, BlockRepository>();
            services.AddTransient<IMessageRepository, MessageRepository>();
            services.AddTransient<IUserServices, UserServices>();
            services.AddTransient<IMessageServices, MessageServices>();
            services.AddTransient<IBlockServices, BlockServices>();

            services.AddDbContext<OfflineMessagingDbContext>(option => option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("OfflineMessagingAPI")));

            services.AddIdentity<User, IdentityRole<int>>(options =>
            {
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 1;
            }).AddEntityFrameworkStores<OfflineMessagingDbContext>();

            services.AddSwaggerDocument(config => config.PostProcess = (doc => doc.Info.Title = "Offline Messaging API")); 

            services.AddControllers().AddNewtonsoftJson(options => 
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
