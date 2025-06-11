using System.Linq.Expressions;

namespace Scoreboard.Cards.VictoryPoints;

public interface IExpression<TOut>
{
    TOut Evaluate(PlayerDeck deck, ICard currentCard);
}

public abstract record Expression<TOut> : IExpression<TOut>
{
    public abstract TOut Evaluate(PlayerDeck deck, ICard currentCard);
}

public record CountExpr(string Field, string Value, bool Distinct = false) : Expression<double>
{
    public override double Evaluate(PlayerDeck deck, ICard currentCard) => Field switch
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
    public override int Evaluate(PlayerDeck deck, ICard currentCard)
    {
        return (int)Math.Floor((double) Inner.Evaluate(deck, currentCard));
    }
}

public record BinaryExpr(IExpression<double> Left, string Op, IExpression<double> Right) : Expression<double>
{
    public override double Evaluate(PlayerDeck deck, ICard currentCard)
    {
        return Op switch
        {
            "/" => Left.Evaluate(deck, currentCard) / Right.Evaluate(deck, currentCard),
            "*" => Left.Evaluate(deck, currentCard) * Right.Evaluate(deck, currentCard),
            "+" => Left.Evaluate(deck, currentCard) + Right.Evaluate(deck, currentCard),
            "-" => Left.Evaluate(deck, currentCard) - Right.Evaluate(deck, currentCard),
            _ => throw new InvalidOperationException($"Unknown operator {Op}")
        };
    }
}

public record ConstExpr(double Value) : Expression<double>
{
    public override double Evaluate(PlayerDeck deck, ICard currentCard) => Value;
}

public record TernaryExpression(IExpression<bool> Condition, IExpression<double> IfTrue, IExpression<double> IfFalse) : Expression<double>
{
    public override double Evaluate(PlayerDeck deck, ICard currentCard)
    {
        return Condition.Evaluate(deck, currentCard) ? IfTrue.Evaluate(deck, currentCard) : IfFalse.Evaluate(deck, currentCard);
    }
}

public record ConditionExpression(string ConditionName) : IExpression<bool>
{
    public bool Evaluate(PlayerDeck deck, ICard currentCard)
    {
        return ConditionName switch
        {
            nameof(ICard.IsOnTavernMat) => currentCard.IsOnTavernMat,
            _ => throw new InvalidOperationException($"Unknown condition: {ConditionName}"),
        };
    }
}