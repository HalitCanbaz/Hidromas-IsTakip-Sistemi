using CrmApp.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrmApp.Models
{
    public class Works
    {

        public int Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public string Status { get; set; }

        public byte Progress { get; set; }

        public string WhoIsCreate { get; set; }

        public DateTime Create { get; set; }

        public DateTime Update { get; set; }

        public DateTime DeadLine { get; set; }

        public DateTime Finished { get; set; }

        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public int CategoriesId { get; set; }
        public Categories Categories { get; set; }       

    }
}
