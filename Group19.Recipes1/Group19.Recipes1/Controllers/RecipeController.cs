using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Group19.AspNetIdentity;
using Group19.Recipes1.DataAccess.Models;
using Group19.Recipes1.DataAccess.Repository;
using Group19.Recipes1.Models;
using Microsoft.AspNet.Identity;

namespace Group19.Recipes1.Controllers
{
    [Authorize]
    public class RecipeController : BaseController
    {
        private readonly IRepository<Recipe> _recipeRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly IRepository<Measurement> _measurementRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public RecipeController(
            IUnitOfWork unitOfWork,
            IRepository<Recipe> recipeRepository,
            IRepository<Category> categoryRepository,
            IRepository<Measurement> measurementRepository,
            UserManager<IdentityUser> userManager)
            : base(unitOfWork)
        {
            _recipeRepository = recipeRepository;
            _measurementRepository = measurementRepository;
            _userManager = userManager;
            _categoryRepository = categoryRepository as CategoryRepository;
        }

        // GET: Recipe
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var recipes = await _recipeRepository.GetAllAsync();
            return View(Mapper.Map<IEnumerable<Recipe>, IEnumerable<RecipeViewModel>>(recipes));
        }

        public async Task<ActionResult> AddNew()
        {
            var measurements = await _measurementRepository.GetAllAsync();
            ViewBag.Measurements = new SelectList(measurements.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddNew(RecipeViewModel recipeViewModel)
        {
            var measurements = await _measurementRepository.GetAllAsync();
            ViewBag.Measurements = new SelectList(measurements.ToList(), "Id", "Name");
            if (!ModelState.IsValid)
            {
                return View();
            }

            var measurement = measurements.FirstOrDefault(c => c.Id == recipeViewModel.IngredientViewModels[0].MeasurementViewModel.Id);

            var recipe = Mapper.Map<RecipeViewModel, Recipe>(recipeViewModel);
            recipe.Author = await _userManager.FindByIdAsync(User.Identity.GetUserId());

            // TODO: For now, only one category and one Ingredient can be added
            foreach (var ingredientViewModel in recipeViewModel.IngredientViewModels)
            {
                var ingredient = Mapper.Map<IngredientViewModel, Ingredient>(ingredientViewModel);
                ingredient.Measurement = measurement;
                recipe.AddIngredient(ingredient);
            }



            foreach (var categoryViewModel in recipeViewModel.CategoryViewModels)
            {
                var category = Mapper.Map<CategoryViewModel, Category>(categoryViewModel);
                category = _categoryRepository.GetOrInsertCategory(category);
                recipe.Categories.Add(category);
            }

            await _recipeRepository.AddAsync(recipe);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Details(int id)
        {
            var recipe = await _recipeRepository.GetByIdAsync(id);
            return View(Mapper.Map<Recipe, RecipeViewModel>(recipe));
        }
    }
}