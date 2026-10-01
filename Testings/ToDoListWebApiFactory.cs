using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;
using ToDoListWebApi;

namespace Testings
{
    public class ToDoListWebApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        readonly PostgreSqlContainer container = new PostgreSqlBuilder("postgres:18-trixie").Build();

        public async Task InitializeAsync()
        {
            await container.StartAsync();

            using IServiceScope scope = Services.CreateScope();
            DbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await dbContext.Database.MigrateAsync();
        }

        public new async Task DisposeAsync()
        {
            await container.DisposeAsync();
            await base.DisposeAsync(); // Must be called as new hides the base method otherwise test host will leak
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<IDbContextOptionsConfiguration<ApplicationDbContext>>();
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseNpgsql(container.GetConnectionString());
                });
            });

            builder.UseEnvironment("Development");
        }
    }
}