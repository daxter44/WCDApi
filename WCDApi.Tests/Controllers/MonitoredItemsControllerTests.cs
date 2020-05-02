using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WCDApi.Controllers;
using WCDApi.Entity;
using WCDApi.Helpers;
using WCDApi.Model.MonitoredItems;
using WCDApi.Services;
using Xunit;

namespace WCDApi.Tests.Controllers
{
    public class MonitoredItemsControllerTests
    {
        private MonitoredItemsController _controller;
        public MonitoredItemsControllerTests()
        {
            
        }
        [Fact]
        public async Task GetAllItems_ReturnsStatus200()
        {
            //Arrange
            var itemServiceMock = new Mock<IMonitoredItemsService>();
            itemServiceMock.Setup(service => service.GetAll()).ReturnsAsync(
                new List<MonitoredItem>
                {
                new MonitoredItem
                {
                    MonitItemId = Guid.NewGuid(),
                    Url = "abc@wp.pl"
                }
                }
            );
            _controller = new MonitoredItemsController(itemServiceMock.Object, Automapper.Mapper);

            //Act
            var result = _controller.GetAll();
            var okResult = result.Result as OkObjectResult;

            //Assert
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }
        //[Fact]
        //public async Task CreateValidItem_ReturnsStatus200()
        //{
        //    //Arrange
        //    var itemServiceMock = new Mock<IMonitoredItemsService>();
        //    itemServiceMock.Setup(service => service.Create(It.IsAny<Guid>(), It.IsAny<MonitoredItem>())).Callback(() => new MonitoredItem { Url="test.pl", ElementName="elementName", Frequency=3} );
        //    _controller = new MonitoredItemsController(itemServiceMock.Object, Automapper.Mapper);
        //    CreateItemModel itemModel = new CreateItemModel { Url = "test.pl", ElementName = "elementName", Frequency = 4 };

        //    //Act
        //    ActionResult<MonitoredItem> result = _controller.AddMonitoredItemToCurrUser(itemModel).Result;

        //    var okResult = result.Result as OkObjectResult;

        //    //Assert
            
        //    Assert.NotNull(okResult);
        //    Assert.Equal(200, okResult.StatusCode);
        //}

    }
}
