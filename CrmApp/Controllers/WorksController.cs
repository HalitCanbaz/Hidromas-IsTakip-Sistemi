using CrmApp.Models;
using CrmApp.Models.Entities;
using CrmApp.ViewModel.RoleViewModels;
using CrmApp.ViewModel.UserViewModels;
using CrmApp.ViewModel.WorkViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CrmApp.Controllers
{
    public class WorksController : Controller
    {
        private readonly SignInManager<AppUser> _SignInManager;
        private readonly UserManager<AppUser> _UserManager;
        private readonly CrmAppDbContext _context;

        public WorksController(CrmAppDbContext context, SignInManager<AppUser> signInManager = null, UserManager<AppUser> userManager = null)
        {
            _context = context;
            _SignInManager = signInManager;
            _UserManager = userManager;
        }


        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> Create()
        {

            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "NameSurName");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName");
            ViewData["DepartmanId"] = new SelectList(_context.Departman, "Id", "DepartmanName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(WorksCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["message"] = "Bir hata oluştu";
                return View();
            }

            var user = await _UserManager.FindByNameAsync(User.Identity.Name);
            //var userList = await _context.Users.Where(x => x.Id == user.Id).ToListAsync();

            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "NameSurName");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName");
            ViewData["DepartmanId"] = new SelectList(_context.Departman, "Id", "DepartmanName");

            var result = new Works()
            {
                Title = model.Title,
                Description = model.Description,
                Status = model.Status,
                Progress = model.Progress,
                WhoIsCreate = user.NameSurName,
                Create = model.Create,
                Update = model.Update,
                DeadLine = model.DeadLine,
                Finished = model.Finished,
                AppUserId = model.AppUserId,
                CategoriesId = model.CategoriesId,
                Departman = model.Departman,
                WorkOpenDepartman = user.DepartmanId
            };

            await _context.AddAsync(result);
            await _context.SaveChangesAsync();
            TempData["message"] = "iş ataması başarılı. Yeni kayıt açabilirsiniz!";

            return RedirectToAction(nameof(WorksController.Create));

        }

        public async Task<IActionResult> WorksAppRovedList()
        {
            var worksList = await _context.Works.ToListAsync();

            var userControl = await _UserManager.FindByNameAsync(User.Identity.Name);

            var worksListViewModel = worksList.Where(x => x.Departman == (userControl.DepartmanId) & x.Status == "beklemede").Select(x => new WorksApprovedList()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Create = x.Create,
                DeadLine = x.DeadLine,
                WhoIsCreate = x.WhoIsCreate,
                Status = x.Status

            }).ToList();
            return View(worksListViewModel);
        }



        [HttpPost]
        public async Task<IActionResult> WorksStatusApproval(int id)
        {
            var work = await _context.Works.FindAsync(id);

            if (work != null)
            {
                work.Status = "onaylandı";

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(WorksController.WorksAppRovedList));
        }


        public async Task<IActionResult> DetailWork(int Id)
        {
            var worksDetails = await _context.Works.Where(x => x.Id == Id).FirstOrDefaultAsync();
            var user = await _context.Users.Where(x => x.Id == worksDetails.AppUserId).FirstOrDefaultAsync();
            var category = await _context.Categories.Where(x => x.Id == worksDetails.CategoriesId).FirstOrDefaultAsync();


            var details = new DetailWork()
            {
                Id = worksDetails.Id,
                Title = worksDetails.Title,
                Description = worksDetails.Description,
                WhoIsCreate = worksDetails.WhoIsCreate,
                Status = worksDetails.Status,
                Progress = worksDetails.Progress,
                Create = worksDetails.Create,
                Update = worksDetails.Update,
                DeadLine = worksDetails.DeadLine,
                Finished = worksDetails.Finished,
                AppUser = user.UserName,
                Categories = category.CategoryName
            };
            return View(details);
        }

        public async Task<IActionResult> MyOpenedenWorks()
        {
            var worksList = await _context.Works.ToListAsync();

            var userControl = await _UserManager.FindByNameAsync(User.Identity.Name);

            var worksListViewModel = worksList.Where(x => x.WorkOpenDepartman == (userControl.DepartmanId) & x.Status == "beklemede").Select(x => new WorksApprovedList()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Create = x.Create,
                DeadLine = x.DeadLine,
                WhoIsCreate = x.WhoIsCreate,
                Status = x.Status

            }).OrderDescending().ToList();
            return View(worksListViewModel);
        }

        public async Task<IActionResult> MyWorks()
        {
            var worksList = await _context.Works.ToListAsync();

            var userControl = await _UserManager.FindByNameAsync(User.Identity.Name);

            var worksListViewModel = worksList.Where(x => x.AppUserId == userControl.Id & (x.Status == "onaylandı" || x.Status=="başlandı")).Select(x => new WorksApprovedList()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                Create = x.Create,
                DeadLine = x.DeadLine,
                WhoIsCreate = x.WhoIsCreate,
                Status = x.Status

            }).ToList();
            return View(worksListViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> WorksStatusStarted(int id)
        {
            var work = await _context.Works.FindAsync(id);

            if (work != null)
            {
                work.Status = "başlandı";

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(WorksController.MyWorks));
        }

        [HttpPost]
        public async Task<IActionResult> WorkStatusFinished(int id)
        {
            var work = await _context.Works.FindAsync(id);

            if (work != null)
            {
                work.Status = "bitti";

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(WorksController.MyWorks));
        }

    }
}
