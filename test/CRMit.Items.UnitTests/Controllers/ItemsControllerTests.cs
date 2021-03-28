using CRMit.Items.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMit.Items.Controllers
{
    [TestFixture]
    public class ItemsControllerTests
    {
        private readonly List<Item> items = new List<Item>()
        {
            new Item { Id = 1, Name = "Laptop", Description = "A brand new laptop.", Price = 100 },
            new Item { Id = 2, Name = "Book", Description = "A book.", Price = 45 }
        };
        private readonly DbContextOptions<ItemsDbContext> dbContextOptions = new DbContextOptionsBuilder<ItemsDbContext>()
            .UseInMemoryDatabase(databaseName: "ItemsDB")
            .Options;

        private ItemsController itemsController;

        [SetUp]
        public async Task Setup()
        {
            using var context = new ItemsDbContext(dbContextOptions);
            await context.AddRangeAsync(items);
            await context.SaveChangesAsync();
            itemsController = new ItemsController(new ItemsDbContext(dbContextOptions));
        }

        [TearDown]
        public async Task TearDown()
        {
            using var context = new ItemsDbContext(dbContextOptions);
            await context.Database.EnsureDeletedAsync();
        }

        [Test]
        public async Task TestCreateItem()
        {
            using var context = new ItemsDbContext(dbContextOptions);
            var newInputDTO = new ItemInput { Name = "Bicycle", Description = "A bicycle.", Price = 200 };
            var newInput = new Item(newInputDTO) { Id = 3 };

            var result = (await itemsController.CreateItemAsync(newInputDTO)).Result as CreatedAtActionResult;
            Assert.That(result.ActionName, Is.EqualTo("GetItem"));
            Assert.That(result.RouteValues["id"], Is.EqualTo(3));
            Assert.That(result.Value, Is.EqualTo(newInput));
            var addedItem = await context.FindAsync<Item>((long)3);
            Assert.That(addedItem, Is.EqualTo(newInput));
        }

        [Test]
        public async Task TestGetList()
        {
            var result = (await itemsController.GetListAsync()).Result as JsonResult;
            var list = result.Value as IEnumerable<Item>;
            CollectionAssert.AreEqual(items, list);
        }

        [Test]
        public async Task TestGetItem()
        {
            var result = (await itemsController.GetItemAsync(1)).Result as JsonResult;
            var item = result.Value as Item;
            Assert.That(item.Name, Is.EqualTo("Laptop"));
        }

        [Test]
        public async Task TestGetItemIfNotInDB()
        {
            var result = (await itemsController.GetItemAsync(4)).Result;
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task TestUpdateItem()
        {
            using var context = new ItemsDbContext(dbContextOptions);
            var itemDTO = new ItemInput { Name = "Book2", Description = "Another book.", Price = 45 };
            var item = new Item(itemDTO) { Id = 2 };

            var result = await itemsController.UpdateItemAsync(2, itemDTO);
            Assert.That(result, Is.InstanceOf<OkResult>());
            Assert.That(context.Find<Item>((long)2), Is.EqualTo(item));
        }

        [Test]
        public async Task TestUpdateItemIfNotExists()
        {
            var item = new ItemInput();
            var result = await itemsController.UpdateItemAsync(3, item);
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task TestDeleteItem()
        {
            using var context = new ItemsDbContext(dbContextOptions);
            var id = 2;

            var result = await itemsController.DeleteItemAsync(id);

            Assert.That(result, Is.InstanceOf<OkResult>());
            Assert.That(context.Items.Any(c => c.Id == id), Is.False);
        }

        [Test]
        public async Task TestDeleteItemOnMissingItem()
        {
            using var context = new ItemsDbContext(dbContextOptions);
            var result = await itemsController.DeleteItemAsync(3);
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
            Assert.That(context.Items.Count(), Is.EqualTo(2));
        }
    }
}