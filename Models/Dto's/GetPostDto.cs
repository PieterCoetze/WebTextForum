namespace WebTextForum.Models.Dto_s
{
    public class GetPostDto
    {
        public int? PostId { get; set; }
        public int? CreatedBy { get; set; }
        public string? CreatedByUsername { get; set; }
        public DateTime? StartdDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int[]? Flags { get; set; }
        public string? SortBy { get; set; }
        public bool SortDecending { get; set; } = false;
        public int PageSize { get; set; } = 20;
        public int PageNo { get; set; } = 1;
    }
}
