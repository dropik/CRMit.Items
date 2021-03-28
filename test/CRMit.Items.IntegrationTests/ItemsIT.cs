using CRMit.Items.Models;
using CRMit.Items.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CRMit.Items.IntegrationTests
{
    [TestFixture]
    public class ItemsIT
    {
        private HttpClient client;

        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            var server = CreateTestServer();
            await ExecuteStartupTasks(server);
            client = server.CreateClient();
        }

        private static TestServer CreateTestServer() => new(new WebHostBuilder()
            .UseEnvironment("Development")
            .ConfigureAppConfiguration(builder =>
            {
                builder.AddJsonFile("appsettings.json", optional: true)
                       .AddJsonFile("appsettings.Development.json", optional: true);
            })
            .UseStartup<Startup>());

        private static async Task ExecuteStartupTasks(TestServer server)
        {
            var tasks = server.Services.GetServices<IStartupTask>();
            foreach (var task in tasks)
            {
                await task.ExecuteAsync();
            }
        }

        [SetUp]
        public async Task Setup()
        {
            var connectionString = "Server=localhost; Database=ItemsDB; User ID=sa; Password=Password1234;";
            using var context = new ItemsDbContext(
                new DbContextOptionsBuilder<ItemsDbContext>()
                    .UseSqlServer(connectionString)
                    .Options);
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
        }

        [Test]
        public async Task Test_CreateItem_AndGetItem_GivenReturnedLocation()
        {
            (var item, var response) = await PostLaptop();
            await EnsureItemCreated(item, response);
        }

        private async Task<(ItemInput item, HttpResponseMessage response)> PostLaptop()
        {
            var item = new ItemInput { Name = "Laptop", Description = "A brand new laptop.", Price = 100 };
            var response = await client.PostAsJsonAsync("/crmit/v1/items/", item);
            response.EnsureSuccessStatusCode();
            return (item, response);
        }

        private async Task EnsureItemCreated(ItemInput item, HttpResponseMessage prevResponse)
        {
            var response = await client.GetAsync(prevResponse.Headers.Location);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<Item>();
            var expectedItem = new Item(item) { Id = 1 };
            Assert.That(result, Is.EqualTo(expectedItem));
        }

        [Test]
        public async Task Test_CreateItem_GivenMissingProperties_ResultsInBadRequest()
        {
            var newItem = new ItemInput { Description = "A brand new laptop." };
            var response = await client.PostAsJsonAsync("/crmit/v1/items/", newItem);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task Test_CreateItem_GivenDuplicatedName_ResultsInBadRequest()
        {
            await PostLaptop();
            await EnsureDuplicatedNameIsNotPermited();
        }

        private async Task EnsureDuplicatedNameIsNotPermited()
        {
            var item2 = new ItemInput { Name = "Laptop", Description = "Should be another laptop.", Price = 200 };
            var response = await client.PostAsJsonAsync("/crmit/v1/items/", item2);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task Test_UpdateItem_GivenDuplicatedName_ResultsInBadRequest()
        {
            await PostLaptop();
            var item = await PostBook();
            await EnsureNameDuplicateIsNotPermitedOnEdit(item);
        }

        private async Task<ItemInput> PostBook()
        {
            var item = new ItemInput { Name = "Book", Description = "A book.", Price = 45 };
            var response = await client.PostAsJsonAsync("/crmit/v1/items/", item);
            response.EnsureSuccessStatusCode();
            return item;
        }

        private async Task EnsureNameDuplicateIsNotPermitedOnEdit(ItemInput item)
        {
            item.Name = "Laptop";
            var response = await client.PutAsJsonAsync("/crmit/v1/items/2/", item);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task Test_DeleteItem_IsSuccessful_AndResultsInNotFound()
        {
            await PostLaptop();
            await DeleteLaptop();
            await EnsureDeletedItemIsNotFound();
        }

        private async Task DeleteLaptop()
        {
            var response = await client.DeleteAsync("/crmit/v1/items/1/");
            response.EnsureSuccessStatusCode();
        }

        private async Task EnsureDeletedItemIsNotFound()
        {
            var response = await client.GetAsync("/crmit/v1/items/1/");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}