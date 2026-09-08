namespace portfolioApi.DTOs
{
    public class ProjectResponseDto
    {
        public int Id { get; set; }
        public string TitleProject { get; set; }
        public string Description { get; set; }
        public string UrlImage { get; set; }
        public string Status { get; set; }
        public string Technologies { get; set; }
    }
}
