using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GameBling.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GameBling.Data
{
    public class GameContext : IdentityDbContext<User>
    {

        public GameContext(DbContextOptions<GameContext> options) : base(options) { }

        public DbSet<Card> Cards { get; set; }

        public override int SaveChanges()
        {
            var entities = from e in ChangeTracker.Entries()
                           where e.State == EntityState.Added
                               || e.State == EntityState.Modified
                           select e.Entity;
            foreach (var entity in entities)
            {
                var validationContext = new ValidationContext(entity);
                Validator.ValidateObject(entity, validationContext);
            }

            return base.SaveChanges();
        }

        protected void OnModelCreatingAsync(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Card>()
                .HasData(new Card()
                {
                    //Id = 1,
                    CardNumber = "123456",
                    ExpirationDate = DateTime.Now.AddMonths(1),
                    CVC = 100,
                    CardHolderName = "BeeDone",
                    //User = await _userManager.FindByIdAsync(userId: "1")
                });


        }
    }

}

