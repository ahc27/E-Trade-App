using Business.Dtos;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LoggingAPI.Data
{
    public class LogRepository : ILogRepository
    {
        private readonly IMongoCollection<LogInfo> _collection;
        public LogRepository(IOptions<MongoDbSettings> options)
        {
            var _mongoDbSettings = options.Value;
            var client = new MongoClient(_mongoDbSettings.ConnectionString);
            var db = client.GetDatabase(_mongoDbSettings.Database);
            _collection = db.GetCollection<LogInfo>(_mongoDbSettings.Collection);
        }

        public async Task AddAsync(LogInfo log)
        {
            await _collection.InsertOneAsync(log);
        }

        public async Task<List<LogInfo>> GetAllAsync()
        {
            return await _collection.Find(Builders<LogInfo>.Filter.Empty).ToListAsync();
        }

        public async Task LogAuthEvent(LogInfo logInfo)
        {
            try { 
            await _collection.InsertOneAsync(logInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Logging failed: {ex.Message}");
                throw;
            }
        }

    }

}
