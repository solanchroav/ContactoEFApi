using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace contactoEFAPI.Models
{
    public partial class RecursosContext : DbContext
    {
        public RecursosContext()
        {
        }

        public RecursosContext(DbContextOptions<RecursosContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Contacto> Contactos { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-NFM67UE\\MSSQLSERVER1;Initial Catalog=Recursos;Integrated Security=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contacto>(entity =>
            {
                entity.Property(e => e.Direccion).HasColumnType("text");

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Empresa)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.FechaNacimiento)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_Nacimiento");

                entity.Property(e => e.ImagenPerfil)
                    .HasColumnType("text")
                    .HasColumnName("Imagen_Perfil");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(40)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
