using CrmApp.Models.Entities;

namespace CrmApp.Models
{
    public class Departman
    {
        public int Id { get; set; }
        public string DepartmanName { get; set; }

        public ICollection<AppUser> AppUsers { get; set; }=new HashSet<AppUser>();



    }
}
