using CrmApp.Models;
using CrmApp.Views.Shared.Components.WorkComponent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrmApp.Views.Shared.Components.AylikOranComponent
{
    public class AylikOranComponent : ViewComponent
    {
        private readonly CrmAppDbContext _context;

        public AylikOranComponent(CrmAppDbContext context)
        {
            _context = context;


        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            DateTime now = DateTime.Now;
            DateTime startOfMonth = new DateTime(now.Year, now.Month, 1);
            DateTime endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);



            var resultFinished = await _context.Works.CountAsync(x => x.Status == "bitti" && x.Create >= startOfMonth && x.Create <= endOfMonth);

            var resultWaiting = await _context.Works.CountAsync(x => x.Status == "onaylandı" && x.Create >= startOfMonth && x.Create <= endOfMonth);


            AylikOranViewModel model = new AylikOranViewModel();

            model.TotalFinished = resultFinished;
            model.TotalWaiting = resultWaiting;
            return View(model);
        }
    }
}
