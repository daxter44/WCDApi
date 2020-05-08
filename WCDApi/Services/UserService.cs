using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WCDApi.DataBase.Data;
using WCDApi.DataBase.Entity;
using WCDApi.Helpers;
using WCDApi.Mail;

namespace WCDApi.Services
{

    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
        Task<ICollection<User>> GetAll();
        Task<User> GetById(Guid id);
        Task<User> Create(User user);
        Task<Task> Update(User user, string password = null);
        Task<Task> Delete(Guid id);
        Task<Task> GenerateNewPassword(Guid id);
    }

    public class UserService : IUserService
    {
        private DataContext _context;
        private  MailSettings _mailSettings;
        public UserService(DataContext context, IOptions<MailSettings> mailSettings)
            {
                _context = context;
                _mailSettings = mailSettings.Value;
            }
        public async Task<User> Authenticate(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var user = await _context.Users.SingleOrDefaultAsync(x => x.EMail == email).ConfigureAwait(false);
           
            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }
        public async Task<User> Create(User user)
        {
            // validation
            var password = GenerateRandomPassword();
            var anyExist= await _context.Users.AnyAsync(x => x.EMail == user.EMail).ConfigureAwait(false);
            if (anyExist)
                throw new AppException("E-mail " + user.EMail + "is already taken");
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            MailSender mail = new MailSender(_mailSettings);
            await mail.sendMail(user.EMail, password).ConfigureAwait(false);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _context.Users.Add(user);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return user;
        }
        public async Task<Task> Delete(Guid id)
        {

            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            return Task.CompletedTask;
        }
        public async Task<ICollection<User>> GetAll()
        {
            return await _context.Users.ToListAsync().ConfigureAwait(false);
        }
        public async Task<User> GetById(Guid id)
        {
            return await _context.Users.FindAsync(id).ConfigureAwait(false);
        }
        public async Task<Task> Update(User userParam, string password = null)
        {
            var user = await _context.Users.FindAsync(userParam.Id);

            if (user == null)
                throw new AppException("User not found");

            // update password if provided
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return Task.CompletedTask;
        }
        public async Task<Task> GenerateNewPassword(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            var password = GenerateRandomPassword();
            // update password if provided
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);
                MailSender mail = new MailSender(_mailSettings);
                await mail.sendMail(user.EMail, password).ConfigureAwait(false);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return Task.CompletedTask;
        }
        public static string GenerateRandomPassword(PasswordOptions opts = null)
        {
            if (opts == null) opts = new PasswordOptions()
            {
                RequiredLength = 8,
                RequiredUniqueChars = 4,
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonAlphanumeric = true,
                RequireUppercase = true
            };

            string[] randomChars = new[] {
                            "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
                            "abcdefghijkmnopqrstuvwxyz",    // lowercase
                            "0123456789",                   // digits
                            "!@$?_-"                        // non-alphanumeric
                        };
            Random rand = new Random(Environment.TickCount);
            List<char> chars = new List<char>();

            if (opts.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (opts.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (opts.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);

            if (opts.RequireNonAlphanumeric)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (int i = chars.Count; i < opts.RequiredLength
                || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
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

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

    }
}
