using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using BankingSystem.App.Data;


namespace BankingSystem.Tests.Infrastructure
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {

        private SqliteConnection _sqliteConnection;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove real DB registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>)
                );
                if (descriptor != null)
                    services.Remove(descriptor);

                // Create one shared in-memory SQLite connection
                _sqliteConnection = new SqliteConnection("DataSource=:memory:");
                _sqliteConnection.Open();

                // Register test DB context
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseSqlite(_sqliteConnection);
                });

                // Build provider and ensure schema
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                db.Database.EnsureCreated();

                // Seed initial test data
                SeedData.Initialize(db);
            });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                _sqliteConnection?.Close();
                _sqliteConnection?.Dispose();
            }
        }
    }
}
