using System.ComponentModel.DataAnnotations;

namespace CrmApp.ViewModel.RoleViewModels
{
    public class RoleUpdateViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Rol İsmi")]
        [Required(ErrorMessage = "Role isim alanı boş bırakılamaz!")]
        public string RoleName { get; set; }

    }
}
