using CrmApp.Models;
using CrmApp.ViewModel.WorkViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrmApp.Controllers
{
    //[Authorize]
    public class TaskCategoryController : Controller
    {
        private readonly CrmAppDbContext _context;

        public TaskCategoryController(CrmAppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> List()
        {
            var categoriList = await _context.Categories.ToListAsync();

            return View(categoriList);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            Categories categories = new Categories();

            categories.CategoryName = model.CategoryName;
            await _context.AddAsync(categories);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(TaskCategoryController.List));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            var categoryEditViewModel = new CategoryEditViewModel()
            {
                Id = category.Id,
                CategoryName = category.CategoryName

            };

            return View(categoryEditViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var categories = await _context.Categories.FindAsync(model.Id);
            categories.CategoryName=model.CategoryName;
            await _context.SaveChangesAsync();

            return View();

        }


        public IActionResult Delete(int id)
        {
            var category =  _context.Categories.Find(id);
            var categoryEditViewModel = new CategoryEditViewModel()
            {
                Id = category.Id,
                CategoryName = category.CategoryName

            };

            return View(categoryEditViewModel);
        }


        [HttpPost]
        public  IActionResult Delete(CategoryEditViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }

            var categories =  _context.Categories.Find(model.Id);
            categories.CategoryName = model.CategoryName;
            _context.Remove(categories);
            _context.SaveChanges();

            return View();


        }
        
    }
}
