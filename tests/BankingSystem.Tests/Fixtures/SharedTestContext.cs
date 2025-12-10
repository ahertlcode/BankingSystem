using System;
using BankingSystem.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using BankingSystem.App.Data;

namespace BankingSystem.Tests.Fixtures
{


    public class SharedTestContext : IDisposable
    {
        public CustomWebApplicationFactory Factory { get; }
        public HttpClient Client { get; }

        public SharedTestContext()
        {
            Factory = new CustomWebApplicationFactory();
            Client = Factory.CreateClient();
        }

        public AppDbContext GetDb() =>
            Factory.Services.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();

        public void Dispose()
        {
            Client.Dispose();
            Factory.Dispose();
        }
    }

}



