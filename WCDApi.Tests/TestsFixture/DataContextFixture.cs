using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WCDApi.Data;
using WCDApi.Entity;

namespace WCDApi.Tests.TestsFixture
{
    public class DataContextFixture : IDisposable
    {
        public DataContext context { get; private set; }

        public DataContextFixture()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                                .UseInMemoryDatabase(databaseName: "Database")
                                .Options;
            context = new DataContext(options);

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash("pass", out passwordHash, out passwordSalt);
            context.Users.Add(new User { Id = Guid.NewGuid(), EMail = "adress1@wp.pl", Role = Role.User, PasswordHash = passwordHash, PasswordSalt = passwordSalt });
            context.Users.Add(new User { Id = Guid.NewGuid(), EMail = "adress2@wp.pl", Role = Role.User });
            context.Users.Add(new User { Id = Guid.NewGuid(), EMail = "adress3@wp.pl", Role = Role.User });
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
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
