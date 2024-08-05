using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using BlogApp.Data;

namespace BlogApp.Models.ViewModels
{
    public class AddTagRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string DisplayName { get; set; }
    }
}