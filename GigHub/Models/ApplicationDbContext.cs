using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GigHub.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Gig> Gigs { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Attendence> Attendences { get; set; }
        public DbSet<Following> Followings { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Each attendance has required at least one gig & each gig has many attendances.
            modelBuilder.Entity<Attendence>()
                .HasRequired(u=>u.Gig)
                .WithMany(g=>g.Attendences)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Followers)
                .WithRequired(f=>f.Followee)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(m => m.Followees)
                .WithRequired(u=>u.Follower)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserNotification>()
                .HasRequired(u=>u.User)
                .WithMany(u=>u.UserNotifications)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}