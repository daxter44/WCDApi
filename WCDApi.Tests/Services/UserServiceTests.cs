using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
    public class UserServiceTests : IClassFixture<DataContextFixture>
    {
        public DataContextFixture _fixture;
        public IOptions<MailSettings> _mailOptions;
        public UserServiceTests()
        {

            _mailOptions = Options.Create<MailSettings>(new MailSettings { SMTPServer = "poczta.interia.pl", Port = 465, Pass = "Ogien2020", Mail = "fireapp@interia.pl" });
            _fixture = new DataContextFixture();
        }
        [Fact]
        public void GetAllUser_ReturnsUserCollections()
        {
            MailSettings mail = new MailSettings { Pass="Ogien"};
            UserService userService = new UserService(_fixture.context, _mailOptions);
            ICollection<User> user = userService.GetAll().Result;
            Assert.Equal(3, user.Count);
        }
        [Fact]
        public async Task Create_ValidUser_ReturnsCreatedUser()
        {
            UserService userService = new UserService(_fixture.context, _mailOptions);
            User user = new User();
            user.EMail = "address4@wp.pl";
            await userService.Create(user);
            Assert.Equal(4, _fixture.context.Users.ToListAsync<User>().Result.Count);
            Assert.Contains(user, _fixture.context.Users.ToListAsync<User>().Result);
        }
        [Fact]
        public async Task Create_InValidUser_ReturnsCreatedUser()
        {
            UserService userService = new UserService(_fixture.context, _mailOptions);
            User user = new User();
            user.EMail = "adress1@wp.pl";
            await Assert.ThrowsAsync<AppException>(() => userService.Create(user));
        }
        [Fact]
        public void AuthenticateValidUser_ReturnsUser()
        {
            UserService userService = new UserService(_fixture.context, _mailOptions);
            Assert.NotNull(userService.Authenticate("adress1@wp.pl", "pass").Result);
        }
        [Fact]
        public void AuthenticateInValidUser_ReturnsUser()
        {
            UserService userService = new UserService(_fixture.context, _mailOptions);
            if (userService.Authenticate("adress1@wp.pl", "pass1") != null)
            {
                User user = userService.Authenticate("adress1@wp.pl", "pass1").Result;
                Assert.Null(user);
            }

        }

    }
}
