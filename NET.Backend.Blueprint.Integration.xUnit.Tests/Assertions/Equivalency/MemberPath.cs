using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace NET.Backend.Blueprint.Integration.xUnit.Tests.Assertions.Equivalency;

internal static partial class MemberPath
{
    private const string AnyItemSegment = "[]";

    public static string Of(LambdaExpression selector)
    {
        var names = new Stack<string>();
        var expression = Unwrap(selector.Body);

        while (expression is MemberExpression member)
        {
            names.Push(member.Member.Name);
            expression = Unwrap(member.Expression);
        }

        if (expression is not ParameterExpression)
        {
            throw new ArgumentException($"'{selector}' must be a member access chain on the lambda parameter.", nameof(selector));
        }

        return string.Join('.', names);
    }

    public static string Member(string parent, string name) => Combine(parent, name);

    public static string Item(string parent, object key) => Combine(parent, $"[{key}]");

    public static string AnyItem(string parent) => Combine(parent, AnyItemSegment);

    public static string Combine(string parent, string child)
    {
        if (parent.Length == 0) return child;
        if (child.Length == 0 || child.StartsWith('[')) return parent + child;
        return $"{parent}.{child}";
    }

    public static string Normalize(string path) => ItemIndex().Replace(path, AnyItemSegment);

    private static Expression? Unwrap(Expression? expression) =>
        expression is UnaryExpression { NodeType: ExpressionType.Convert or ExpressionType.ConvertChecked } convert
            ? convert.Operand
            : expression;

    [GeneratedRegex(@"\[\d+\]")]
    private static partial Regex ItemIndex();
}
