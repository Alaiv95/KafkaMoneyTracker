using System.Linq.Expressions;

namespace Application.specs;

public interface ISpec<TEntity, TFilter>
{
    Expression<Func<TEntity, bool>> Build(TFilter filter);
}
