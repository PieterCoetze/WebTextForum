namespace WebTextForum.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public string PostText { get; set; }
        public int Likes { get; set; }
        public Comment[] Comments { get; set; }
        public PostFlag[] Flags { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
