using Sprache;

namespace Scoreboard.Cards.VictoryPoints;

public static class VpAlgorithmParser
{
    public static int Evaluate(string alg, IEnumerable<CardAndCount> cardAndCounts, ICard currentCard)
    {
        var deck = new PlayerDeck(cardAndCounts);

        // always floor result so we get an integer value
        var expression = new FloorExpr(Expr.End().Parse(alg));
        return expression.Evaluate(deck, currentCard);
    }

    // Matches strings like "count(type:Action)" or "count(name:Duchy)" or "count(*)"
    private static readonly Parser<IExpression<double>> _countExpr =
        Parse.String("count(")
            .Then(_ =>
                Parse.String("*").Select(_ => new CountExpr(ExpressionConstants.Any, ExpressionConstants.Any))
                .Or(
                    Identifier
                        .Then(firstToken =>
                            Parse.Char(':')
                                .Then(__ => Identifier.Select(value =>
                                {
                                    var isDistinct = firstToken.StartsWith(ExpressionConstants.Distinct);
                                    if (isDistinct)
                                    {
                                        // e.g., "name" from "distinct:name"
                                        var actualField = firstToken[ExpressionConstants.Distinct.Length..].TrimStart();
                                        return new CountExpr(value, ExpressionConstants.Any, Distinct: true);
                                    }

                                    return new CountExpr(firstToken, value);
                                })))
                )
            )
            .Then(expr => Parse.Char(')').Return(expr));

    // Simple integer constant
    private static readonly Parser<IExpression<double>> ConstExpr =
    Parse.Char('-').Or(Parse.Char('+')).Optional()
        .Then(sign => Parse.Number.Select(number =>
        {
            var prefix = sign.IsDefined ? sign.Get().ToString() : "";
            return new ConstExpr(double.Parse(prefix + number));
        }));

    // Binary operator parser: e.g. "count(...) / 3"
    private static readonly Parser<string> Operator =
        Parse.String("/")
            .Or(Parse.String("*"))
            .Or(Parse.String("+"))
            .Or(Parse.String("-"))
            .Text();

    private static readonly Parser<IExpression<double>> BinaryExpr =
       Parse.ChainOperator(Operator.Token(),
           _countExpr.Or(ConstExpr),
           (op, left, right) => new BinaryExpr(left, op, right)
       );

    // Identifier: letters only
    private static readonly Parser<string> Identifier = Parse.Letter.AtLeastOnce().Text();

    // The top-level entry point
    private static readonly Parser<IExpression<double>> Expr =
        BinaryExpr
            .Or(_countExpr)
            .Or(ConstExpr);
}