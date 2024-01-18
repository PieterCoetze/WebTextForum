namespace WebTextForum.Models
{
    public class Response
    {
        public dynamic? Data { get; set; }

        public bool Success { get; set; }

        public string? Message { get; set; }
    }
}
