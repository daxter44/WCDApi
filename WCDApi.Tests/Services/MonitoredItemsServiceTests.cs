using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WCDApi.DataBase.Entity;
using WCDApi.Helpers;
using WCDApi.Services;
using WCDApi.Tests.TestsFixture;
using Xunit;
using Assert = Xunit.Assert;

namespace WCDApi.Tests.Services
{
    public class MonitoredItemsServiceTests : IClassFixture<DataContextFixture>
    {
        public DataContextFixture _fixture;
        public MonitoredItemsServiceTests(DataContextFixture fixture)
        {
            _fixture = fixture;
        }
        [Fact]
        public void GetAllItems_ReturnsUserCollections()
        {
            MonitoredItemsServices ItemsService = new MonitoredItemsServices(_fixture.context);
            ICollection<MonitoredItem> items = ItemsService.GetAll().Result;
            Assert.Equal(2, items.Count);
        }
        [Fact]
        public void CreateValidItem_ReturnsItems()
        {
            MonitoredItemsServices itemsService = new MonitoredItemsServices(_fixture.context);
            MonitoredItem item = new MonitoredItem { ElementName="CreatedTest", Url = "www.google.com", Frequency=5};
            itemsService.Create(Guid.Parse("00000000-0000-0000-0000-000000000001"),item);
            Assert.Equal(2, _fixture.context.MonitoredItems.ToListAsync<MonitoredItem>().Result.Count);
            Assert.Contains(item, _fixture.context.MonitoredItems.ToListAsync<MonitoredItem>().Result);
        }
        [Fact]
        public async Task PutInvalidUserId_ReturnsThrowException()
        {
            MonitoredItemsServices itemsService = new MonitoredItemsServices(_fixture.context);
            MonitoredItem item = new MonitoredItem { ElementName = "CreatedTest", Url = "www.google.com", Frequency = 5 };
            await Assert.ThrowsAsync<AppException>(() => itemsService.Create(Guid.Parse("00000000-0000-0000-0000-000000000002"), item));
        }
    }
}
