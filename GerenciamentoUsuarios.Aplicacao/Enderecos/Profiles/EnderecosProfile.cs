using AutoMapper;
using GerenciamentoUsuarios.DataTransfer.Enderecos.Request;
using GerenciamentoUsuarios.DataTransfer.Enderecos.Response;
using GerenciamentoUsuarios.Dominio.Enderecos.Entidades;
using GerenciamentoUsuarios.Dominio.Enderecos.Servicos.Comandos;
using GerenciamentoUsuarios.Dominio.Enderecos.Servicos.Filtros;

namespace GerenciamentoUsuarios.Aplicacao.Enderecos.Profiles;

public class EnderecosProfile : Profile
{
    public EnderecosProfile()
    {
        CreateMap<EnderecosInserirRequest, EnderecosInserirComando>();
        CreateMap<EnderecosEditarRequest, EnderecosEditarComando>();
        CreateMap<EnderecosListarRequest, EnderecosListarFiltro>();
        CreateMap<Endereco, EnderecosResponse>();
    }
}
