// Generated by https://quicktype.io
namespace MyCourse.Models.Options
{
    public partial class CoursesOptions
    {
        public int PerPage { get; set; }
        public CoursesOrderOptions Order { get; set; }
        public int CacheExpiration { get; set; }
    }

    public partial class CoursesOrderOptions
    {
        public string By { get; set; }
        public bool Ascending { get; set; }
        public string[] Allow { get; set; }
    }
}