using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ChatApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public bool status { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }

        public string ImagePath { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("MyCon", throwIfV1Schema: false)
        {
            Database.SetInitializer(new ApplicationDBInitializer());
        }

        public DbSet<Message> Messages { get; set; } 
        public DbSet<Crud> Cruds { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }

    public class ApplicationDBInitializer : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {



            var store = new RoleStore<IdentityRole>(context);
            var manager = new RoleManager<IdentityRole>(store);
            // RoleTypes is a class containing constant string values for different roles
            List<IdentityRole> identityRoles = new List<IdentityRole>();
            identityRoles.Add(new IdentityRole() { Name = "Admin" });
            identityRoles.Add(new IdentityRole() { Name = "User" });

            foreach (IdentityRole role in identityRoles)
            {
                manager.Create(role);
            }

            // Initialize default user
            var store1 = new UserStore<ApplicationUser>(context);
            var manager1 = new UserManager<ApplicationUser>(store1);
            ApplicationUser admin = new ApplicationUser();
            admin.Email = "admin@gmail.com";
            admin.UserName = "admin@gmail.com";
            admin.EmailConfirmed = true;
            admin.status = true;
            admin.ImagePath = "~/Images/download.png";
            manager1.Create(admin, "Admin@123");
            manager1.AddToRole(admin.Id, "Admin");

            // Add code to initialize context tables


            base.Seed(context);



        }
    }

}
