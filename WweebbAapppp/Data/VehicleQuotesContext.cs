﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VehicleQuotes.Models;
using WweebbAapppp.Models;
using NamespaceChat;


namespace WweebbAapppp
{
    public class VehicleQuotesContext : IdentityUserContext<IdentityUser>
    {
        public VehicleQuotesContext(DbContextOptions<VehicleQuotesContext> options)
            : base(options)
        {
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Make> Makes { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<BodyType> BodyTypes { get; set; }

        public DbSet<Model> Models { get; set; }
        public DbSet<ModelStyle> ModelStyles { get; set; }
        public DbSet<ModelStyleYear> ModelStyleYears { get; set; }

        public DbSet<QuoteRule> QuoteRules { get; set; }
        public DbSet<QuoteOverride> QuoteOverides { get; set; }

        public DbSet<Quote> Quotes { get; set; }



        public DbSet<Chat> Chat { get; set; }
        public DbSet<Message> Messages { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Size>().HasData(
                new Size { ID = 1, Name = "Subcompact" },
                new Size { ID = 2, Name = "Compact" },
                new Size { ID = 3, Name = "Mid Size" },
                new Size { ID = 5, Name = "Full Size" }
            );

            modelBuilder.Entity<BodyType>().HasData(
                new BodyType { ID = 1, Name = "Coupe" },
                new BodyType { ID = 2, Name = "Sedan" },
                new BodyType { ID = 3, Name = "Hatchback" },
                new BodyType { ID = 4, Name = "Wagon" },
                new BodyType { ID = 5, Name = "Convertible" },
                new BodyType { ID = 6, Name = "SUV" },
                new BodyType { ID = 7, Name = "Truck" },
                new BodyType { ID = 8, Name = "Mini Van" },
                new BodyType { ID = 9, Name = "Roadster" }
            );
        }

      /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=MyNewBD;Trusted_Connection=True;");
        }*/
    }
}