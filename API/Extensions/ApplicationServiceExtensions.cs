﻿using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration config)
        {
            //services.AddDbContext<DataContext>(opt =>
            //{
            //    opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            //});

            services.AddDbContext<DataContext>(opt =>
            {
                // Assuming UseMySQL, but check your package's documentation
                opt.UseMySQL(config.GetConnectionString("DefaultConnection"));
            });

            services.AddCors();
            services.AddScoped<ITokenService, TokenService>();
            return services;
        }


    }
}
