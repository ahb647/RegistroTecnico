﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RegistroTecnico.Contexto;

#nullable disable

namespace RegistroTecnico.Migrations
{
    [DbContext(typeof(TecnicoContext))]
    [Migration("20250204215441_Sistemas")]
    partial class Sistemas
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("RegistroTecnico.Models.Ciudad", b =>
                {
                    b.Property<int>("CiudadID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CiudadID"));

                    b.Property<string>("CiudadNombre")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CiudadID");

                    b.ToTable("Ciudades");
                });

            modelBuilder.Entity("RegistroTecnico.Models.Clientes", b =>
                {
                    b.Property<int>("ClienteID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClienteID"));

                    b.Property<string>("Direccion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaIngreso")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("LimiteCredito")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Nombres")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Rnc")
                        .HasColumnType("int");

                    b.Property<int>("TecnicoID")
                        .HasColumnType("int");

                    b.HasKey("ClienteID");

                    b.HasIndex("TecnicoID");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("RegistroTecnico.Models.Sistemas", b =>
                {
                    b.Property<int>("SistemaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SistemaId"));

                    b.Property<double>("Complejidad")
                        .HasColumnType("float");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("SistemaId");

                    b.ToTable("Sistemas");
                });

            modelBuilder.Entity("RegistroTecnico.Models.Tecnicos", b =>
                {
                    b.Property<int>("TecnicoID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TecnicoID"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("SueldoHora")
                        .HasColumnType("float");

                    b.HasKey("TecnicoID");

                    b.ToTable("Tecnicos");
                });

            modelBuilder.Entity("RegistroTecnico.Models.Tickets", b =>
                {
                    b.Property<int>("TicketID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TicketID"));

                    b.Property<string>("Asunto")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("ClienteID")
                        .HasColumnType("int");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<string>("Prioridad")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("TecnicoID")
                        .HasColumnType("int");

                    b.Property<decimal>("TiempoInvertido")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("TicketID");

                    b.HasIndex("ClienteID");

                    b.HasIndex("TecnicoID");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("RegistroTecnico.Models.Clientes", b =>
                {
                    b.HasOne("RegistroTecnico.Models.Tecnicos", "Tecnico")
                        .WithMany()
                        .HasForeignKey("TecnicoID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tecnico");
                });

            modelBuilder.Entity("RegistroTecnico.Models.Tickets", b =>
                {
                    b.HasOne("RegistroTecnico.Models.Clientes", "Cliente")
                        .WithMany()
                        .HasForeignKey("ClienteID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("RegistroTecnico.Models.Tecnicos", "Tecnico")
                        .WithMany()
                        .HasForeignKey("TecnicoID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Cliente");

                    b.Navigation("Tecnico");
                });
#pragma warning restore 612, 618
        }
    }
}
