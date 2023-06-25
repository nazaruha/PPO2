using Microsoft.Extensions.DependencyInjection;
using PPO2.Core.AutoMapper;
using PPO2.Core.Interfaces;
using PPO2.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPO2.Core
{
    public static class ServiceExtensions
    {
        public static void AddCoreServices(this IServiceCollection services)
        {
            // Project service
            services.AddTransient<IProjectService, ProjectService>();
            // Customer service
            services.AddTransient<ICustomerService, CustomerService>();
            // CustomerProject service
            services.AddTransient<ICustomerProjectService, CustomerProjectService>();
            // Plan service
            services.AddTransient<IPlanService, PlanService>();
            // PlanProject service
            services.AddTransient<IPlanProjectService, PlanProjectService>();
            // Manufacturer service
            services.AddTransient<IManufacturerService, ManufacturerService>();
            // Product service
            services.AddTransient<IProductService, ProductService>();
            // ProductManufacturer service
            //services.AddTransient<IProductManufacturerService, ProductManufacturerService>();
            // Storage service
            services.AddTransient<IStorageService, StorageService>();
            // Order service
            services.AddTransient<IOrderService, OrderService>();
        }

        //Add automapper
        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProjectProfile));
            services.AddAutoMapper(typeof(AutoMapperCustomerProfile));
            services.AddAutoMapper(typeof(AutoMapperPlanProfile));
            services.AddAutoMapper(typeof(AutoMapperManufacturerProfile));
            services.AddAutoMapper(typeof(AutoMapperProductProfile));
            services.AddAutoMapper(typeof(AutoMapperStorageProfile));
            services.AddAutoMapper(typeof(AutoMapperOrderProfile));
        }

        public static void CorsConfiguration(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
        }
    }
}
