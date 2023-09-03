﻿using ControleFinanceiro.API.Models;

namespace ControleFinanceiro.API.DTOs.TransacaoFinanceira;

public class CreateTransacaoDto
{
    public string Descricao { get; set; }
    
    public DateTime Data { get; set; }
    
    public decimal Valor { get; set; }
    
    public TipoTransacao Tipo { get; set; }
}