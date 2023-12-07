using System.ComponentModel.DataAnnotations;

namespace CrmApp.ViewModel.WorkViewModels
{
    public class DepartmentCreateViewModel
    {
        [Required(ErrorMessage ="Boş bırakılamaz")]
        public string DepartmentName { get; set; }
    }
}
