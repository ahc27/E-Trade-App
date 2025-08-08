using Business.Dtos;
using classLib;

namespace LoggingAPI.Data
{
    public interface ILogRepository
    {
        public Task AddAsync(LogInfo log);
        public Task<List<LogInfo>> GetAllAsync();

        public Task LogAuthEvent(LogInfo logInfo);

    }
}
