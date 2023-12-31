﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ordering.API.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication MigrateDatabase<TContext>(this WebApplication app,
        Action<TContext, IServiceProvider> seeder, int? retry = 0) where TContext : DbContext
    {
        int retryForAvailability = retry.Value;

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<TContext>>();
            var context = services.GetRequiredService<TContext>();

            try
            {
                logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

                InvokeSeeder(seeder, context, services);
                
                logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
            }
            catch (SqlException ex)
            {
                logger.LogError(ex, "An error occured while migrating the database used on context {DbContextName}", typeof(TContext).Name);

                if (retryForAvailability < 50)
                {
                    retryForAvailability++;
                    System.Threading.Thread.Sleep(2000);
                    MigrateDatabase(app, seeder, retryForAvailability);
                }
            }
        }
        return app;
    }

    private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext context,
        IServiceProvider services) where TContext : DbContext
    {
        context.Database.Migrate();
        seeder(context, services);
    }
}