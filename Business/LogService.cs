using System;
using System.Threading.Tasks;
using EternalBAND.DataAccess;
using EternalBAND.DomainObjects;

namespace EternalBAND.Business
{
    public class LogService
    {
        private ApplicationDbContext _entities;

        public LogService(ApplicationDbContext entities)
        {
            _entities = entities;
        }

        public async Task LogMethod(string type, string value, string pageOrMethod)
        {
            await _entities.Logs.AddAsync(new Logs()
            {
                Type = type,
                Date = DateTime.Now,
                Value = value,
                PageOrMethod = pageOrMethod
            });
        }

        public async Task ErrorLogMethod(string message, Exception exception, string pageOrMethod)
        {
            await _entities.ErrorLogs.AddAsync(new ErrorLogs()
            {
                PageOrMethod = pageOrMethod,
                Date = DateTime.Now,
                LongMessage = exception.Message,
                Message = message
            });
        }
    }
}