using MyFinances.Domain.DTOs.Link;
using MyFinances.Domain.Models;
using Sieve.Attributes;

namespace MyFinances.Domain.DTOs.TransacaoFinanceira;

public class ReadTransacaoDTO
{
    public Guid Id { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public string Descricao { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public DateTime Data { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public decimal Valor { get; set; }
    
    public TipoTransacao Tipo { get; set; }
    
    public Guid IdUsuario { get; set; }

    public List<LinkDTO> Links { get; set; } = new();
}