using FluentValidation;

namespace iCrabee.ViewModels.Contents
{
    public class VoteCreateRequestValidator : AbstractValidator<VoteCreateRequest>
    {
        public VoteCreateRequestValidator()
        {
            RuleFor(x => x.KnowledgeBaseId)
                .GreaterThan(0)
                .WithMessage(string.Format(Messages.Required, "Mã bài đăng"));
        }
    }
}
