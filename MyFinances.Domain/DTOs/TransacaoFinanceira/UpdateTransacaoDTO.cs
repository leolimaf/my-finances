﻿using System.ComponentModel.DataAnnotations;
using MyFinances.Domain.Models;

namespace MyFinances.Domain.DTOs.TransacaoFinanceira;

public class UpdateTransacaoDTO
{
    [Required]
    public string Descricao { get; set; }
    
    [Required]
    public DateTime Data { get; set; }
    
    [Required]
    public double Valor { get; set; }
    
    [Required]
    public TipoTransacao Tipo { get; set; }
    
    [Required] 
    public Guid IdUsuario { get; set; }
}