namespace CrmApp.ViewModel.WorkViewModels
{
    public class WorksApprovedList
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Create { get; set; }

        public DateTime DeadLine { get; set; }

        public string WhoIsCreate { get; set; }

        public string Status { get; set; }

        public string WorkOrderNumber { get; set; }
        public int AppUserId { get; set; }

    }
}
