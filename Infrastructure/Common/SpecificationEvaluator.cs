using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Common;

public static class SpecificationEvaluator
{
    public static IQueryable<TEntity> GetQuery<TEntity>(IQueryable<TEntity> inputQueryable,
                                                        Specification<TEntity> specification)
        where TEntity : AuditableEntity
    {
        IQueryable<TEntity> queryable = inputQueryable;

        if (specification.Criteria is not null)
        {
            queryable = queryable.Where(specification.Criteria);
        }

        specification.IncludeExpressions
                     .Aggregate(queryable, (current, includeExpression) => current.Include(includeExpression));
        // Applies an accumulator over a sequence / Aggregate(seed,Func<seed,item> => ...)

        if (specification.IsSplitQuery)
        {
            //AsSplitQurey => improve perfomance doing joins operations
            queryable = queryable.AsSplitQuery();
        }

        if (specification.OrderByExpression is not null)
        {
            queryable = queryable.OrderBy(specification.OrderByExpression);
        }

        if (specification.OrderByDescendingExpression is not null)
        {
            queryable = queryable.OrderByDescending(specification.OrderByDescendingExpression);
        }

        return queryable;
    }
}
