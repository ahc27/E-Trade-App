using AutoMapper;
using Business.Dtos;
using classLib.LogDtos;
using LoggingAPI.Data;

namespace LoggingAPI.Business.Service
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;
        private readonly IMapper _mapper;

        public LogService(ILogRepository logRepository, IMapper mapper)
        {
            _logRepository = logRepository;
            _mapper = mapper;
        }

        public async Task LogAuthAsync(Log userLog)
        {
            if (userLog == null) throw new ArgumentNullException(nameof(userLog), "User log cannot be null");

            var log = _mapper.Map<LogInfo>(userLog);
            await _logRepository.LogAuthEvent(log);
        }
        public async Task<IEnumerable<LogInfo>> GetAllLogsAsync()
        {
            return await _logRepository.GetAllAsync();
        }
        
        public async Task AddLogAsync(Log logInfo)
        {
            if (logInfo == null) throw new ArgumentNullException(nameof(logInfo), "Log cannot be null");

            var log = _mapper.Map<LogInfo>(logInfo);
            await _logRepository.AddAsync(log);
        }
    }
}
