using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http.OData.Query;

namespace ODataToSql
{
    public static class ODataQueryOptionsExtensions
    {
        public static Expression<Func<TElement, bool>> ToExpression<TElement>(this FilterQueryOption filter)
        {
            if (filter == null)
            {
                return null;
            }
            
            IQueryable queryable = Enumerable.Empty<TElement>().AsQueryable();
            queryable = filter.ApplyTo(queryable, new ODataQuerySettings
            {
                EnableConstantParameterization = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            });

            var methodCallExp = queryable.Expression as MethodCallExpression;
            if (methodCallExp == null)
            {
                // return a default generic expression that validates to true
                return Expression.Lambda<Func<TElement, bool>>(Expression.Constant(true),
                    Expression.Parameter(typeof(TElement)));
            }


            var quote = methodCallExp.Arguments[1] as UnaryExpression;
            if (quote == null)
            {
                // return a default generic expression that validates to true
                return Expression.Lambda<Func<TElement, bool>>(Expression.Constant(true),
                    Expression.Parameter(typeof(TElement)));
            }

            return quote.Operand as Expression<Func<TElement, bool>>;
        }

        public static Expression ToExpression<TElement>(this OrderByQueryOption orderBy)
        {
            if (orderBy == null)
            {
                return null;
            }
            
            var queryable = Enumerable.Empty<TElement>().AsQueryable();
            queryable = orderBy.ApplyTo(queryable, new ODataQuerySettings
            {
                EnableConstantParameterization = false,
                HandleNullPropagation = HandleNullPropagationOption.False
            });

            return  queryable.Expression as MethodCallExpression;
        }

        public static int PageNumber(this ODataQueryOptions query)
        {
            if (query.Skip == null || query.Top == null) return 1;
            
            var page = query.Skip.Value / query.Top.Value;
            return query.Skip.Value % query.Top.Value > 0 ? page + 1 : page;
        }

        public static int PageSize(this ODataQueryOptions query)
        {
            return query.Top != null ? query.Top.Value : 25;
        }
    }
}
