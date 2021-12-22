using FluentValidation;

namespace iCrabee.ViewModels.Contents
{
    public class LabelCreateRequestValidator : AbstractValidator<LabelCreateRequest>
    {
        public LabelCreateRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(string.Format(Messages.Required, "Tên"));
        }
    }
}
