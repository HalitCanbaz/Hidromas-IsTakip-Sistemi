using CrmApp.Models;
using CrmApp.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace CrmApp.Views.Shared.Components.WorkComponent
{
    public class OnayBekleyenComponent : ViewComponent
    {
        private readonly CrmAppDbContext _context;
        private readonly UserManager<AppUser> _UserManager;
        public OnayBekleyenComponent(CrmAppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _UserManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userControl = await _UserManager.FindByNameAsync(User.Identity.Name);


            var result = await _context.Works.Where(x=> x.Departman==userControl.DepartmanId && x.Status=="beklemede").CountAsync();


            OnayBekleyenViewModel model = new OnayBekleyenViewModel();

            model.Total = result;

            return View(model);
        }



    }
}
