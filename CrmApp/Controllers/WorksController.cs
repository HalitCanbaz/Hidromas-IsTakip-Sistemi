using CrmApp.Models;
using CrmApp.Models.Entities;
using CrmApp.ViewModel.RoleViewModels;
using CrmApp.ViewModel.UserViewModels;
using CrmApp.ViewModel.WorkViewModels;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

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

            //ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "NameSurName");
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

            //ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "NameSurName");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName");
            ViewData["DepartmanId"] = new SelectList(_context.Departman, "Id", "DepartmanName");

            DateTime systemClock = DateTime.Now;
            DateTime controlClock = systemClock.AddMinutes(30);

            if (controlClock >= model.DeadLine)
            {
                TempData["timeMessage"] = "Talep ettiğiniz tarih sistem saatinden minimum 30 dk yukarıda olmak zorundadır!";
                return View();
            }



            string numberUp = "";

            DateTime thisYear = DateTime.Now;
            string years = Convert.ToString(thisYear.Year % 100);


            var currentWorks = await _context.Works.OrderByDescending(x => x.WorkOrderNumber).FirstOrDefaultAsync();


            if (currentWorks != null)
            {
                string number = currentWorks.WorkOrderNumber.Substring(currentWorks.WorkOrderNumber.Length - 3);
                int numberIntl = Convert.ToInt32(number) + 1;
                string numberString = Convert.ToString(numberIntl);

                if (numberString.Length <= 1)
                {
                    numberUp = years + "-" + "000" + (Convert.ToString(numberString));

                }
                else if (numberString.Length <= 2)
                {
                    numberUp = years + "-" + "00" + (Convert.ToString(numberString));

                }
                else if (numberString.Length <= 3)
                {
                    numberUp = years + "-" + "0" + (Convert.ToString(numberString));

                }

            }
            else
            {
                numberUp = years + "-" + "0001";
            }

            var user = await _UserManager.FindByNameAsync(User.Identity.Name);

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
                AppUserId = 1,
                CategoriesId = model.CategoriesId,
                Departman = model.Departman,
                WorkOpenDepartman = user.DepartmanId,
                WorkOrderNumber = numberUp

            };

            await _context.AddAsync(result);
            await _context.SaveChangesAsync();
            TempData["message"] = "İş ataması başarılı. Yeni kayıt açabilirsiniz!";

            return RedirectToAction(nameof(WorksController.Create));

        }

        public async Task<IActionResult> WorksAppRovedList(int Id)
        {
            var worksList = await _context.Works.ToListAsync();

            var userControl = await _UserManager.FindByNameAsync(User.Identity.Name);




            var worksListViewModel = worksList.Where(x => x.Departman == (userControl.DepartmanId) & x.Status == "beklemede").Select(x => new WorksApprovedList()
            {
                Id = x.Id,
                Title = x.Title,
                Create = x.Create,
                DeadLine = x.DeadLine,
                WhoIsCreate = x.WhoIsCreate,
                Status = x.Status,
                WorkOrderNumber = x.WorkOrderNumber,

            }).ToList();
            return View(worksListViewModel);
        }



        public async Task<IActionResult> WorkStatusApprovalDetail(int Id)
        {
            var userControl = await _UserManager.FindByNameAsync(User.Identity.Name);

            var bilgiIslemKullanicilari = _context.Users
               .Where(u => u.DepartmanId == userControl.DepartmanId && u.Status == "onaylandı")
               .Select(u => new { Id = u.Id, NameSurName = u.NameSurName })
               .ToList();

            // SelectList'e eklerken kullanabilirsiniz
            ViewData["AppUserId"] = new SelectList(bilgiIslemKullanicilari, "Id", "NameSurName");


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
                AppUserId = user.Id,
                AppUser = user.UserName,
                Categories = category.CategoryName,
                WorkOrderNumber = worksDetails.WorkOrderNumber,
                ApprovedNote = worksDetails.ApprovedNote

            };
            return View(details);
        }

        [HttpPost]
        public async Task<IActionResult> WorkStatusApprovalDetail(int Id, DetailWork model)
        {
            var userControl = await _UserManager.FindByNameAsync(User.Identity.Name);

            var bilgiIslemKullanicilari = _context.Users
               .Where(u => u.DepartmanId == userControl.DepartmanId && u.Status == "onaylandı")
               .Select(u => new { Id = u.Id, NameSurName = u.NameSurName })
               .ToList();

            ViewData["AppUserId"] = new SelectList(bilgiIslemKullanicilari, "Id", "NameSurName");


            var worksDetails = await _context.Works.Where(x => x.Id == Id).FirstOrDefaultAsync();
            var user = await _context.Users.Where(x => x.Id == worksDetails.AppUserId).FirstOrDefaultAsync();
            var category = await _context.Categories.Where(x => x.Id == worksDetails.CategoriesId).FirstOrDefaultAsync();


            if (worksDetails != null)
            {
                worksDetails.Status = "onaylandı";
                worksDetails.AppUserId = model.AppUserId;
                worksDetails.ApprovedNote = model.ApprovedNote;
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
                Categories = category.CategoryName,
                WorkOrderNumber = worksDetails.WorkOrderNumber,
                ApprovedNote = worksDetails.ApprovedNote


            };
            return View(details);
        }

        public async Task<IActionResult> MyOpenedenWorks()
        {
            var worksList = await _context.Works.ToListAsync();

            var userControl = await _UserManager.FindByNameAsync(User.Identity.Name);

            var worksListViewModel = worksList.Where(x => x.WorkOpenDepartman == userControl.DepartmanId).Select(x => new WorksApprovedList()
            {
                Id = x.Id,
                Title = x.Title,
                Create = x.Create,
                DeadLine = x.DeadLine,
                WhoIsCreate = x.WhoIsCreate,
                Status = x.Status,
                WorkOrderNumber = x.WorkOrderNumber

            }).OrderBy(x => x.Create).ToList();
            return View(worksListViewModel);
        }

        public async Task<IActionResult> MyWorks()
        {
            var worksList = await _context.Works.ToListAsync();

            var userControl = await _UserManager.FindByNameAsync(User.Identity.Name);

            var worksListViewModel = worksList.Where(x => x.AppUserId == userControl.Id & (x.Status == "onaylandı" || x.Status == "başlandı")).Select(x => new WorksApprovedList()
            {
                Id = x.Id,
                Title = x.Title,
                Create = x.Create,
                DeadLine = x.DeadLine,
                WhoIsCreate = x.WhoIsCreate,
                Status = x.Status,
                WorkOrderNumber = x.WorkOrderNumber


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


        public async Task<IActionResult> WorkStatusFinished()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> WorkStatusFinished(int id, WorkStatusFinishedViewModel model)
        {
            var work = await _context.Works.FindAsync(id);

            //if (model.FinishedDescription == null)
            //{
            //    ViewData["aciklama"] = "İşin açıklaması girilmeden kayıt bitirilemez!";
            //    return RedirectToAction(nameof(WorksController.DetailWork), id);
            //}

            if (work != null)
            {
                work.FinishedDescription = model.FinishedDescription;
                work.Status = "bitti";

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(WorksController.MyWorks));
        }

    }
}
