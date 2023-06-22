﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WweebbAapppp;

#nullable disable

namespace WweebbAapppp.Migrations
{
    [DbContext(typeof(VehicleQuotesContext))]
    partial class VehicleQuotesContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("VehicleQuotes.Models.BodyType", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("BodyTypes");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Name = "Coupe"
                        },
                        new
                        {
                            ID = 2,
                            Name = "Sedan"
                        },
                        new
                        {
                            ID = 3,
                            Name = "Hatchback"
                        },
                        new
                        {
                            ID = 4,
                            Name = "Wagon"
                        },
                        new
                        {
                            ID = 5,
                            Name = "Convertible"
                        },
                        new
                        {
                            ID = 6,
                            Name = "SUV"
                        },
                        new
                        {
                            ID = 7,
                            Name = "Truck"
                        },
                        new
                        {
                            ID = 8,
                            Name = "Mini Van"
                        },
                        new
                        {
                            ID = 9,
                            Name = "Roadster"
                        });
                });

            modelBuilder.Entity("VehicleQuotes.Models.Make", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("Makes");
                });

            modelBuilder.Entity("VehicleQuotes.Models.Model", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<int>("MakeID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ID");

                    b.HasIndex("MakeID");

                    b.HasIndex("Name", "MakeID")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("Models");
                });

            modelBuilder.Entity("VehicleQuotes.Models.ModelStyle", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<int>("BodyTypeID")
                        .HasColumnType("int");

                    b.Property<int>("ModelID")
                        .HasColumnType("int");

                    b.Property<int>("SizeID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("BodyTypeID");

                    b.HasIndex("SizeID");

                    b.HasIndex("ModelID", "BodyTypeID", "SizeID")
                        .IsUnique();

                    b.ToTable("ModelStyles");
                });

            modelBuilder.Entity("VehicleQuotes.Models.ModelStyleYear", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<int>("ModelStyleID")
                        .HasColumnType("int");

                    b.Property<string>("Year")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ID");

                    b.HasIndex("ModelStyleID");

                    b.HasIndex("Year", "ModelStyleID")
                        .IsUnique()
                        .HasFilter("[Year] IS NOT NULL");

                    b.ToTable("ModelStyleYears");
                });

            modelBuilder.Entity("VehicleQuotes.Models.Quote", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<int>("BodyTypeID")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("HasAllTires")
                        .HasColumnType("bit");

                    b.Property<bool>("HasAllWheels")
                        .HasColumnType("bit");

                    b.Property<bool>("HasAlloyWheels")
                        .HasColumnType("bit");

                    b.Property<bool>("HasCompleteInterior")
                        .HasColumnType("bit");

                    b.Property<bool>("HasEngine")
                        .HasColumnType("bit");

                    b.Property<bool>("HasKey")
                        .HasColumnType("bit");

                    b.Property<bool>("HasTitle")
                        .HasColumnType("bit");

                    b.Property<bool>("HasTransmission")
                        .HasColumnType("bit");

                    b.Property<bool>("ItMoves")
                        .HasColumnType("bit");

                    b.Property<string>("Make")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Model")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ModelStyleYearID")
                        .HasColumnType("int");

                    b.Property<int>("OfferedQuote")
                        .HasColumnType("int");

                    b.Property<bool>("RequiresPickup")
                        .HasColumnType("bit");

                    b.Property<int>("SizeID")
                        .HasColumnType("int");

                    b.Property<string>("Year")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("BodyTypeID");

                    b.HasIndex("ModelStyleYearID");

                    b.HasIndex("SizeID");

                    b.ToTable("Quotes");
                });

            modelBuilder.Entity("VehicleQuotes.Models.QuoteOverride", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<int>("ModelStyleYearID")
                        .HasColumnType("int");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("ModelStyleYearID")
                        .IsUnique();

                    b.ToTable("QuoteOverides");
                });

            modelBuilder.Entity("VehicleQuotes.Models.QuoteRule", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("FeatureType")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FeatureValue")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("PriceModifier")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("FeatureType", "FeatureValue")
                        .IsUnique()
                        .HasFilter("[FeatureType] IS NOT NULL AND [FeatureValue] IS NOT NULL");

                    b.ToTable("QuoteRules");
                });

            modelBuilder.Entity("VehicleQuotes.Models.Size", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ID");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasFilter("[Name] IS NOT NULL");

                    b.ToTable("Sizes");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Name = "Subcompact"
                        },
                        new
                        {
                            ID = 2,
                            Name = "Compact"
                        },
                        new
                        {
                            ID = 3,
                            Name = "Mid Size"
                        },
                        new
                        {
                            ID = 5,
                            Name = "Full Size"
                        });
                });

            modelBuilder.Entity("WweebbAapppp.Models.RefreshToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("VehicleQuotes.Models.Model", b =>
                {
                    b.HasOne("VehicleQuotes.Models.Make", "Make")
                        .WithMany()
                        .HasForeignKey("MakeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Make");
                });

            modelBuilder.Entity("VehicleQuotes.Models.ModelStyle", b =>
                {
                    b.HasOne("VehicleQuotes.Models.BodyType", "BodyType")
                        .WithMany()
                        .HasForeignKey("BodyTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VehicleQuotes.Models.Model", "Model")
                        .WithMany("ModelStyles")
                        .HasForeignKey("ModelID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VehicleQuotes.Models.Size", "Size")
                        .WithMany()
                        .HasForeignKey("SizeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BodyType");

                    b.Navigation("Model");

                    b.Navigation("Size");
                });

            modelBuilder.Entity("VehicleQuotes.Models.ModelStyleYear", b =>
                {
                    b.HasOne("VehicleQuotes.Models.ModelStyle", "ModelStyle")
                        .WithMany("ModelStyleYears")
                        .HasForeignKey("ModelStyleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ModelStyle");
                });

            modelBuilder.Entity("VehicleQuotes.Models.Quote", b =>
                {
                    b.HasOne("VehicleQuotes.Models.BodyType", "BodyType")
                        .WithMany()
                        .HasForeignKey("BodyTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("VehicleQuotes.Models.ModelStyleYear", "ModelStyleYear")
                        .WithMany()
                        .HasForeignKey("ModelStyleYearID");

                    b.HasOne("VehicleQuotes.Models.Size", "Size")
                        .WithMany()
                        .HasForeignKey("SizeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BodyType");

                    b.Navigation("ModelStyleYear");

                    b.Navigation("Size");
                });

            modelBuilder.Entity("VehicleQuotes.Models.QuoteOverride", b =>
                {
                    b.HasOne("VehicleQuotes.Models.ModelStyleYear", "ModelStyleYear")
                        .WithMany()
                        .HasForeignKey("ModelStyleYearID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ModelStyleYear");
                });

            modelBuilder.Entity("VehicleQuotes.Models.Model", b =>
                {
                    b.Navigation("ModelStyles");
                });

            modelBuilder.Entity("VehicleQuotes.Models.ModelStyle", b =>
                {
                    b.Navigation("ModelStyleYears");
                });
#pragma warning restore 612, 618
        }
    }
}
