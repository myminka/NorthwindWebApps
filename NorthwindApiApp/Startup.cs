using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Northwind.Services.Interfaces;
using Northwind.Services.EntityFramework.Services;
using System.Data.SqlClient;
using Northwind.Services.DataAccess.Services;
using Northwind.DataAccess.Interfaces;
using Northwind.DataAccess.SqlServer;
using Northwind.DataAccess.Employees;
using Northwind.Services.Employees;
using Northwind.Services.EntityFrameworkCore.Context;
using Microsoft.EntityFrameworkCore;
using Northwind.Services.Blogging;

namespace NorthwindApiApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddSingleton<IProductManagementService, ProductManagementService>();
            services.AddSingleton<IProductCategoryManagementService, ProductCategoryManagementService>();
            services.AddSingleton<IProductCategoryPicturesService, ProductCategoryPicturesService>();
            services.AddSingleton<IEmployeeManagementService, EmployeeManagementService>();

            services.AddScoped<IProductDataAccessObject, ProductManagementDataAccessService>();
            services.AddScoped<IProductCategoryDataAccessObject, ProductCategoriesManagementDataAccessService>();
            services.AddScoped<IProductCategoryPicturesDataAccessObject, ProductCategoryPictureManagementDataAccessService>();
            services.AddScoped<IEmployeeDataAccessObject, EmployeeManagementDataAccessService>();

            services.AddScoped((service) =>
            {
                var sqlConnection = new SqlConnection(this.Configuration["ConnectionString"]);
                sqlConnection.Open();
                return sqlConnection;
            });

            services.AddTransient<NorthwindDataAccessFactory, SqlServerDataAccessFactory>();

            services.AddDbContext<NorthwindContext>(options => options.UseSqlServer(this.Configuration.GetConnectionString("ConnectionString")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
