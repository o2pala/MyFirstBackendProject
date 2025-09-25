using System.Linq.Expressions;

namespace TodoApi.ViewModels
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> TakePage<T>(this IQueryable<T> query, int pageIndex, int pageSize, int rowCount, out int actualPageIndex)
        {
            actualPageIndex = pageIndex;
            if (pageIndex >= 0)
            {
                int num = pageIndex * pageSize;
                if (num < rowCount)
                {
                    query = query.Skip(num).Take(pageSize);
                }
                else
                {
                    query = query.Take(pageSize);
                    actualPageIndex = 0;
                }
            }

            return query;
        }
        public static IQueryable<T> SortWith<T>(this IQueryable<T> query, string name, bool ascending = true)
        {
            if (string.IsNullOrEmpty(name))
            {
                return query;
            }

            ParameterExpression parameterExpression = Expression.Parameter(typeof(T), "p");
            MemberExpression memberExpression = Expression.PropertyOrField(parameterExpression, name);
            MethodCallExpression expression = Expression.Call(typeof(Queryable), ascending ? "OrderBy" : "OrderByDescending", new Type[2] { query.ElementType, memberExpression.Type }, query.Expression, Expression.Quote(Expression.Lambda(memberExpression, parameterExpression)));
            return query.Provider.CreateQuery<T>(expression);
        }
    }
}
