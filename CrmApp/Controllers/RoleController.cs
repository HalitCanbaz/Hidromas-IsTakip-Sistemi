using CrmApp.Models.Entities;
using CrmApp.ViewModel.RoleViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace CrmApp.Controllers
{
    //[Authorize(Roles ="admin")]
    public class RoleController : Controller
    {
        private readonly UserManager<AppUser> _UserManager;
        private readonly RoleManager<AppRole> _RoleManager;
        public RoleController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _UserManager = userManager;
            _RoleManager = roleManager;
        }


        public async Task<IActionResult> RoleList()
        {
            var roleList = await _RoleManager.Roles.ToListAsync();
            var roleListViewModel = roleList.Select(x => new RoleListViewModel()
            {
                Id = x.Id,
                RoleName = x.Name

            }).ToList();
            return View(roleListViewModel);
        }

        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _RoleManager.CreateAsync(new AppRole() { Name = model.Name });

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return RedirectToAction(nameof(RoleController.RoleList));
        }


        public async Task<IActionResult> RoleUpdate(string id)
        {
            var roleToUpdate = await _RoleManager.FindByIdAsync(id);
            if (roleToUpdate == null)
            {
                throw new Exception("Güncellenecek Rol'ün Id'si bulunamamıştır.");
            }
            return View(new RoleUpdateViewModel() { Id = roleToUpdate.Id, RoleName = roleToUpdate.Name });
        }


        [HttpPost]
        public async Task<IActionResult> RoleUpdate(RoleUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var roleToUpdate = await _RoleManager.FindByIdAsync(Convert.ToString(model.Id));

            roleToUpdate.Name = model.RoleName;

            var result = await _RoleManager.UpdateAsync(roleToUpdate);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View();
            }
            TempData["message"] = "Düzeltme işlemi başarılı";

            return RedirectToAction(nameof(RoleController.RoleList));
        }

        public async Task<IActionResult> RoleDelete(string id)
        {
            var roleToDelete = await _RoleManager.FindByIdAsync(id);


            if (roleToDelete == null)
            {
                throw new Exception("Güncellenecek Rol'ün Id'si bulunamamıştır.");

            }

            var result = await _RoleManager.DeleteAsync(roleToDelete);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            TempData["message"] = "Silme işlemi başarılı";


            return RedirectToAction(nameof(RoleController.RoleList));
        }

        public async Task<IActionResult> AssignRoleToUser(string id)
        {
            //kullanıcı bilgilerini aldık
            var currentUser = await _UserManager.FindByIdAsync(id);
            ViewBag.userId = id;
            //rol bilgilerini aldık
            var roles = await _RoleManager.Roles.ToListAsync();

            //rolleri listeledik
            var roleViewModelList = new List<AssignRoleToUserViewModel>();


            var userRoles = await _UserManager.GetRolesAsync(currentUser);


            foreach (var role in roles)
            {
                var assignRoleToUserViewModel = new AssignRoleToUserViewModel()
                {
                    Id = role.Id,
                    Name = role.Name,

                };
                if (userRoles.Contains(role.Name))
                {
                    assignRoleToUserViewModel.Exist = true;
                }

                roleViewModelList.Add(assignRoleToUserViewModel);
            }

            return View(roleViewModelList);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRoleToUser(string userId, List<AssignRoleToUserViewModel> model)
        {
            var userToAssignRoles = await _UserManager.FindByIdAsync(userId);
            foreach (var role in model)
            {
                if (role.Exist)
                {
                    await _UserManager.AddToRoleAsync(userToAssignRoles, role.Name);
                }
                else
                {
                    await _UserManager.RemoveFromRoleAsync(userToAssignRoles, role.Name);
                }
            }


            return RedirectToAction(nameof(UserController.UserList), "User");
        }


    }
}
