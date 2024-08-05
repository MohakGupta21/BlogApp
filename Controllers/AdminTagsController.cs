using BlogApp.Data;
using BlogApp.Models.Domain;
using BlogApp.Models.ViewModels;
using BlogApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    [Authorize(Roles ="Admin")]

    public class AdminTagsController : Controller
    {

        private readonly ITagRepository _tagRepository;
        public AdminTagsController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            ValidateTag(addTagRequest);
            if(!ModelState.IsValid)
                return View();
            //Mapping AddTagRequest to Tag
            var tag = new Tag { Name = addTagRequest.Name, DisplayName = addTagRequest.DisplayName };

            await _tagRepository.AddAsync(tag);

            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> List(string? searchQuery, string? sortBy, string? sortDirection, int pageSize=3, int pageNo=1)
        {
            var totalRecords = await _tagRepository.CountAsync();
            var totalPages = Math.Ceiling((decimal)totalRecords / pageSize);

            if(pageNo > totalPages)
            {
                pageNo--;
            }

            if(pageNo < 1)
            {
                pageNo++;
            }

            ViewBag.TotalPages = totalPages;
            ViewBag.SearchQuery = searchQuery;
            ViewBag.SortBy = sortBy;
            ViewBag.SortDirection = sortDirection;
            ViewBag.PageSize = pageSize;
            ViewBag.PageNo = pageNo;



            var TagsList = await _tagRepository.GetAllAsync(searchQuery,sortBy,sortDirection,pageNo,pageSize);
            
            return View(TagsList);
        }
        
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await _tagRepository.GetAsync(id);

            if(tag!=null)
            {
                var editTagRequest = new EditTagRequest{
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName 
                };
                return View(editTagRequest);
            }

            return View(null);
            
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag{
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };

            var updatedTag = await _tagRepository.UpdateAsync(tag);

            if(updatedTag!=null)
            {
                // Show Success notification
                return RedirectToAction("List");
            }
            
            // Show error notification
            return RedirectToAction("Edit", new {id = editTagRequest.Id});
            
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            var deletedTag = await _tagRepository.DeleteAsync(editTagRequest.Id);

            if(deletedTag !=null)
            {
                // Show success notification
                return RedirectToAction("List");   
            }
            // Show error notification
            return RedirectToAction("Edit", new {id = editTagRequest.Id});
        }

        private void ValidateTag(AddTagRequest request)
        {
            if(request.Name is not null && request.DisplayName is not null)
            {
                if(request.Name == request.DisplayName)
                {
                    ModelState.AddModelError("DisplayName","Name cannot be the same as DisplayName");
                }
            }
        }

    }

}