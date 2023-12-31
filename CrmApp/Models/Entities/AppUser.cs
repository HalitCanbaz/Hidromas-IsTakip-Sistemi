﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace CrmApp.Models.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string NameSurName { get; set; }

        public string? Picture { get; set; }

        public DateTime? RegisterDate { get; set; }

        public bool IsActive { get; set; }

        public  string Status { get; set; }


        public string? Description { get; set; }        

        public ICollection<Works> Works { get; set; }= new HashSet<Works>();

        [ForeignKey("DepartmanId")]
        public int DepartmanId { get; set; }
        public Departman Departman { get; set; }

    }
}
