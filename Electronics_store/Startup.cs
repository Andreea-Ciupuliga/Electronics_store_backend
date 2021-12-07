using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Electronics_store.Data;
using Electronics_store.Repositories.CategoryRepository;
using Electronics_store.Repositories.UserRepository;
using Electronics_store.Services;
using Electronics_store.Services.CategoryService;
using Electronics_store.Services.UserService;
//using Electronics_store.Utilities;
//using Electronics_store.Utilities.JWTUtils;
using Microsoft.EntityFrameworkCore;

namespace Electronics_store
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

            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Electronics_store", Version = "v1" });
            });
            
            
            //repositories
            
            //se creeaza de fiecare data cand se face un request
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            
            //se creeaza cand se face primul request
            //services.AddSingleton<IDatabaseRepository, DatabaseRepository>();
            
            //se creeaza o data per client requeat
            //services.AddScoped<IDatabaseRepository, DatabaseRepository>();



            //services
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddDbContext<ElectronicsStoreContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            //services.AddScoped<IJWTUtils, JWTUtils>();
            services.AddScoped<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Electronics_store v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

           // app.UseMiddleware<JWTMiddleware>();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
