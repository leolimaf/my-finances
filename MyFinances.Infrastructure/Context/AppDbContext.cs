﻿using Microsoft.EntityFrameworkCore;
using MyFinances.Domain.Models;

namespace MyFinances.Infrastructure.Context;

public class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TransacaoFinanceira>()
            .HasOne(transacao => transacao.Usuario)
            .WithMany(usuario => usuario.TransacaoFinanceiras)
            .HasForeignKey(transacao => transacao.IdUsuario);
    }

    public DbSet<TransacaoFinanceira> TransacoesFinanceiras { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
}