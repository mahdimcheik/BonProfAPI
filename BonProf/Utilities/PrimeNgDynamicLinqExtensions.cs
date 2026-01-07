namespace BonProf.Utilities;

using BonProf.Models;
using System.Linq.Dynamic.Core;

public static class PrimeNgDynamicLinqExtensions
{
    public static IQueryable<T> ApplyCustomTableState<T>(
        this IQueryable<T> query,
        CustomTableState state)
    {
        if (state == null)
            return query;

        query = ApplyFilters(query, state.Filters);
        query = ApplyGlobalSearch(query, state.Search);
        query = ApplySorting(query, state.Sorts);

        return query;
    }

    private static IQueryable<T> ApplyFilters<T>(
    IQueryable<T> query,
    Dictionary<string, Filter> filters)
    {
        if (filters == null || filters.Count == 0)
            return query;

        var values = new List<object>();
        var conditions = new List<string>();

        foreach (var (field, filter) in filters)
        {
            if (filter?.Value == null || string.IsNullOrWhiteSpace(filter.Value.ToString()))
                continue;

            var index = values.Count;
            values.Add(filter.Value);

            if (IsCollectionField<T>(field))
            {
                conditions.Add(BuildAnyCondition(field, filter.MatchMode, index));
            }
            else
            {
                conditions.Add(BuildScalarCondition(field, filter.MatchMode, index));
            }
        }

        if (conditions.Count > 0)
        {
            var where = string.Join(" AND ", conditions);
            query = query.Where(where, values.ToArray());
        }

        return query;
    }

    private static string BuildScalarCondition(
    string field,
    string matchMode,
    int index)
    {
        return matchMode switch
        {
            "equals" => $"{field} == @{index}",
            "contains" => $"{field}.Contains(@{index})",
            "startsWith" => $"{field}.StartsWith(@{index})",
            "endsWith" => $"{field}.EndsWith(@{index})",
            "gte" => $"{field} >= @{index}",
            "lte" => $"{field} <= @{index}",
            "between" => BuildBetween(field, index),
            _ => throw new NotSupportedException($"MatchMode '{matchMode}' non supporté")
        };
    }
    private static string BuildBetween(string field, int index)
    {
        // index = début, index+1 = fin
        return $"{field} >= @{index} AND {field} <= @{index + 1}";
    }

    private static string BuildAnyCondition(
    string field,
    string matchMode,
    int index)
    {
        var parts = field.Split('.');
        var collection = parts[0];
        var property = parts[1];

        return matchMode switch
        {
            "equals" => $"{collection}.Any({property} == @{index})",
            "contains" => $"{collection}.Any({property}.Contains(@{index}))",
            _ => throw new NotSupportedException($"MatchMode '{matchMode}' non supporté sur collection")
        };
    }

    private static bool IsCollectionField<T>(string field)
    {
        var rootProperty = typeof(T).GetProperty(field.Split('.')[0]);
        return rootProperty != null &&
               typeof(System.Collections.IEnumerable)
                   .IsAssignableFrom(rootProperty.PropertyType) &&
               rootProperty.PropertyType != typeof(string);
    }

    private static IQueryable<T> ApplySorting<T>(
    IQueryable<T> query,
    List<SortCriterion> sorts)
    {
        if (sorts == null || sorts.Count == 0)
            return query;

        var orderClauses = sorts
            .Where(s => !string.IsNullOrWhiteSpace(s.Field))
            .Select(s => $"{s.Field} {(s.Order == 1 ? "asc" : "desc")}");

        return query.OrderBy(string.Join(", ", orderClauses));
    }

    private static IQueryable<T> ApplyGlobalSearch<T>(
    IQueryable<T> query,
    string search)
    {
        if (string.IsNullOrWhiteSpace(search))
            return query;

        var stringProps = typeof(T).GetProperties()
            .Where(p => p.PropertyType == typeof(string))
            .Select(p => $"{p.Name}.Contains(@0)");

        if (!stringProps.Any())
            return query;

        var where = string.Join(" OR ", stringProps);
        return query.Where(where, search);
    }
}



