using System.ComponentModel.DataAnnotations;

namespace CrmApp.ViewModel.UserViewModels
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "Eski Şifre")]
        [Required(ErrorMessage = "Eski şifre alanı boş bırakılamaz!")]
        public string PasswordOld { get; set; }



        [Display(Name = "Yeni Şifre")]
        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz!")]
        public string PasswordNew { get; set; }



        [Display(Name = "Yeni Şifre Tekrar")]
        [Required(ErrorMessage = "Şifre tekrar alanı boş bırakılamaz!")]
        [Compare(nameof(PasswordNew), ErrorMessage = "Şifreler aynı değil!")]
        public string PasswordConfirm { get; set; }

    }
}
