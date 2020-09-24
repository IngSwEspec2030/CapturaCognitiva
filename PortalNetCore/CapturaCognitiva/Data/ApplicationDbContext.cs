using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CapturaCognitiva.Models.AccountViewModels;
using CapturaCognitiva.Data.Migrations;
using CapturaCognitiva.Data.Entities;
using CapturaCognitiva.Models.ViewModels;

namespace CapturaCognitiva.Data
{
    public class ApplicationUser : IdentityUser
    {
        public long Cedula { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsRemoved { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateEnabled { get; set; }
        public DateTime? DateRemoved { get; set; }
        public DateTime? DateEmailConfirmed { get; set; }
        public int Attemps { get; set; }
        public string Nombres { get; set; }
        public long Telefono { get; set; }
        public DateTime? DateBlock { get; set; }     
    }
    public class ApplicationDbContext : IdentityDbContext
    {
        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<CodigoForgotPassword> CodigoForgotPasswords { get; set; }
        public virtual DbSet<Guide> Guides { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Sender> Senders { get; set; }
        public virtual DbSet<Receiver> Receivers { get; set; }
        public virtual DbSet<imagenestest> Imagenestests { get; set; }

        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }    

        
        public DbSet<CapturaCognitiva.Models.ViewModels.DetailsGuideViewModels> DetailsGuideViewModels { get; set; }

    }
}
