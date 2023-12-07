using System.ComponentModel.DataAnnotations;

namespace CrmApp.ViewModel.UserViewModels
{
    public class SignInViewModel
    {

        [Required(ErrorMessage = "Kullanıcı ad alanı boş bırakılamaz!")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz!")]
        [Display(Name = "Şifre")]
        public string Password { get; set; }


        [Display(Name = "Beni hatırla")]
        public bool RememberMe { get; set; }
    }
}
