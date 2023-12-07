using System.ComponentModel.DataAnnotations;

namespace CrmApp.ViewModel.RoleViewModels
{
    public class CreateRoleViewModel
    {
        [Display(Name = "Rol İsmi")]
        [Required(ErrorMessage = "Role isim alanı boş bırakılamaz!")]
        public string Name { get; set; } 
    }
}
