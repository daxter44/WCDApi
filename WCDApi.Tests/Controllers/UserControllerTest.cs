using System;
using System.Collections.Generic;
using AutoMapper;
using Moq;
using WCDApi.Controllers;
using WCDApi.Entity;
using WCDApi.Services;
using WCDApi.Helpers;
using WCDApi.Tests.TestsFixture;
using Xunit;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WCDApi.Tests.Controllers
{
    public class UserControllerTest 
    {
        private UsersController _controller;
        private MapperConfiguration _mapperConfiguration; 
        public UserControllerTest()
        {
            AppSettings app = new AppSettings() { Secret = "SECRET" }; // Sample property
                                                                                            // Make sure you include using Moq;
            var mock = new Mock<IOptions<AppSettings>>();
            // We need to set the Value of IOptions to be the SampleOptions Class
            mock.Setup(ap => ap.Value).Returns(app);
            var itemServiceMock = new Mock<IUserService>();
            itemServiceMock.Setup(service => service.GetAll()).ReturnsAsync(
                new List<User>
                {
                new User
                {
                    Id = Guid.NewGuid(),
                    EMail = "abc@wp.pl"
                }
                }
            );
            _controller = new UsersController(itemServiceMock.Object, Automapper.Mapper, mock.Object);
        }
        [Fact]
        public async Task Create_ValidUser_ReturnsCreatedUser()
        {
            var result =_controller.GetAll();
            var okResult = result.Result as OkObjectResult;

            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
        }

    }
}
