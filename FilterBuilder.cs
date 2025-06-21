using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using TestExpressionTree.Models;

namespace TestExpressionTree.Controllers;

public static class FilterBuilder
{
    public static Expression<Func<T, bool>> BuildExpression<T>(FilterComponent filter)
    {
        var param = Expression.Parameter(typeof(T), "e");
        var body = BuildExpressionBody<T>(filter, param);
        return Expression.Lambda<Func<T, bool>>(body, param);
    }

    private static Expression BuildExpressionBody<T>(FilterComponent component, ParameterExpression param)
    {
        if (component is FilterRule rule)
            return BuildRuleExpression<T>(rule, param);

        var group = component as FilterGroup;
        var expressions = group.Filters
            .Select(f => BuildExpressionBody<T>(f, param))
            .ToList();

        return group.LogicalOperator.Equals("And", StringComparison.OrdinalIgnoreCase)
            ? expressions.Aggregate(Expression.AndAlso)
            : expressions.Aggregate(Expression.OrElse);
    }

    private static Expression BuildRuleExpression<T>(FilterRule rule, ParameterExpression param)
    {
        var member = Expression.PropertyOrField(param, rule.Field);
        var constant = Expression.Constant(rule.Value);

        return rule.Operator switch
        {
            "Equals" => Expression.Equal(member, Expression.Convert(constant, member.Type)),
            "GreaterThan" => Expression.GreaterThan(member, Expression.Convert(constant, member.Type)),
            "LessThan" => Expression.LessThan(member, Expression.Convert(constant, member.Type)),
            "In" => BuildInExpression(member, rule.Value),
            _ => throw new NotSupportedException($"Operator {rule.Operator} not supported")
        };
    }

    private static Expression BuildInExpression(Expression member, object value)
    {
        var values = (IEnumerable<object>)value;
        var elementType = member.Type;
        var containsMethod = typeof(Enumerable)
            .GetMethods()
            .First(m => m.Name == "Contains" && m.GetParameters().Length == 2)
            .MakeGenericMethod(elementType);

        // Create a typed list from the values, from object to T
        var typedList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));
        foreach (var val in (IEnumerable<object>)values)
        {
            typedList.Add(Convert.ChangeType(val, elementType));
        }

        // Wrap the list(values) and Type (IEnumerable<T>)
        var valuesConst = Expression.Constant(typedList,
            typeof(IEnumerable<>).MakeGenericType(elementType));

        // Call Contains
        return Expression.Call(containsMethod, valuesConst, member);
    }
}