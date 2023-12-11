using System.ComponentModel.DataAnnotations;

namespace CrmApp.ViewModel.WorkViewModels
{
    public class WorkStatusFinishedViewModel
    {
        public int Id { get; set; }



        [Required(ErrorMessage ="Açıklama girilmeden iş bitirilemez!")]
        public string FinishedDescription { get; set; }
    }
}
