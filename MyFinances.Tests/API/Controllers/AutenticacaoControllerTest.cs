using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Mapster;
using MyFinances.Application.DTOs.Token;
using MyFinances.Application.DTOs.Usuario;
using MyFinances.Tests.Fixtures;
using MyFinances.Tests.Helpers.HttpHelper;
using MyFinances.Tests.Mappers;
using Xunit.Priority;

namespace MyFinances.Tests.API.Controllers;

[Collection(nameof(IntegrationApiTestFixtureCollection))]
[TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
public class AutenticacaoControllerTest
{
    private readonly WebApplicationFactoryFixture _factory;
    private readonly HttpClient _client;

    public AutenticacaoControllerTest(WebApplicationFactoryFixture factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
        _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_factory.AccessToken}");
        Mapper.ConfigureMapster();
    }

    [Fact(DisplayName = "Ao cadastrar um usuário deve ser retornado o usuário cadastrado")]
    [Trait("Autenticação e Autorização", "Cadastro"), Priority(1)]
    public async Task AoCadastrarUsuario()
    {
        // GIVEN
        var novoUsuario = DataFixture.ObterUsuarios(1, true).First();
        var usuarioRequest = novoUsuario.Adapt<CreateUsuarioDTO>();
        
        // WHEN
        var requisicao = await _client.PostAsJsonAsync(HttpHelper.UrlsUsuario.Cadastrar, usuarioRequest);
        var retorno = await requisicao.Content.ReadFromJsonAsync<ReadUsuarioDTO>();
        
        // THEN
        requisicao.StatusCode.Should().Be(HttpStatusCode.Created);
        
        Assert.NotNull(retorno);

        retorno.NomeCompleto.Should().Be(novoUsuario.NomeCompleto);
        retorno.Email.Should().Be(novoUsuario.Email);
    }

    [Fact(DisplayName = "Ao logar um usuário deve ser retornado o access token e o refresh token")]
    [Trait("Autenticação e Autorização", "Login"), Priority(2)]
    public async Task AoLogarUsuario()
    {
        // GIVEN
        var usuario = DataFixture.ObterUsuarios(1).First();
        var usuarioRequest = usuario.Adapt<LoginUsuarioDTO>();
        
        // WHEN
        var requisicao = await _client.PostAsJsonAsync(HttpHelper.UrlsUsuario.Logar, usuarioRequest);
        var retorno = await requisicao.Content.ReadFromJsonAsync<ReadLoginUsuarioDTO>();
        
        // THEN
        requisicao.StatusCode.Should().Be(HttpStatusCode.OK);
        
        Assert.NotNull(retorno);

        retorno.Authenticated.Should().Be(true);
        retorno.AccessToken.Should().NotBeNullOrWhiteSpace();
        retorno.RefreshToken.Should().NotBeNullOrWhiteSpace();
        
        // TODO: REMOVER DEPENDÊNCIA ENTRE OS TESTES
        _factory.AccessToken = retorno.AccessToken;
        _factory.RefreshToken = retorno.RefreshToken;
    }

    [Fact(DisplayName = "Ao atualizar o token do usuário autenticado deve ser retornado o novo access e refresh token")]
    [Trait("Autenticação e Autorização", "Login"), Priority(3)]
    public async Task AoAtualizarToken()
    {
        // GIVEN
        var atualizacaoTokenRequest = new TokenDTO(_factory.AccessToken, _factory.RefreshToken);

        // WHEN
        var requisicao = await _client.PostAsJsonAsync(HttpHelper.UrlsUsuario.AtualizarToken, atualizacaoTokenRequest);
        var retorno = await requisicao.Content.ReadFromJsonAsync<ReadLoginUsuarioDTO>();

        // THEN
        requisicao.StatusCode.Should().Be(HttpStatusCode.OK);
        
        Assert.NotNull(retorno);

        retorno.Authenticated.Should().Be(true);
        retorno.AccessToken.Should().NotBeEquivalentTo(atualizacaoTokenRequest.AccessToken);
        retorno.RefreshToken.Should().NotBeEquivalentTo(atualizacaoTokenRequest.RefreshToken);
        
        // TODO: REMOVER DEPENDÊNCIA ENTRE OS TESTES
        _factory.AccessToken = retorno.AccessToken;
        _factory.RefreshToken = retorno.RefreshToken;
    }

    [Fact(DisplayName = "Ao deslogar o usuário, seu acesso deve ser invalidado")]
    [Trait("Autenticação e Autorização", "Logoff"), Priority(4)]
    public async Task AoRevogarToken()
    {
        // GIVEN 
        
        // WHEN
        var requisicao = await _client.PostAsJsonAsync(HttpHelper.UrlsUsuario.Deslogar, new {});
        
        // THEN
        requisicao.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

}