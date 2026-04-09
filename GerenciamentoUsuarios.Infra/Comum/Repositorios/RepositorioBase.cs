using System.Linq.Expressions;
using GerenciamentoUsuarios.Dominio.libs.Repositorios;
using GerenciamentoUsuarios.Infra.Contexto;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoUsuarios.Infra.Comum.Repositorios;

public class RepositorioBase<T> : IRepositorioBase<T> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public RepositorioBase(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public void Inserir(T entidade) => _dbSet.Add(entidade);

    public void Editar(T entidade) => _dbSet.Update(entidade);

    public void Excluir(T entidade) => _dbSet.Remove(entidade);

    public T? Recuperar(int id) => _dbSet.Find(id);

    public T? Recuperar(Expression<Func<T, bool>> expressao) => _dbSet.FirstOrDefault(expressao);

    public IQueryable<T> Query() => _dbSet.AsQueryable();

    public async Task InserirAsync(T entidade, CancellationToken ct = default)
    {
        await _dbSet.AddAsync(entidade, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task EditarAsync(T entidade, CancellationToken ct = default)
    {
        _dbSet.Update(entidade);
        await _context.SaveChangesAsync(ct);
    }

    public async Task ExcluirAsync(T entidade, CancellationToken ct = default)
    {
        _dbSet.Remove(entidade);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<T?> RecuperarAsync(int id, CancellationToken ct = default)
        => await _dbSet.FindAsync([id], ct);

    public async Task<T?> RecuperarAsync(Expression<Func<T, bool>> expressao, CancellationToken ct = default)
        => await _dbSet.FirstOrDefaultAsync(expressao, ct);
}
