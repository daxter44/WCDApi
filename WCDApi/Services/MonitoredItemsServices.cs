using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WCDApi.DataBase.Data;
using WCDApi.DataBase.Entity;
using WCDApi.Helpers;

namespace WCDApi.Services
{
    public interface IMonitoredItemsService
    {
        Task<ICollection<MonitoredItem>> GetAll();
        Task<MonitoredItem> GetById(Guid id);
        Task<MonitoredItem> Create(Guid UserId, MonitoredItem item);
        Task<Task> Update(MonitoredItem item);
        Task<Task> Delete(Guid id);
        Task<Task> StartMonit(Guid id);
        Task<Task> StopMonit(Guid id);
    }
    public class MonitoredItemsServices : IMonitoredItemsService
    {
        private DataContext _context;
        public MonitoredItemsServices(DataContext context)
        {
            _context = context;
        }

        public async Task<MonitoredItem> Create(Guid UserId, MonitoredItem item)
        {
            var user = _context.Users.Find(UserId);
            if (user == null)
                throw new AppException("User not found");
            if (user.MonitoredItems == null)
                user.MonitoredItems = new Collection<MonitoredItem>();
            
            item.ProcessId = ProcessManager.StartCommand();
            if (item.ProcessId == 0)
                throw new AppException("Cannot run your process");
            user.MonitoredItems.Add(item);
            _context.Entry(user).State = EntityState.Modified;
            _context.MonitoredItems.Add(item);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return item;
        }

        public async Task<Task> Delete(Guid id)
        {
            var item = await _context.MonitoredItems.FindAsync(id);
            if (item != null)
            {
                ProcessManager.StopCommand(item.ProcessId);
                _context.MonitoredItems.Remove(item);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            return Task.CompletedTask;
        }

        public async Task<ICollection<MonitoredItem>> GetAll()
        {
            return await _context.MonitoredItems.ToListAsync().ConfigureAwait(false);
        }

        public async Task<MonitoredItem> GetById(Guid id)
        {
            return await _context.MonitoredItems.FindAsync(id).ConfigureAwait(false);
        }

        public async Task<Task> StartMonit(Guid id)
        {
           MonitoredItem item = await _context.MonitoredItems.FindAsync(id).ConfigureAwait(false);
           if (item == null)
               throw new AppException("Item not found");
           item.ProcessId = ProcessManager.StartCommand();
            if (item.ProcessId == 0)
                throw new AppException("Cant run process");
            item.isActive = true;
            _context.MonitoredItems.Update(item);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return Task.CompletedTask;
        }

        public async Task<Task> StopMonit(Guid id)
        {
            MonitoredItem item = await _context.MonitoredItems.FindAsync(id).ConfigureAwait(false);
            if (item == null)
                throw new AppException("Item not found");
            bool stoppedSuccesfull = ProcessManager.StopCommand(item.ProcessId);
            if (!stoppedSuccesfull)
                throw new AppException("Cant stop process");
            item.isActive = false;
            _context.MonitoredItems.Update(item);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return Task.CompletedTask;
        }

        public async Task<Task> Update(MonitoredItem itemParam)
        {
            var item = await _context.MonitoredItems.FindAsync(itemParam.MonitItemId);

            if (item == null)
                throw new AppException("Item not found");

            _context.MonitoredItems.Update(item);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return Task.CompletedTask;
        }

    }
}
