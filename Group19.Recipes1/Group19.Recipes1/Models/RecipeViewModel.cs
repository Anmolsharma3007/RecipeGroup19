using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Group19.Recipes1.Models
{
    public class RecipeViewModel
    {
        public RecipeViewModel()
        {
            IngredientViewModels = new List<IngredientViewModel>();
            CategoryViewModels = new List<CategoryViewModel>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(128)]
        public string Name { get; set; }

        public string Author { get; set; }

        public int PreparationTime { get; set; }

        public int CookingTime { get; set; }

        [Display(Name = "Category")]
        //public CategoryViewModels CategoryViewModels { get; set; }
        public IList<CategoryViewModel> CategoryViewModels { get; set; }

        [Required]
        [Display(Name = "Ingredients")]
        public IList<IngredientViewModel> IngredientViewModels { get; set; }
        //public IngredientViewModel IngredientViewModels { get; set; }        
    }
}