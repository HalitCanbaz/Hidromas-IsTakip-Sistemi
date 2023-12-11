using System.ComponentModel.DataAnnotations;

namespace CrmApp.ViewModel.UserViewModels
{
    public class SignUpViewModel
    {
        [Display(Name = "Kullanıcı Adı")]
        [Required(ErrorMessage = "Kullanıcı ad alanı boş bırakılamaz!")]
        public string UserName { get; set; }

        [Display(Name = "Ad & Soyad")]
        [Required(ErrorMessage = "Ad Soyad alanı boş bırakılamaz!")]
        public string NameSurname { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email alanı boş bırakılamaz!")]
        [EmailAddress(ErrorMessage = "Email formatı yanlış")]
        public string Email { get; set; }


        [Display(Name = "Telefon")]
        [Required(ErrorMessage = "Telefon alanı boş bırakılamaz!")]
        public string Phone { get; set; }

        [Display(Name = "Departman Adı")]
        [Required(ErrorMessage = "Departman alanı boş bırakılamaz!")]
        public int DepartmanId { get; set; }


        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz!")]
        public string Password { get; set; }


        [Display(Name = "Şifre Tekrar")]
        [Required(ErrorMessage = "Şifre tekrar alanı boş bırakılamaz!")]
        [Compare(nameof(Password), ErrorMessage = "Şifreler aynı değil!")]
        public string PasswordConfirm { get; set; }

        public DateTime RegisterDate { get; set; } = DateTime.Now;

        public string Status { get; set; } = "beklemede";
    }
}
