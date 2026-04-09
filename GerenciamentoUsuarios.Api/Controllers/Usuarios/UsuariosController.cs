using System.Security.Claims;
using GerenciamentoUsuarios.Aplicacao.Usuarios.Servicos.Interfaces;
using GerenciamentoUsuarios.DataTransfer.Usuarios.Request;
using GerenciamentoUsuarios.DataTransfer.Usuarios.Response;
using GerenciamentoUsuarios.Dominio.libs.Consultas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoUsuarios.Api.Controllers.Usuarios;

[Route("api/usuarios")]
[ApiController]
[Authorize]
public class UsuariosController : ControllerBase
{
    private readonly IUsuariosAppServico _usuariosAppServico;

    public UsuariosController(IUsuariosAppServico usuariosAppServico)
    {
        _usuariosAppServico = usuariosAppServico;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<UsuariosResponse>> Inserir([FromBody] UsuariosInserirRequest request, CancellationToken ct)
    {
        var response = await _usuariosAppServico.InserirAsync(request, ct);
        return Ok(response);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult> Login([FromBody] UsuariosLoginRequest request, CancellationToken ct)
    {
        var token = await _usuariosAppServico.LoginAsync(request, ct);
        return Ok(new { token });
    }

    [HttpGet]
    public ActionResult<PaginacaoConsulta<UsuariosResponse>> Listar([FromQuery] UsuariosListarRequest request)
    {
        var response = _usuariosAppServico.Listar(request);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public ActionResult<UsuariosResponse> Recuperar(int id)
    {
        var response = _usuariosAppServico.Recuperar(id);
        return Ok(response);
    }

    [HttpPut]
    public async Task<ActionResult<UsuariosResponse>> Editar([FromBody] UsuariosEditarRequest request, CancellationToken ct)
    {
        var usuarioAutenticadoId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        if (request.Id != usuarioAutenticadoId)
            return Unauthorized();

        var response = await _usuariosAppServico.EditarAsync(request, ct);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Desativar(int id, CancellationToken ct)
    {
        var usuarioAutenticadoId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _usuariosAppServico.ExcluirAsync(id, usuarioAutenticadoId, ct);
        return Ok();
    }

    [HttpPatch("{id}/reativar")]
    [Authorize(Roles = "Administrador")]
    public async Task<ActionResult> Reativar(int id, CancellationToken ct)
    {
        await _usuariosAppServico.ReativarAsync(id, ct);
        return Ok();
    }
}
