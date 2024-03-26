﻿using MagicVilla_API.Modelos;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Datos
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) //base indica el padre
        {
            
        }

        public DbSet<Villa> Villas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa
                {
                    Id = 1,
                    Nombre = "Villa prueba",
                    Detalle = "Detalle de la villa...",
                    Tarifa = 5,
                    Ocupantes = 6,
                    MetrosCuadrados = 7,
                    ImagenUrl = "",
                    Amenidad = "",
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now
                },
                new Villa
                {
                    Id = 2,
                    Nombre = "Birlstone",
                    Detalle = "Detalle de la villa...",
                    Tarifa = 5,
                    Ocupantes = 6,
                    MetrosCuadrados = 7,
                    ImagenUrl = "",
                    Amenidad = "",
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now
                }
            );
        }
    }
}