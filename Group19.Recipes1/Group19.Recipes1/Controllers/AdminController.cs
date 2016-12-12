using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Group19.AspNetIdentity;
using Group19.Recipes1.DataAccess.Repository;
using Group19.Recipes1.Models;
using Microsoft.AspNet.Identity;

namespace Group19.Recipes1.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class AdminController : BaseController
    {
        private readonly UsersRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(
            IUnitOfWork unitOfWork,
            IRepository<IdentityUser> userRepository,
            UserManager<IdentityUser> userManager)
            : base(unitOfWork)
        {
            _userRepository = userRepository as UsersRepository;
            _userManager = userManager;
        }

        // GET: Admin
        public async Task<ActionResult> Index()
        {
            var users = await _userRepository.GetAllAsync();
            return View(Mapper.Map<IEnumerable<IdentityUser>, IEnumerable<UserViewModel>>(users));
        }

        public async Task<ActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction("Index");

            var user = await _userRepository.GetByIdAsync(id);
            var userViewModel = Mapper.Map<IdentityUser, UserViewModel>(user);
            userViewModel.UserRecipes = _userRepository.GetUserRecipeCount(id);
            return View(userViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Details(UserViewModel model)
        {
            if (!ModelState.IsValid || model.Id.Equals(User.Identity.GetUserId()))
                return View(model);

            await UpdateUserRole(model.Id, model.IsAdmin, Roles.Admin);
            await UpdateUserRole(model.Id, model.IsEditor, Roles.Editor);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string userId)
        {
            if (!ModelState.IsValid || userId == null || userId.Equals(User.Identity.GetUserId()))
                return RedirectToAction("Details", "Admin", new { id = userId });
            var result = await _userManager.DeleteAsync(await _userManager.FindByIdAsync(userId));
            if (result.Succeeded)
                return RedirectToAction("Index");

            return RedirectToAction("Details", "Admin", new { id = userId });
        }

        private async Task UpdateUserRole(string id, bool addRole, string role)
        {
            if (addRole)
                await _userManager.AddToRoleAsync(id, role);
            else
                await _userManager.RemoveFromRoleAsync(id, role);
        }
    }
}