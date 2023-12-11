using CrmApp.Models.Entities;
using CrmApp.Models;
using Microsoft.AspNetCore.Identity;
using CrmApp.Models.Entities;
using System;
using CrmApp.Localization;
using CrmApp.CustomValidations;

namespace CrmApp.Extensions
{
    public static class StartUpExtensions
    {
        public static void AddIdentityWithExtensions(this IServiceCollection services)
        {
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(1);
            });
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = false;
                options.User.AllowedUserNameCharacters = "zxcvbnmilkjhgfdsaqwertyuop";
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.MaxFailedAccessAttempts = 3;

            })
            .AddDefaultTokenProviders()
            .AddUserValidator<UserValidator>()
            .AddPasswordValidator<PasswordValidator>()
            .AddEntityFrameworkStores<CrmAppDbContext>()
            .AddErrorDescriber<LocalizationIdentityErrorDescriber>();

        }

    }
}
