﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MinhasFinancas.API.Data;

#nullable disable

namespace MinhasFinancas.API.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MinhasFinancas.Domain.Models.TransacaoFinanceira", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime2")
                        .HasColumnName("DATA");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("DESCRICAO");

                    b.Property<long>("IdUsuario")
                        .HasColumnType("bigint")
                        .HasColumnName("ID_USUARIO");

                    b.Property<int>("Tipo")
                        .HasColumnType("int")
                        .HasColumnName("TIPO");

                    b.Property<double>("Valor")
                        .HasColumnType("float")
                        .HasColumnName("VALOR");

                    b.HasKey("Id");

                    b.HasIndex("IdUsuario");

                    b.ToTable("TRANSACAO_FINANCEIRA");
                });

            modelBuilder.Entity("MinhasFinancas.Domain.Models.Usuario", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("ID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("EMAIL");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("NOME");

                    b.Property<string>("SenhaHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("SENHA_HASH");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("TOKEN");

                    b.Property<DateTime?>("ValidadeToken")
                        .HasColumnType("datetime2")
                        .HasColumnName("VALIDADE_TOKEN");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Nome")
                        .IsUnique();

                    b.ToTable("USUARIO");
                });

            modelBuilder.Entity("MinhasFinancas.Domain.Models.TransacaoFinanceira", b =>
                {
                    b.HasOne("MinhasFinancas.Domain.Models.Usuario", "Usuario")
                        .WithMany("TransacaoFinanceiras")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("MinhasFinancas.Domain.Models.Usuario", b =>
                {
                    b.Navigation("TransacaoFinanceiras");
                });
#pragma warning restore 612, 618
        }
    }
}