using BlogApp.Models.ViewModels;
using BlogApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdminUsersController:Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IBlogPostLikeRepository _blogPostLikeRepository;
        private readonly IBlogPostCommentRepository _blogPostCommentRepository;

        public AdminUsersController(IUserRepository userRepository, UserManager<IdentityUser> userManager, IBlogPostLikeRepository blogPostLikeRepository, IBlogPostCommentRepository blogPostCommentRepository)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _blogPostLikeRepository = blogPostLikeRepository;
            _blogPostCommentRepository = blogPostCommentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var users = await _userRepository.GetAll();

            var usersViewModel = new UserViewModel
            {
                Users = new List<User>()
            };
            foreach (var user in users)
            {
                usersViewModel.Users.Add(
                    new User{
                        Id = Guid.Parse(user.Id),
                        Username = user.UserName,
                        EmailAddress = user.Email
                    }
                );   
            }
            return View(usersViewModel);
        }
    
        [HttpPost]
        public async Task<IActionResult> List(UserViewModel userViewModel)
        {
            var identityUser = new IdentityUser
            {
                UserName = userViewModel.Username,
                Email = userViewModel.Email
            };

            var identityResult = await _userManager.CreateAsync(identityUser,userViewModel.Password);

            if(identityResult!=null && identityResult.Succeeded)
            {
                var roles = new List<string>{"User"};

                if(userViewModel.AdminRoleChecked)
                {
                    roles.Add("Admin");
                }
                identityResult = await _userManager.AddToRolesAsync(identityUser,roles);

                if(identityResult!=null && identityResult.Succeeded)
                {
                    return RedirectToAction("List");
                }
            }
            return RedirectToAction("Error","Home");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid itemid)
        {
            var user = await _userManager.FindByIdAsync(itemid.ToString()); 
            await _blogPostLikeRepository.DeleteLikesOfUser(itemid);
            await _blogPostCommentRepository.DeleteCommentsByUserAsync(itemid);

            if(user is not null)
            {
                var identityResult = await _userManager.DeleteAsync(user);

                
                if(identityResult is not null && identityResult.Succeeded)
                    return RedirectToAction("List");
            }
            return RedirectToAction("Error","Home");
        }
    }
}