using FluentValidation;

namespace NZWalks.API.Validators
{
    public class UpdateWalkValidator : AbstractValidator<Models.DTO.UpdateWalkRequest>
    {
        public UpdateWalkValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Length).GreaterThan(0);

        }
    }
}
