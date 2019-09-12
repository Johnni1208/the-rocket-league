using System;
using System.Text;
using System.Threading;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using The_Rocket_League_Backend.Data;

namespace The_Rocket_League_Backend{
    public class Startup{
        public Startup(IConfiguration configuration){
            Configuration = configuration;
        }

        public IConfiguration Configuration{ get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services){
            var dbConnectionString = Configuration.GetConnectionString("DefaultConnectionString");
            WaitForDbStartup(dbConnectionString);
            services.AddDbContext<DataContext>(x =>
                x.UseMySql(dbConnectionString));

            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IRocketRepository, RocketRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(opt => {
                    opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters{
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                            .GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }

        private static void WaitForDbStartup(string connectionString){
            var connection = new MySqlConnection(connectionString);
            var retries = 1;
            while (retries < 7)
            {
                try
                {
                    Console.WriteLine("Connecting to db. Trial: {0}", retries);
                    connection.Open();
                    connection.Close();
                    break;
                }
                catch (MySqlException)
                {
                    Thread.Sleep((int) Math.Pow(2, retries) * 1000);
                    retries++;
                }
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DataContext context){
            if (env.IsDevelopment()){
                app.UseDeveloperExceptionPage();
            }
            else{
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//                app.UseHsts();
            }

//            app.UseHttpsRedirection();
            context.Database.Migrate();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}