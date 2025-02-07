using HorseRacing.Application.Common.Exceptions;
using HorseRacing.Domain.Common.Errors;
using System.Linq.Expressions;
using System.Reflection;

namespace HorseRacing.Infrastructure.Extension
{
    public static class OrderByExtension
    {
        /// <summary>
        /// Сортировка по возрастанию
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="query">Запрос</param>
        /// <param name="propertyName">Наименование поля. Пример "FirstName"</param>
        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> query, string propertyName)
        {
            return query.BaseOrderBy(propertyName, "OrderBy");
        }
        /// <summary>
        /// Сортировка по убыванию
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="query">Запрос</param>
        /// <param name="propertyName">Наименование поля. Пример "FirstName"</param>
        public static IOrderedQueryable<TSource> OrderByDescending<TSource>(this IQueryable<TSource> query, string propertyName)
        {
            return query.BaseOrderBy(propertyName, "OrderByDescending");
        }
        /// <summary>
        /// Вторичная сортировка по возрастанию
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="query">Запрос</param>
        /// <param name="propertyName">Наименование поля. Пример "FirstName"</param>
        public static IOrderedQueryable<TSource> ThenBy<TSource>(this IQueryable<TSource> query, string propertyName)
        {
            return query.BaseOrderBy(propertyName, "ThenBy");
        }
        /// <summary>
        /// Вторичная сортировка по убыванию
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="query">Запрос</param>
        /// <param name="propertyName">Наименование поля. Пример "FirstName"</param>
        public static IOrderedQueryable<TSource> ThenByDescending<TSource>(this IQueryable<TSource> query, string propertyName)
        {
            return query.BaseOrderBy(propertyName, "ThenByDescending");
        }
        /// <summary>
        /// Базовый метод сортировки
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="query">Запрос</param>
        /// <param name="propertyName">Наименование поля. Пример "FirstName"</param>
        /// <param name="sortOrder">Тип сортировки. Пример "OrderBy"</param>
        private static IOrderedQueryable<TSource> BaseOrderBy<TSource>(this IQueryable<TSource> query, string propertyName, string sortOrder)
        {
            var entityType = typeof(TSource);

            var propertyInfo = entityType.GetProperty(propertyName);

            if (propertyInfo == null)
                throw new TableOrderArgsException(Errors.Table.UnknownField.Description);

            ParameterExpression arg = Expression.Parameter(entityType, "x");
            MemberExpression property = Expression.Property(arg, propertyName);
            var selector = Expression.Lambda(property, new ParameterExpression[] { arg });

            var enumarableType = typeof(System.Linq.Queryable);
            var method = enumarableType.GetMethods()
                 .Where(m => m.Name == sortOrder && m.IsGenericMethodDefinition)
                 .Where(m =>
                 {
                     var parameters = m.GetParameters().ToList();
                     return parameters.Count == 2;
                 }).Single();

            MethodInfo genericMethod = method.MakeGenericMethod(entityType, propertyInfo.PropertyType);

            return (IOrderedQueryable<TSource>)genericMethod.Invoke(genericMethod, new object[] { query, selector });
        }
    }
}
