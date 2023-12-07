using System.ComponentModel.DataAnnotations;

namespace CrmApp.ViewModel.UserViewModels
{
    public class ForgetPasswordViewModel
    {

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email alanı boş bırakılamaz!")]
        [EmailAddress(ErrorMessage = "Email formatı yanlış")]
        public string Email { get; set; }

    }
}
