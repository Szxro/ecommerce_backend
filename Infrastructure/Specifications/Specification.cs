using Domain.Common;
using System.Linq.Expressions;

namespace Infrastructure.Specifications;

public abstract class Specification<TEntity>
    where TEntity : AuditableEntity // Generic constraint
{
    protected Specification(Expression<Func<TEntity, bool>> criteria)
    {
        Criteria = criteria;
    }

    public Expression<Func<TEntity, bool>>? Criteria { get; private set; } // represent the where statement

    public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = new();

    public Expression<Func<TEntity, object>>? OrderByExpression { get; private set; }

    public Expression<Func<TEntity, object>>? OrderByDescendingExpression { get; private set; }

    public Expression<Func<TEntity, bool>>? AnyExpression { get; private set; }

    public bool IsSplitQuery { get; protected set; }

    protected void AddInclude(Expression<Func<TEntity, object>> includeExpression) => IncludeExpressions.Add(includeExpression);

    protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression) => OrderByExpression = orderByExpression;

    protected void AddOrderDescendingBy(Expression<Func<TEntity, object>> orderByExpression) => OrderByDescendingExpression = orderByExpression;

    protected void AddAnyExpression(Expression<Func<TEntity, bool>> anyExpression) => AnyExpression = anyExpression;
}
