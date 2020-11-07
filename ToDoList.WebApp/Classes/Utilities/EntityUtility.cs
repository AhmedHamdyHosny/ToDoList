using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Classes.Utilities
{
    public class EntityUtility<TEntity>
    {
        public static Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> GetOrderBy(string orderColumn, string orderDir)
        {
            // get order by Query
            Type typeQueryable = typeof(IQueryable<TEntity>);
            ParameterExpression argQueryable = Expression.Parameter(typeQueryable);
            var outerExpression = Expression.Lambda(argQueryable, argQueryable);
            string[] props = orderColumn.Split('.');
            IQueryable<TEntity> query = new List<TEntity>().AsQueryable<TEntity>();
            Type type = typeof(TEntity);
            ParameterExpression arg = Expression.Parameter(type);

            Expression expr = arg;
            PropertyInfo pi = type.GetProperty(orderColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            expr = Expression.Property(expr, pi);
            type = pi.PropertyType;

            LambdaExpression lambda = Expression.Lambda(expr, arg);
            string methodName = orderDir.Equals("desc", StringComparison.OrdinalIgnoreCase) ? "OrderByDescending" : "OrderBy";

            MethodCallExpression resultExp =
                Expression.Call(typeof(Queryable), methodName, new Type[] { typeof(TEntity), type }, outerExpression.Body, Expression.Quote(lambda));
            var finalLambda = Expression.Lambda(resultExp, argQueryable);

            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderby = (Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>)finalLambda.Compile();

            return orderby;
        }
    }
}