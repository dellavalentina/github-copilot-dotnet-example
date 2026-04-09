using System.Linq.Expressions;

namespace GerenciamentoUsuarios.Dominio.libs.Repositorios;

public interface IRepositorioBase<T> where T : class
{
    void Inserir(T entidade);
    void Editar(T entidade);
    void Excluir(T entidade);
    T? Recuperar(int id);
    T? Recuperar(Expression<Func<T, bool>> expressao);
    IQueryable<T> Query();

    Task InserirAsync(T entidade, CancellationToken ct = default);
    Task EditarAsync(T entidade, CancellationToken ct = default);
    Task ExcluirAsync(T entidade, CancellationToken ct = default);
    Task<T?> RecuperarAsync(int id, CancellationToken ct = default);
    Task<T?> RecuperarAsync(Expression<Func<T, bool>> expressao, CancellationToken ct = default);
}
