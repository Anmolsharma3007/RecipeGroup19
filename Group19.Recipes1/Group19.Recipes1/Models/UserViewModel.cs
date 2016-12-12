using System.ComponentModel.DataAnnotations;

namespace Group19.Recipes1.Models
{
    public class UserViewModel
    {
        [Required]
        public string Id { get; set; }

        [Display(Name = "User name")]
        [Required]
        public string UserName { get; set; }

        [Display(Name = "Is Admin")]
        public bool IsAdmin { get; set; }

        [Display(Name = "Is Editor")]
        public bool IsEditor { get; set; }

        [Display(Name = "Created recipes")]
        public int UserRecipes { get; set; }
    }
}