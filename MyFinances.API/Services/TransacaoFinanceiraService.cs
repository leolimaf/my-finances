using System.Security.Claims;
using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.JsonPatch;
using MyFinances.Domain.DTOs.TransacaoFinanceira;
using MyFinances.Domain.Models;
using MyFinances.API.Data;
using MyFinances.API.Services.Interfaces;
using Sieve.Models;
using Sieve.Services;

namespace MyFinances.API.Services;

public class TransacaoFinanceiraService : ITransacaoFinanceiraService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly SieveProcessor _sieveProcessor;
    private readonly ILinkService _linkService;

    public TransacaoFinanceiraService(AppDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor, SieveProcessor sieveProcessor, ILinkService linkService)
    {
        _context = context;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _sieveProcessor = sieveProcessor;
        _linkService = linkService;
    }

    public ReadTransacaoDTO AdicionarTransacao(CreateTransacaoDTO transacaoDto)
    {
        TransacaoFinanceira transacao = _mapper.Map<TransacaoFinanceira>(transacaoDto);
        transacao.IdUsuario = ObterIdDoUsuarioAutenticado();
        _context.TransacoesFinanceiras.Add(transacao);
        _context.SaveChanges();
        return _mapper.Map<ReadTransacaoDTO>(transacao);
    }

    public ReadTransacaoDTO ObterTransacaoPorId(Guid id)
    {
        var transacao = _context.TransacoesFinanceiras.Find(id);

        if (transacao is null || transacao.IdUsuario != ObterIdDoUsuarioAutenticado()) 
            return null!;
        
        var transacaoDto = _mapper.Map<ReadTransacaoDTO>(transacao);
        AdicionarLinksParaTransacoes(transacaoDto);

        return transacaoDto;
    }

    public List<ReadTransacaoDTO> ListarTransacoes(SieveModel model)
    {
        var transacoes = _context.TransacoesFinanceiras
            .Where(x => x.IdUsuario == ObterIdDoUsuarioAutenticado());

        var readTransacaoDto = _mapper.Map<List<ReadTransacaoDTO>>(transacoes).AsQueryable();
        
        readTransacaoDto = _sieveProcessor.Apply(model, readTransacaoDto);
        
        return readTransacaoDto.ToList();
    }

    public Result AtualizarTransacao(Guid id, UpdateTransacaoDTO transacaoDto)
    {
        var transacao = _context.TransacoesFinanceiras.Find(id);
        
        var idUsuario = ObterIdDoUsuarioAutenticado();

        if (transacao is null || transacao.IdUsuario != idUsuario)
            return Result.Fail($"A transação financeira {id} do usuário {idUsuario} não foi encontrada.");
        
        transacao.IdUsuario = idUsuario;

        _mapper.Map(transacaoDto, transacao);
        _context.SaveChanges();
        return Result.Ok();
    }

    public Result AtualizarTransacaoParcialmente(Guid id, JsonPatchDocument transacaoDto)
    {
        var transacao = _context.TransacoesFinanceiras.Find(id);
        
        var idUsuario = ObterIdDoUsuarioAutenticado();
        
        if (transacao is null || transacao.IdUsuario != idUsuario)
            return Result.Fail($"A transação financeira {id} do usuário {idUsuario} não foi encontrada.");
        
        transacao.IdUsuario = idUsuario;
        
        transacaoDto.ApplyTo(transacao);
        _context.SaveChanges();
        return Result.Ok();
    }

    public Result RemoverTransacao(Guid id)
    {
        var transacao = _context.TransacoesFinanceiras.Find(id);
        
        var idUsuario = ObterIdDoUsuarioAutenticado();

        if (transacao is null || transacao.IdUsuario != idUsuario)
            return Result.Fail($"A transação financeira {id} do usuário {idUsuario} não foi encontrada.");
        
        transacao.IdUsuario = idUsuario;

        _context.Remove(transacao);
        _context.SaveChanges();
        return Result.Ok();
    }
    
    private Guid ObterIdDoUsuarioAutenticado()
    {
        var idUsuario = _httpContextAccessor.HttpContext?.User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        
        return idUsuario is not null ? new Guid(idUsuario) : default;
    }

    private void AdicionarLinksParaTransacoes(ReadTransacaoDTO transacaoDto)
    {
        transacaoDto.Links.Add(
            _linkService.Generate("obter-por-id", 
                new { transacaoDto.Id },
            "self",
            "GET"));
        
        transacaoDto.Links.Add(
            _linkService.Generate("atualizar", 
                new { transacaoDto.Id },
            "atualizar",
            "PUT"));
        
        transacaoDto.Links.Add(
            _linkService.Generate("remover", 
                new { transacaoDto.Id },
            "remover",
            "DELETE"));
    }
}