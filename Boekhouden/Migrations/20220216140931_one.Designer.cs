﻿// <auto-generated />
using System;
using Boekhouden.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Boekhouden.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220216140931_one")]
    partial class one
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("Boekhouden.CustomerDiscount", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<double>("DiscountAmount")
                        .HasColumnType("float");

                    b.Property<int>("Percentage")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("CustomerDiscount");
                });

            modelBuilder.Entity("Boekhouden.Invoice", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int?>("CustomerDiscountID")
                        .HasColumnType("int");

                    b.Property<string>("OrderDateTime")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("SubTotal")
                        .HasColumnType("float");

                    b.Property<string>("TableNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Total")
                        .HasColumnType("float");

                    b.HasKey("ID");

                    b.HasIndex("CustomerDiscountID");

                    b.ToTable("Invoice");
                });

            modelBuilder.Entity("Boekhouden.TransactionRow", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int?>("InvoiceID")
                        .HasColumnType("int");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("ProductDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("TransactionRowDiscount")
                        .HasColumnType("float");

                    b.Property<double>("VatAmount")
                        .HasColumnType("float");

                    b.Property<int>("VatType")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("InvoiceID");

                    b.ToTable("TransactionRow");
                });

            modelBuilder.Entity("Boekhouden.Invoice", b =>
                {
                    b.HasOne("Boekhouden.CustomerDiscount", "CustomerDiscount")
                        .WithMany()
                        .HasForeignKey("CustomerDiscountID");

                    b.Navigation("CustomerDiscount");
                });

            modelBuilder.Entity("Boekhouden.TransactionRow", b =>
                {
                    b.HasOne("Boekhouden.Invoice", null)
                        .WithMany("TransactionRows")
                        .HasForeignKey("InvoiceID");
                });

            modelBuilder.Entity("Boekhouden.Invoice", b =>
                {
                    b.Navigation("TransactionRows");
                });
#pragma warning restore 612, 618
        }
    }
}
