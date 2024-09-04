using BlogApp.Models.Domain;
using BlogApp.Models.ViewModels;
using BlogApp.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository _blogPostRespository;
        private readonly IBlogPostLikeRepository _blogPostLikeRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        private readonly IBlogPostCommentRepository _blogPostCommentRepository;


        public BlogsController(IBlogPostRepository blogPostRepository, IBlogPostLikeRepository blogPostLikeRepository, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IBlogPostCommentRepository blogPostCommentRepository)
        {
            _blogPostRespository = blogPostRepository;
            _blogPostLikeRepository = blogPostLikeRepository;
            _signInManager = signInManager;
            _userManager = userManager;
            _blogPostCommentRepository = blogPostCommentRepository;

        }
        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var blogPost = await _blogPostRespository.GetByUrlHandleAsync(urlHandle);
            var liked = false;


            if (blogPost != null)
            {
                var totalLikes = await _blogPostLikeRepository.GetTotalLikes(blogPost.Id);

                if (_signInManager.IsSignedIn(User))
                {
                    // get like for this blog for this user
                    var blogPostLikes = await _blogPostLikeRepository.GetLikesForBlog(blogPost.Id);

                    var userId = _userManager.GetUserId(User);
                    if (userId != null)
                    {
                        var likeFromUser = blogPostLikes.FirstOrDefault(x => x.UserId == Guid.Parse(userId));
                        liked = likeFromUser != null;
                    }
                }
                // Get comments for blog posts
                var blogPostComments = await _blogPostCommentRepository.GetCommentsByBlogIdAsync(blogPost.Id);

                var comments = new List<BlogComment>();

                foreach (var blogPostComment in blogPostComments)
                {
                    var userId = blogPostComment.UserId.ToString();
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        comments.Add(
                            new BlogComment
                            {
                                Description = blogPostComment.Description,
                                Date = blogPostComment.DateAdded,
                                Username = user.UserName
                            }
                        );
                    }

                }

                var model = new BlogDetailsViewModels
                {
                    Id = blogPost.Id,
                    Heading = blogPost.Heading,
                    PageTitle = blogPost.PageTitle,
                    Content = blogPost.Content,
                    ShortDescription = blogPost.ShortDescription,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    UrlHandle = blogPost.UrlHandle,
                    PublishedDate = blogPost.PublishedDate,
                    Author = blogPost.Author,
                    Tags = blogPost.Tags,
                    TotalLikes = totalLikes,
                    Liked = liked,
                    Comments = comments
                };
                return View(model);
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Index(BlogDetailsViewModels blogDetailsViewModels)
        {
            if (_signInManager.IsSignedIn(User))
            {
                var domainModel = new BlogPostComment
                {
                    BlogPostId = blogDetailsViewModels.Id,
                    Description = blogDetailsViewModels.CommentDescription,
                    UserId = Guid.Parse(_userManager.GetUserId(User)),
                    DateAdded = DateTime.Now
                };
                await _blogPostCommentRepository.AddAsync(domainModel);
                return RedirectToAction("Index", "Blogs", new { urlHandle = blogDetailsViewModels.UrlHandle });
            }
            return View();

        }
        [HttpGet]
        public async Task<IActionResult> DeleteComment(string commentDesc, DateTime commentDate,string userId,Guid blogId, string urlHandle)
        {   
            bool IsAdmin = _signInManager.IsSignedIn(User) && User.IsInRole("Admin");
            var blogPostComment = await _blogPostCommentRepository.DeleteAsync(commentDesc,commentDate,userId,blogId,IsAdmin);

            if(blogPostComment == null)
                return RedirectToAction("Error","Home");
            return RedirectToAction("Index", new { urlHandle });
        }

        // [HttpGet]
        // public async Task<IActionResult> EditComment()
        // {
        //     _blogPostCommentRepository.DeleteCommentsByUserAsync(blogDetailsViewModels.)
        // }

    }
}

