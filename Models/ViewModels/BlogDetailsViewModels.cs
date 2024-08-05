using BlogApp.Models.Domain;

namespace BlogApp.Models.ViewModels
{
    public class BlogDetailsViewModels
    {
        public Guid Id { get; set; }
        
        public string Heading { get; set; }

        public string PageTitle { get; set; }

        public string Content { get; set; }

        public string ShortDescription { get; set; }

        public string FeaturedImageUrl { get; set; }

        public string UrlHandle { get; set; }

        public DateTime PublishedDate { get; set; }

        public string Author { get; set; }

        public bool Visible { get; set; }

        public ICollection<Tag> Tags { get; set; } //Reference Navigation Property

        public int TotalLikes { get; set; }
        public bool Liked { get; set; }

        public string CommentDescription { get; set; }
        public IEnumerable<BlogComment> Comments { get; set; }

    }
}