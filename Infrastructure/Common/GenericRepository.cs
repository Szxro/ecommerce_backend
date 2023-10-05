using Domain.Common;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Common;

public abstract class GenericRepository<TEntity> where TEntity : AuditableEntity
{
    protected readonly AppDbContext _context;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TEntity?> GetBy(Expression<Func<TEntity, bool>> expression,
                                      ICollection<string>? includes = null)
    {
        IQueryable<TEntity>? query = _context.Set<TEntity>();

        if (includes is not null)
        {
            foreach (string entity in includes)
            {
                query = query.Include(entity);
            }

            return await query.AsSplitQuery().AsNoTracking().FirstOrDefaultAsync(expression);
            // AsSplitQurey => improve perfomance doing joins operations
            // AsNotTracking => improve perfomance 
        }

        return  await query.AsNoTracking().FirstOrDefaultAsync(expression);
    }

    public void Add(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);  
    }

    public void AddRange(ICollection<TEntity> entities)
    {
        _context.Set<TEntity>().AddRange(entities);
    }

    public void Delete(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
    }

    public void Update(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
    }

    public void ChangeTrackerToUnchanged(TEntity entity)
    {
        _context.Entry(entity).State = EntityState.Unchanged;
    }
}
