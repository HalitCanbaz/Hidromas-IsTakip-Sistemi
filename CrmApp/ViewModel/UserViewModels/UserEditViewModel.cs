using System.ComponentModel.DataAnnotations;

namespace CrmApp.ViewModel.UserViewModels
{
    public class UserEditViewModel
    {
        [Display(Name = "Kullanıcı Adı")]
        [Required(ErrorMessage = "Kullanıcı ad alanı boş bırakılamaz!")]
        public string UserName { get; set; }

        [Display(Name = "Ad Soyad")]
        [Required(ErrorMessage = "Ad Soyad alanı boş bırakılamaz!")]
        public string NameSurname { get; set; }
        public int DepartmanId { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email alanı boş bırakılamaz!")]
        [EmailAddress(ErrorMessage = "Email formatı yanlış")]
        public string Email { get; set; }


        [Display(Name = "Telefon")]
        [Required(ErrorMessage = "Telefon alanı boş bırakılamaz!")]
        public string Phone { get; set; }

        [Display(Name = "Profil Fotoğrafı")]
        public IFormFile? Picture { get; set; }

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

    }
}
