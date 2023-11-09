using Domain.Common;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Infrastructure.Common;

public abstract class GenericRepository<TEntity> where TEntity : AuditableEntity
{
    protected readonly AppDbContext _context;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
    }

    public IQueryable<TEntity> ApplySpecification(Specification<TEntity> specification)
    {
        return SpecificationEvaluator.GetQuery(_context.Set<TEntity>(), specification);
    }

    public void Add(TEntity entity)
    {
        _context.Set<TEntity>().Add(entity);  
    }

    public void AddRange(ICollection<TEntity> entities)
    {
        _context.Set<TEntity>().AddRange(entities);
    }

    public Task DeleteByAsync(Expression<Func<TEntity,bool>> expression)
    {
        // return how many rows where deleted
        return _context.Set<TEntity>().Where(expression).ExecuteDeleteAsync();
    }

    public Task UpdateByAsync(Expression<Func<TEntity, bool>> whereExpression,
                              Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> propertiesExpression)
    {
        // return how many rows were affected by the update
        return _context.Set<TEntity>().Where(whereExpression).ExecuteUpdateAsync(propertiesExpression);

        // properties expression are those who specifies a property and its corresponding value (its used by the execute update method) 
    }

    public void ChangeTrackerToUnchanged(TEntity entity)
    {
        _context.Entry(entity).State = EntityState.Unchanged;
    }
}
