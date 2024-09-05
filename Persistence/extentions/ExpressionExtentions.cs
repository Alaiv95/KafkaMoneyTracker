﻿using System.Linq.Expressions;

namespace Infrastructure.extentions;

public static class ExpressionExtentions
{
    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        var param = Expression.Parameter(typeof(T), "x");
        var body = Expression.AndAlso(
                Expression.Invoke(left, param),
                Expression.Invoke(right, param)
            );

        return Expression.Lambda<Func<T, bool>>(body, param);
    }

    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
    {
        var param = Expression.Parameter(typeof(T), "x");
        var body = Expression.OrElse(
                Expression.Invoke(left, param),
                Expression.Invoke(right, param)
            );

        return Expression.Lambda<Func<T, bool>>(body, param);
    }
}
