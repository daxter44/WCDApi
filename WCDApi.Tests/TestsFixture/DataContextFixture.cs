using Microsoft.EntityFrameworkCore;
using System;
using WCDApi.DataBase.Data;
using WCDApi.DataBase.Entity;

namespace WCDApi.Tests.TestsFixture
{
    public class DataContextFixture : IDisposable
    {
        public DataContext context { get; private set; }

        public DataContextFixture()
        {

            var options = new DbContextOptionsBuilder<DataContext>()
                                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                                .Options;
            context = new DataContext(options);
            
            if (!context.Users.AnyAsync<User>().Result) {
                AddUsers();
            }
            if (!context.MonitoredItems.AnyAsync<MonitoredItem>().Result)
            {
                AddMonitoredItems();
            }

        }

        public void Dispose()
        {
            context.Dispose();
        }
        private void AddUsers()
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash("pass", out passwordHash, out passwordSalt);
            context.Users.Add(new User { Id = Guid.Parse("00000000-0000-0000-0000-000000000001"), EMail = "adress1@wp.pl", Role = Role.User, PasswordHash = passwordHash, PasswordSalt = passwordSalt });
            context.Users.Add(new User { Id = Guid.NewGuid(), EMail = "adress2@wp.pl", Role = Role.User });
            context.Users.Add(new User { Id = Guid.NewGuid(), EMail = "adress3@wp.pl", Role = Role.User });
            context.SaveChanges();
        }
        private void AddMonitoredItems()
        {
            User user = context.Users.FirstOrDefaultAsync<User>().Result;
            context.MonitoredItems.Add(new MonitoredItem { MonitItemId = Guid.NewGuid(), ElementName = "test", Url = "www.google.com", EndMonitDate = DateTime.Now, Frequency = 3, StartMonitDate = DateTime.Now, User = user });
            context.SaveChanges();
        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
