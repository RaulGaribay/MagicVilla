﻿// <auto-generated />
using System;
using MagicVilla_API.Datos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MagicVillaAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240326155754_AlimentarTablaVilla")]
    partial class AlimentarTablaVilla
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MagicVilla_API.Modelos.Villa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Amenidad")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Detalle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaActualizacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImagenUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MetrosCuadrados")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Ocupantes")
                        .HasColumnType("int");

                    b.Property<double>("Tarifa")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Villas");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Amenidad = "",
                            Detalle = "Detalle de la villa...",
                            FechaActualizacion = new DateTime(2024, 3, 26, 9, 57, 54, 157, DateTimeKind.Local).AddTicks(5450),
                            FechaCreacion = new DateTime(2024, 3, 26, 9, 57, 54, 157, DateTimeKind.Local).AddTicks(5438),
                            ImagenUrl = "",
                            MetrosCuadrados = 7,
                            Nombre = "Villa prueba",
                            Ocupantes = 6,
                            Tarifa = 5.0
                        },
                        new
                        {
                            Id = 2,
                            Amenidad = "",
                            Detalle = "Detalle de la villa...",
                            FechaActualizacion = new DateTime(2024, 3, 26, 9, 57, 54, 157, DateTimeKind.Local).AddTicks(5454),
                            FechaCreacion = new DateTime(2024, 3, 26, 9, 57, 54, 157, DateTimeKind.Local).AddTicks(5453),
                            ImagenUrl = "",
                            MetrosCuadrados = 7,
                            Nombre = "Birlstone",
                            Ocupantes = 6,
                            Tarifa = 5.0
                        });
                });
#pragma warning restore 612, 618
        }
    }
}