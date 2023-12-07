using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CrmApp.TagHelpers
{
    public class UserPictureTagHelper:TagHelper
    {
        public string PictureUrl { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "img";


            if (string.IsNullOrEmpty(PictureUrl))
            {
                output.Attributes.SetAttribute("src", "/userpicture/default.jpg");
            }
            else
            {
                output.Attributes.SetAttribute("src", $"/userpicture/{PictureUrl}");
            }
        }

    }
}
