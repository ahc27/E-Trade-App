namespace UserMicroservice.Services.Dtos
{
    public class CreateUserdto
    {
        public string firstName { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string phoneNumber { get; set; }
        public string address { get; set; }
        public bool isSeller { get; set; }
    }
}
