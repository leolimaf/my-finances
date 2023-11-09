using System.ComponentModel.DataAnnotations;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MyFinances.Domain.DTOs.TransacaoFinanceira;
using MyFinances.API.Services.Interfaces;
using Sieve.Models;

namespace MyFinances.API.Controllers;

[ApiController]
[Authorize(Policy = "Bearer")]
[ApiVersion("1.0")]
[Route("v{version:apiVersion}/transacoes-financeiras")]
[Produces("application/json")]
public class TransacaoFinanceiraController : ControllerBase
{
    private readonly ITransacaoFinanceiraService _transacaoFinanceiraService;

    public TransacaoFinanceiraController(ITransacaoFinanceiraService transacaoFinanceiraService)
    {
        _transacaoFinanceiraService = transacaoFinanceiraService;
    }

    /// <summary> Adiciona uma transação financeira</summary>
    /// <remarks>Realiza a entrada das receitas e despesas do usuário autenticado.</remarks>
    /// <response code="201">Requisição realizada com sucesso</response>
    [HttpPost, Route("adicionar")]
    [ProducesResponseType(201, Type = typeof(ReadTransacaoDTO))]
    public IActionResult AdicionarTransacaoFinanceira([FromBody] CreateTransacaoDTO transacaoDto)
    {
        ReadTransacaoDTO readTransacaoDto = _transacaoFinanceiraService.AdicionarTransacao(transacaoDto);
        return CreatedAtAction(nameof(ObterTransacaoPorId), new {version = HttpContext.GetRequestedApiVersion()!.ToString(), readTransacaoDto.Id} , readTransacaoDto);
    }
    
    /// <summary>Obtem uma transação financeira</summary>
    /// <remarks>A partir do identificador de uma transação financeira do usuário autenticado, é possível obte-lá.</remarks>
    /// <param name="id">Identificador da transação financeira</param>
    /// <response code="200">Requisição realizada com sucesso</response>
    /// <response code="404">A transação não foi encontrada</response>
    [HttpGet, Route("obter-por-id", Name = "obter-por-id"), EndpointName("obter-por-id")]
    [ProducesResponseType(200, Type = typeof(ReadTransacaoDTO))]
    [ProducesResponseType(404, Type = null!)]
    public IActionResult ObterTransacaoPorId(Guid id)
    {
        ReadTransacaoDTO readTransacaoDto = _transacaoFinanceiraService.ObterTransacaoPorId(id);
        if (readTransacaoDto is null) 
            return NotFound(readTransacaoDto);
        return Ok(readTransacaoDto);
    }

    /// <summary>Lista todas as transações financeiras</summary>
    /// <remarks>Retorna todas as transações financeiras do usuário autenticado.</remarks>
    /// <response code="200">Requisição realizada com sucesso</response>
    [HttpGet, Route("listar")]
    [ProducesResponseType(200, Type = typeof(List<ReadTransacaoDTO>))]
    public IActionResult ListarTransacoes([FromQuery] SieveModel model)
    {
        List<ReadTransacaoDTO> readTransacaoDtos = _transacaoFinanceiraService.ListarTransacoes(model);
        
        return Ok(readTransacaoDtos);
    }

    /// <summary>Atualiza uma transação financeira</summary>
    /// <remarks>A partir do identificador de uma transação financeira do usuário autenticado, é possível editá-la.</remarks>
    /// <param name="id">Identificador da transação financeira</param>
    /// <response code="204">Requisição realizada com sucesso</response>
    /// <response code="404">A transação não foi encontrada</response>
    [HttpPut, Route("atualizar")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404, Type = null!)]
    public IActionResult AtualizarTransacao([FromQuery, Required] Guid id, [FromBody] UpdateTransacaoDTO transacaoDto)
    {
        Result result = _transacaoFinanceiraService.AtualizarTransacao(id, transacaoDto);
        if (result.IsFailed)
            return NotFound();
        return NoContent();
    }
    
    /// <summary>Atualiza uma transação financeira parcialmente</summary>
    /// <remarks>A partir do identificador de uma transação financeira do usuário autenticado, é possível atualiza-la  utilizando o verto Patch do Http.</remarks>
    /// <param name="id">Identificador da transação financeira</param>
    /// <response code="204">Requisição realizada com sucesso</response>
    /// <response code="404">A transação não foi encontrada</response>
    [HttpPatch, Route("atualizar-parcialmente")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404, Type = null!)]
    public IActionResult AtualizarTransacaoParcialmente([FromQuery, Required] Guid id, [FromBody] JsonPatchDocument transacaoDto)
    {
        Result result = _transacaoFinanceiraService.AtualizarTransacaoParcialmente(id, transacaoDto);
        if (result.IsFailed)
            return NotFound();
        return NoContent();
    }

    /// <summary>Remove uma transação financeira</summary>
    /// <remarks>A partir do identificador de uma transação financeira do usuário autenticado, é possível apagá-la.</remarks>
    /// <param name="id">Identificador da transação financeira</param>
    /// <response code="204">Requisição realizada com sucesso</response>
    /// <response code="404">A transação não foi encontrada</response>
    [HttpDelete, Route("remover")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404, Type = null!)]
    public IActionResult RemoverTransacao([FromQuery] Guid id)
    {
        Result result = _transacaoFinanceiraService.RemoverTransacao(id);
        if (result.IsFailed)
            return NotFound();
        return NoContent();
    }
}