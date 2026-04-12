using System.Security.Claims;
using GerenciamentoUsuarios.Aplicacao.Enderecos.Servicos.Interfaces;
using GerenciamentoUsuarios.DataTransfer.Enderecos.Request;
using GerenciamentoUsuarios.DataTransfer.Enderecos.Response;
using GerenciamentoUsuarios.Dominio.libs.Consultas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoUsuarios.Api.Controllers.Enderecos;

[Route("api/enderecos")]
[ApiController]
[Authorize]
public class EnderecosController : ControllerBase
{
    private readonly IEnderecosAppServico _enderecosAppServico;

    public EnderecosController(IEnderecosAppServico enderecosAppServico)
    {
        _enderecosAppServico = enderecosAppServico;
    }

    private int ObterUsuarioAutenticadoId()
        => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    public async Task<ActionResult<EnderecosResponse>> Inserir([FromBody] EnderecosInserirRequest request, CancellationToken ct)
    {
        var usuarioId = ObterUsuarioAutenticadoId();
        var response = await _enderecosAppServico.InserirAsync(usuarioId, request, ct);
        return Ok(response);
    }

    [HttpGet]
    public ActionResult<PaginacaoConsulta<EnderecosResponse>> Listar([FromQuery] EnderecosListarRequest request)
    {
        var usuarioId = ObterUsuarioAutenticadoId();
        var response = _enderecosAppServico.Listar(usuarioId, request);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public ActionResult<EnderecosResponse> Recuperar(int id)
    {
        var usuarioId = ObterUsuarioAutenticadoId();
        var response = _enderecosAppServico.Recuperar(id, usuarioId);
        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<EnderecosResponse>> Editar(int id, [FromBody] EnderecosEditarRequest request, CancellationToken ct)
    {
        var usuarioId = ObterUsuarioAutenticadoId();
        var response = await _enderecosAppServico.EditarAsync(id, usuarioId, request, ct);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Desativar(int id, CancellationToken ct)
    {
        var usuarioId = ObterUsuarioAutenticadoId();
        await _enderecosAppServico.ExcluirAsync(id, usuarioId, ct);
        return Ok();
    }

    [HttpPatch("{id}/principal")]
    public async Task<ActionResult> DefinirPrincipal(int id, CancellationToken ct)
    {
        var usuarioId = ObterUsuarioAutenticadoId();
        await _enderecosAppServico.DefinirPrincipalAsync(id, usuarioId, ct);
        return Ok();
    }
}
