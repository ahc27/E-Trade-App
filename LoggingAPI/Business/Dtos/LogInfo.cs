namespace Business.Dtos
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class LogInfo
    {
        public string? EntityId { get; set; }
        public string? Level { get; set; }
        public string? Action { get; set; }
        public string? Message { get; set; }
        public DateTime Timestamp { get; set; }
        public string? ServiceName { get; set; }
        public Exception? Exception { get; set; }
        public bool IsSuccess { get; set; }

    }

}
