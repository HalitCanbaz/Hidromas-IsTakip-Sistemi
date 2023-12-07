using CrmApp.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace CrmApp.TagHelpers
{
    public class UserRoleNamesTagHelper : TagHelper
    {
        public string UserId { get; set; }
        private readonly UserManager<AppUser> _UserManager;

        public UserRoleNamesTagHelper(UserManager<AppUser> userManager)
        {
            _UserManager = userManager;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var user = await _UserManager.FindByIdAsync(UserId);
            var userRole = await _UserManager.GetRolesAsync(user);

            var stringBuilder = new StringBuilder();

            userRole.ToList().ForEach(x =>
            {
                stringBuilder.Append(@$"<span class='badge bg-secondary'>{x.ToLower()}</span>");
            });
            
            output.Content.SetHtmlContent(stringBuilder.ToString());


        }

    }
}
