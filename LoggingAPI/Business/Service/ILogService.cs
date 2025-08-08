using Business.Dtos;
using classLib.LogDtos;

namespace LoggingAPI.Business.Service
{
    public interface ILogService
    {
        Task LogAuthAsync(Log userLog);
        Task<IEnumerable<LogInfo>> GetAllLogsAsync();
        Task AddLogAsync(Log log);
    }
}
