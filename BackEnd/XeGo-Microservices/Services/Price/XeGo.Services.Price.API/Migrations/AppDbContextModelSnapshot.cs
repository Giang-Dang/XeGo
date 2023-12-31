﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using XeGo.Services.Price.API.Data;

#nullable disable

namespace XeGo.Services.Price.API.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("XeGo.Services.Price.API.Entities.CodeValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EffectiveEndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("EffectiveStartDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SortOrder")
                        .HasColumnType("int");

                    b.Property<string>("Value1")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Value10")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Value10Type")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Value1Type")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Value2")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Value2Type")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Value3")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Value3Type")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Value4")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Value4Type")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Value5")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Value5Type")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Value6")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Value6Type")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Value7")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Value7Type")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Value8")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Value8Type")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Value9")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Value9Type")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.ToTable("CodeValues");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedBy = "",
                            CreatedDate = new DateTime(2023, 12, 9, 2, 47, 44, 599, DateTimeKind.Utc).AddTicks(8789),
                            EffectiveEndDate = new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999),
                            EffectiveStartDate = new DateTime(2023, 12, 9, 2, 47, 44, 599, DateTimeKind.Utc).AddTicks(8790),
                            IsActive = true,
                            LastModifiedBy = "",
                            LastModifiedDate = new DateTime(2023, 12, 9, 2, 47, 44, 599, DateTimeKind.Utc).AddTicks(8789),
                            Name = "DROP_CHARGE_THRESHOLD",
                            Value1 = "500",
                            Value1Type = "DOUBLE"
                        });
                });

            modelBuilder.Entity("XeGo.Services.Price.API.Entities.Discount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FromDay")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Percent")
                        .HasColumnType("float");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<DateTime>("ToDay")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Discounts");
                });

            modelBuilder.Entity("XeGo.Services.Price.API.Entities.Price", b =>
                {
                    b.Property<int>("RideId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DiscountId")
                        .HasColumnType("int");

                    b.Property<double>("DistanceInMeters")
                        .HasColumnType("float");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("TotalPrice")
                        .HasColumnType("float");

                    b.Property<int>("VehicleTypeId")
                        .HasColumnType("int");

                    b.HasKey("RideId");

                    b.HasIndex("DiscountId");

                    b.HasIndex("VehicleTypeId");

                    b.ToTable("Prices");
                });

            modelBuilder.Entity("XeGo.Services.Price.API.Entities.VehicleTypePrice", b =>
                {
                    b.Property<int>("VehicleTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VehicleTypeId"));

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("DropCharge")
                        .HasColumnType("float");

                    b.Property<string>("LastModifiedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("PricePerKm")
                        .HasColumnType("float");

                    b.HasKey("VehicleTypeId");

                    b.ToTable("VehicleTypePrices");

                    b.HasData(
                        new
                        {
                            VehicleTypeId = 1,
                            CreatedBy = "SYSTEM",
                            CreatedDate = new DateTime(2023, 12, 9, 2, 47, 44, 599, DateTimeKind.Utc).AddTicks(8650),
                            DropCharge = 1.0,
                            LastModifiedBy = "SYSTEM",
                            LastModifiedDate = new DateTime(2023, 12, 9, 2, 47, 44, 599, DateTimeKind.Utc).AddTicks(8650),
                            PricePerKm = 1.0
                        },
                        new
                        {
                            VehicleTypeId = 2,
                            CreatedBy = "SYSTEM",
                            CreatedDate = new DateTime(2023, 12, 9, 2, 47, 44, 599, DateTimeKind.Utc).AddTicks(8653),
                            DropCharge = 1.5,
                            LastModifiedBy = "SYSTEM",
                            LastModifiedDate = new DateTime(2023, 12, 9, 2, 47, 44, 599, DateTimeKind.Utc).AddTicks(8653),
                            PricePerKm = 1.5
                        });
                });

            modelBuilder.Entity("XeGo.Services.Price.API.Entities.Price", b =>
                {
                    b.HasOne("XeGo.Services.Price.API.Entities.Discount", "Discount")
                        .WithMany()
                        .HasForeignKey("DiscountId");

                    b.HasOne("XeGo.Services.Price.API.Entities.VehicleTypePrice", "VehicleTypePrice")
                        .WithMany()
                        .HasForeignKey("VehicleTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Discount");

                    b.Navigation("VehicleTypePrice");
                });
#pragma warning restore 612, 618
        }
    }
}
