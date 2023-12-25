using Domain.Common;
using Infrastructure.Persistence;
using Infrastructure.Specifications;
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

    public async Task<int?> DeleteByAsync(Expression<Func<TEntity,bool>> expression)
    {
        // return how many rows where deleted
        return await _context.Set<TEntity>().Where(expression).ExecuteDeleteAsync();
    }

    public async Task<int?> UpdateByAsync(Expression<Func<TEntity, bool>> whereExpression,
                                          Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> propertiesExpression,
                                          List<string>? includeExpressions = null)
    {
        IQueryable<TEntity> queryable = _context.Set<TEntity>();

        if (includeExpressions is not null)
        {
            includeExpressions.Aggregate(queryable, (current, includeExpression) => current.Include(includeExpression));
        }

        // return how many rows were affected by the update
        return await queryable.Where(whereExpression).ExecuteUpdateAsync(propertiesExpression);
        // properties expression are those who specifies a property and its corresponding value (its used by the execute update method) 
    }

    public void ChangeEntityContextTracker(object entity,EntityState entityState)
    {
        _context.Entry(entity).State = entityState;
    }

    public bool CheckHaveAnyData()
    {
        return _context.Set<TEntity>().Any();
    }

    public async Task<bool> CheckHaveAnyDataAsync()
    {
        return await _context.Set<TEntity>().AnyAsync();
    }

    public async Task<bool> CheckPropertyExistAsync(Expression<Func<TEntity,bool>> expression, List<string>? includeStatements = null)
    {
        IQueryable<TEntity> queryable = _context.Set<TEntity>();

        if (includeStatements is not null)
        {
            includeStatements.Aggregate(queryable, (current, include) => current.Include(include));
        }

        return await queryable.AnyAsync(expression);
    }
}
