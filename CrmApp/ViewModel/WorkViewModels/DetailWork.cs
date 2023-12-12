using System.ComponentModel.DataAnnotations;

namespace CrmApp.ViewModel.WorkViewModels
{
    public class DetailWork
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "İşi bitirebilmek için açıklama girmek zorundasınız!")]
        public string FinishedDescription { get; set; }

        public string WhoIsCreate { get; set; }

        public string Status { get; set; }

        public byte Progress { get; set; }

        public DateTime Create { get; set; }

        public DateTime Update { get; set; }

        public DateTime DeadLine { get; set; }

        public DateTime Finished { get; set; }

        public string AppUser { get; set; }
        public int AppUserId { get; set; }

        public string Categories { get; set; }

        public string WorkOrderNumber { get; set; }

        public string ApprovedNote { get; set; }
    }
}
