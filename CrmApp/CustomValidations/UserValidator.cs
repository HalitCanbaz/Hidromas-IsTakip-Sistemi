using CrmApp.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace CrmApp.CustomValidations
{
    public class UserValidator : IUserValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            var errors= new List<IdentityError>();
            var isDijit = int.TryParse(user.UserName[0].ToString(), out _);

            if (isDijit)
            {
                errors.Add(new() { Code = "UserNameContainFirstLetterDijit", Description = "Kullanıcı adında sayısal karakter olamaz"});
            }

            if (errors.Any())
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }
}
