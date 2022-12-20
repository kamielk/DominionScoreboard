using FluentValidation;

namespace DominionScoreboard.Games;

public class CreateGameRequestValidator : AbstractValidator<CreateGameRequest>
{
    public CreateGameRequestValidator()
    {
        RuleFor(game => game.Players).NotEmpty()
            .DependentRules(() =>
            {
                RuleForEach(game => game.Players).SetValidator(new PlayerValidator());
                RuleFor(game => game.Players.Count()).InclusiveBetween(2, 6);
            });
    }

    private class PlayerValidator : AbstractValidator<CreateGameRequest.Player>
    {
        public PlayerValidator()
        {
            RuleFor(player => player.Name).NotEmpty();
            RuleFor(player => player.Cards).NotNull();
        }
    }
}