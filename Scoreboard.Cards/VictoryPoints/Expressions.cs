namespace Scoreboard.Cards.VictoryPoints;

public interface IExpression<TOut>
{
    TOut Evaluate(PlayerDeck deck);
}

public abstract record Expression<TOut> : IExpression<TOut>
{
    public abstract TOut Evaluate(PlayerDeck deck);
}

public record CountExpr(string Field, string Value, bool Distinct = false) : Expression<double>
{
    public override double Evaluate(PlayerDeck deck) => Field switch
    {
        ExpressionConstants.Any => deck.CountAll(),
        ExpressionConstants.Type => deck.CountByType(Value),
        ExpressionConstants.Name when Distinct => deck.CountDistinctNames(),
        ExpressionConstants.Name => deck.CountByName(Value),
        _ => throw new InvalidOperationException($"Unknown field: {Field}"),
    };
}

public record FloorExpr(IExpression<double> Inner) : Expression<int>
{
    public override int Evaluate(PlayerDeck deck)
    {
        return (int)Math.Floor((double) Inner.Evaluate(deck));
    }
}

public record BinaryExpr(IExpression<double> Left, string Op, IExpression<double> Right) : Expression<double>
{
    public override double Evaluate(PlayerDeck deck)
    {
        return Op switch
        {
            "/" => Left.Evaluate(deck) / Right.Evaluate(deck),
            "*" => Left.Evaluate(deck) * Right.Evaluate(deck),
            "+" => Left.Evaluate(deck) + Right.Evaluate(deck),
            "-" => Left.Evaluate(deck) - Right.Evaluate(deck),
            _ => throw new InvalidOperationException($"Unknown operator {Op}")
        };
    }
}

public record ConstExpr(double Value) : Expression<double>
{
    public override double Evaluate(PlayerDeck deck) => Value;
}
