using CrmApp.Controllers;
using CrmApp.Models;
using CrmApp.Models.Entities;
using CrmApp.Services;
using CrmApp.ViewModel.UserViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace CrmApp.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _UserManager;
        private readonly SignInManager<AppUser> _SignInManager;
        private readonly RoleManager<AppRole> _RoleManager;
        private readonly IEmailServices _EmailServices;
        private readonly IFileProvider _FileProvider;
        private readonly CrmAppDbContext _context;

        public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailServices emailServices, IFileProvider fileProvider, RoleManager<AppRole> roleManager, CrmAppDbContext context)
        {
            _UserManager = userManager;
            _SignInManager = signInManager;
            _EmailServices = emailServices;
            _FileProvider = fileProvider;
            _RoleManager = roleManager;
            _context = context;
        }

        public IActionResult SignUp()
        {
            Departman departman = new Departman();

            ViewData["DepartmanId"] = new SelectList(_context.Departman, "Id", "DepartmanName", departman.Id);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            Departman departman = new Departman();
            ViewData["DepartmanId"] = new SelectList(_context.Departman, "Id", "DepartmanName", departman.Id);

            if (!ModelState.IsValid)
            {
                return View();
            }


            var result = await _UserManager.CreateAsync(new()
            { UserName = model.UserName, Email = model.Email, PhoneNumber = model.Phone, DepartmanId = model.DepartmanId, RegisterDate = model.RegisterDate }, model.Password);



            if (result.Succeeded)
            {
                TempData["message"] = "KayıtBaşarılı";
                return RedirectToAction(nameof(UserController.SignUp));
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }

        public async Task<IActionResult> UserList()
        {


            /*var currentUse= await _UserManager.FindByNameAsync(User.Identity.Name);*/
            var userList = await _UserManager.Users.ToListAsync();

            var userListViewModel = userList.Select(x => new UserListViewModel()
            {
                Id = x.Id,
                UserName = x.UserName,
                Email = x.Email,
                Phone = x.PhoneNumber

            }).ToList();
            //.Where(x => x.UserName == currentUse.UserName)

            return View(userListViewModel);
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var controlUser = await _UserManager.FindByNameAsync(model.UserName);

            if (controlUser == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre yanlış");
                return View();
            }

            var result = await _SignInManager.PasswordSignInAsync(controlUser, model.Password, model.RememberMe, true);




            TempData["UserName"] = controlUser.UserName;
            TempData["UserPicture"] = controlUser.Picture;
            TempData["UserMail"] = controlUser.Email;


            if (result.Succeeded)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");

            }


            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Çok sayıda başarısız giriş denemeniz oldu. Kullanıcınız kilitlendi.");
                return View();
            }


            ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre yanlış");


            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _SignInManager.SignOutAsync();
            return RedirectToAction(nameof(UserController.SignIn));
        }

        public async Task<IActionResult> UserDetails()
        {
            var currentUser = await _UserManager.FindByNameAsync(User.Identity.Name);
            var userEditViewModel = new UserDetailsViewModel
            {
                UserName = currentUser.UserName,
                Email = currentUser.Email,
                Phone = currentUser.PhoneNumber,
                PictureUrl = currentUser.Picture
            };
            return View(userEditViewModel);
        }

        public async Task<IActionResult> UserEdit()
        {
            var currentUser = await _UserManager.FindByNameAsync(User.Identity.Name);

            var userEditViewModel = new UserEditViewModel()
            {
                UserName = currentUser.UserName!,
                Email = currentUser.Email!,
                Phone = currentUser.PhoneNumber!,
                Description = currentUser.Description!,

            };

            return View(userEditViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UserEdit(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var currentUser = await _UserManager.FindByNameAsync(User.Identity.Name);
            currentUser.UserName = model.UserName;
            currentUser.Email = model.Email;
            currentUser.PhoneNumber = model.Phone;
            currentUser.Description = model.Description;


            if (model.Picture != null && model.Picture.Length > 0)
            {
                var wwwrootFolder = _FileProvider.GetDirectoryContents("wwwroot");
                var randomFileName = $"{Guid.NewGuid().ToString()}{Path.GetExtension(model.Picture.FileName)}";

                var newPicturePath = Path.Combine(wwwrootFolder.First(x => x.Name == "userpicture").PhysicalPath, randomFileName);

                using var stream = new FileStream(newPicturePath, FileMode.Create);
                await model.Picture.CopyToAsync(stream);
                currentUser.Picture = randomFileName;

            }
            var updateToUserResult = await _UserManager.UpdateAsync(currentUser);

            if (!updateToUserResult.Succeeded)
            {
                foreach (var error in updateToUserResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    return View();
                }
            }

            TempData["message"] = "Kayıt Başarılı";
            return RedirectToAction(nameof(UserController.UserDetails));

        }


        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var currentUser = await _UserManager.FindByNameAsync(User.Identity.Name);
            var checkOldPassword = await _UserManager.CheckPasswordAsync(currentUser, model.PasswordOld);

            if (!checkOldPassword)
            {
                ModelState.AddModelError(string.Empty, "Yanlış eski şifre");
                return View();
            }

            var resultChangePassword = await _UserManager.ChangePasswordAsync(currentUser, model.PasswordOld, model.PasswordConfirm);

            if (!resultChangePassword.Succeeded)
            {
                foreach (IdentityError error in resultChangePassword.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View();
            }



            await _SignInManager.SignOutAsync();
            await _SignInManager.PasswordSignInAsync(currentUser, model.PasswordConfirm, true, false);


            TempData["message"] = "Şifreniz başarıyla değiştirilmiştir.";


            return RedirectToAction(nameof(UserController.ChangePassword));
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel model)
        {
            var hasUser = await _UserManager.FindByEmailAsync(model.Email);

            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Bu Email adresine ait kullanıcı bulunamamıştır.");
                return View();
            }

            string forgetPasswordToken = await _UserManager.GeneratePasswordResetTokenAsync(hasUser);

            var passwordLink = Url.Action("ResetPassword", "User", new { userId = hasUser.Id, Token = forgetPasswordToken },
                HttpContext.Request.Scheme);
            await _EmailServices.SendResetPasswordEmail(passwordLink, hasUser.Email);

            TempData["message"] = "Şifre yenileme linki eposta adresinize gönderilmiştir.";
            return RedirectToAction(nameof(UserController.ForgetPassword));
        }


        public IActionResult ResetPassword(string userId, string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var userId = TempData["userId"].ToString();
            var token = TempData["token"].ToString();
            if (userId == null || token == null)
            {
                throw new Exception("Bir hata meydana geldi. Lütfen tekrar deneyiniz.");
            }

            var hasUser = await _UserManager.FindByIdAsync(userId);

            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamamıştır.");
                return View();
            }

            IdentityResult result = await _UserManager.ResetPasswordAsync(hasUser, token, model.Password);
            if (result.Succeeded)
            {
                TempData["message"] = "Şifreniz başarıyla yenilenmiştir.";
                return RedirectToAction(nameof(UserController.ResetPassword));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();
        }
    }
}
