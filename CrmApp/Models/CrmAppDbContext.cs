using CrmApp.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace CrmApp.Models
{
    public class CrmAppDbContext:IdentityDbContext<AppUser, AppRole, int>
    {
        public CrmAppDbContext(DbContextOptions<CrmAppDbContext> options) :
            base(options)
        {

        }
        public DbSet<Works> Works { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Departman> Departman { get; set; }
    }
}
