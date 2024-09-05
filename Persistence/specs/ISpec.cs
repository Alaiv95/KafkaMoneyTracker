using System.Linq.Expressions;

namespace Infrastructure.specs;

public interface ISpec<TEntity, TFilter>
{
    Expression<Func<TEntity, bool>> Build(TFilter filter);
}
