﻿// <auto-generated />
using System;
using Aircraft_project.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Aircraft_project.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231230144108_newUpdares")]
    partial class newUpdares
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Aircraft_project.Models.Admin", b =>
                {
                    b.Property<int>("AdminId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AdminId"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AdminId");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("Aircraft_project.Models.Aircraft", b =>
                {
                    b.Property<int>("AircraftId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AircraftId"));

                    b.Property<string>("AdditionalInformation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EngineType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("FuelCapacity")
                        .HasColumnType("float");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ManufacturingDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("MaxSpeed")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PassengerCapacity")
                        .HasColumnType("int");

                    b.Property<double>("Range")
                        .HasColumnType("float");

                    b.HasKey("AircraftId");

                    b.ToTable("Aircraft");
                });

            modelBuilder.Entity("Aircraft_project.Models.OrderItem", b =>
                {
                    b.Property<int>("OrderItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderItemId"));

                    b.Property<int>("AircraftId")
                        .HasColumnType("int");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<string>("Price")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("OrderItemId");

                    b.HasIndex("AircraftId");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("Aircraft_project.Models.Orders", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"));

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("OrderStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PostalCode")
                        .HasColumnType("int");

                    b.Property<string>("Shipment_Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("OrderId");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Aircraft_project.Models.Payment", b =>
                {
                    b.Property<int>("PaymentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentId"));

                    b.Property<string>("Amount")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.HasKey("PaymentId");

                    b.HasIndex("OrderId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("Aircraft_project.Models.Users", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MobileNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Aircraft_project.Models.OrderItem", b =>
                {
                    b.HasOne("Aircraft_project.Models.Aircraft", "Aircraft")
                        .WithMany()
                        .HasForeignKey("AircraftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Aircraft_project.Models.Orders", "Orders")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Aircraft");

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Aircraft_project.Models.Orders", b =>
                {
                    b.HasOne("Aircraft_project.Models.Users", "Users")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Aircraft_project.Models.Payment", b =>
                {
                    b.HasOne("Aircraft_project.Models.Orders", "Orders")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Aircraft_project.Models.Orders", b =>
                {
                    b.Navigation("OrderItems");
                });
#pragma warning restore 612, 618
        }
    }
}