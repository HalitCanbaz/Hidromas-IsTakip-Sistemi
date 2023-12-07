using System.ComponentModel.DataAnnotations;

namespace CrmApp.ViewModel.WorkViewModels
{
    public class CategoryCreateViewModel
    {
        [Required(ErrorMessage ="Boş Bırakılamaz")]
        public string CategoryName { get; set; }
    }
}
