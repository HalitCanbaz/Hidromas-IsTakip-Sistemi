using Microsoft.AspNetCore.Identity;

namespace CrmApp.Localization
{
    public class LocalizationIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateUserName(string userName)
        {
            return new() { Code = "DuplicateUserName", Description = $"{userName} daha önce başka bir kullanıcı tarafından alınmıştır." };
        }

        public override IdentityError InvalidUserName(string userName)
        {
            return new() { Code = "InvalidUserName", Description = $"Kullanıcı adı '{userName}' için büyük harf veya rakam kullanılamaz."};
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new() { Code = "PasswordTooShort", Description = $"Şifre 8 karakter ve üzerinde olmalıdır." };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new() { Code = "PasswordUpperCase", Description = $"Şifrenizde büyük harf olmalı" };
        }
        public override IdentityError PasswordRequiresLower()
        {
            return new() { Code = "PasswordLowerCase", Description = $"Şifrenizde Küçük harf olmalı" };

        }


    }
}
