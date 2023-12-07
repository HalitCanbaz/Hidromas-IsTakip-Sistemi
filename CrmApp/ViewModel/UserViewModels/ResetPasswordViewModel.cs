using System.ComponentModel.DataAnnotations;

namespace CrmApp.ViewModel.UserViewModels
{
    public class ResetPasswordViewModel
    {
        [Display(Name = "Yeni Şifre")]
        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz!")]
        public string Password { get; set; }


        [Display(Name = "Yeni Şifre Tekrar")]
        [Required(ErrorMessage = "Şifre tekrar alanı boş bırakılamaz!")]
        [Compare(nameof(Password), ErrorMessage = "Şifreler aynı değil!")]
        public string PasswordConfirm { get; set; }
    }
}
