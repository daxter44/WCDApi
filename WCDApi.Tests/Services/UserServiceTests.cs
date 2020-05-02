using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WCDApi.Data;
using WCDApi.Entity;
using WCDApi.Helpers;
using WCDApi.Services;
using WCDApi.Tests.TestsFixture;
using Xunit;
using Assert = Xunit.Assert;

namespace WCDApi.Tests.Services
{
    public class UserServiceTests : IClassFixture<DataContextFixture>
    {
        public DataContextFixture _fixture;
        public MapperConfiguration _mapperConfiguration;
        public UserServiceTests(DataContextFixture fixture)
        { 
            _fixture = fixture;
        }
        [Fact]
        public void GetAllUser_ReturnsUserCollections()
        {
            UserService userService = new UserService(_fixture.context);
            ICollection<User> user = userService.GetAll().Result;
            Assert.Equal(3, user.Count);
        }
        [Fact]
        public async Task Create_ValidUser_ReturnsCreatedUser()
        {
            UserService userService = new UserService(_fixture.context);
            User user = new User();
            user.EMail = "address4@wp.pl";
            await userService.Create(user);
            Assert.Equal(4, _fixture.context.Users.ToListAsync<User>().Result.Count);
            Assert.Contains(user, _fixture.context.Users.ToListAsync<User>().Result);           
        }
        [Fact]
        public async Task Create_InValidUser_ReturnsCreatedUser()
        {
            UserService userService = new UserService(_fixture.context);
            User user = new User();
            user.EMail = "adress1@wp.pl";
            await Assert.ThrowsAsync<AppException>(() => userService.Create(user));
        }
        [Fact]
        public void AuthenticateValidUser_ReturnsUser()
        {
            UserService userService = new UserService(_fixture.context);
            Assert.NotNull(userService.Authenticate("adress1@wp.pl", "pass").Result);
        }
        [Fact]
        public void AuthenticateInValidUser_ReturnsUser()
        {
            UserService userService = new UserService(_fixture.context);
            if (userService.Authenticate("adress1@wp.pl", "pass1")!= null)
            {
                User user = userService.Authenticate("adress1@wp.pl", "pass1").Result;
                Assert.Null(user);
            }
           
        }

    }
}
