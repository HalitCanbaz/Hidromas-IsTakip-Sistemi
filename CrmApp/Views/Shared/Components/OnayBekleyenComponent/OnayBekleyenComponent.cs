using CrmApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrmApp.Views.Shared.Components.WorkComponent
{
    public class OnayBekleyenComponent:ViewComponent
    {
        private readonly CrmAppDbContext _context;

        public OnayBekleyenComponent(CrmAppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result =await _context.Works.CountAsync(x => x.Status == "beklemede");

            OnayBekleyenViewModel model = new OnayBekleyenViewModel();

            model.Total = result;

            return View(model);
        }



    }
}
