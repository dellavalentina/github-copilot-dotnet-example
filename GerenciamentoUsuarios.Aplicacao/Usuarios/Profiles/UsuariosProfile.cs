using AutoMapper;
using GerenciamentoUsuarios.DataTransfer.Usuarios.Request;
using GerenciamentoUsuarios.DataTransfer.Usuarios.Response;
using GerenciamentoUsuarios.Dominio.Usuarios.Entidades;
using GerenciamentoUsuarios.Dominio.Usuarios.Servicos.Comandos;
using GerenciamentoUsuarios.Dominio.Usuarios.Servicos.Filtros;

namespace GerenciamentoUsuarios.Aplicacao.Usuarios.Profiles;

public class UsuariosProfile : Profile
{
    public UsuariosProfile()
    {
        CreateMap<UsuariosInserirRequest, UsuariosInserirComando>();
        CreateMap<UsuariosEditarRequest, UsuariosEditarComando>();
        CreateMap<UsuariosListarRequest, UsuariosListarFiltro>();
        CreateMap<Usuario, UsuariosResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForSourceMember(src => src.SenhaHash, opt => opt.DoNotValidate());
    }
}
