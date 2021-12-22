﻿using FluentValidation;

namespace iCrabee.ViewModels.Contents
{
    public class CommentCreateRequestValidator : AbstractValidator<CommentCreateRequest>
    {
        public CommentCreateRequestValidator()
        {
            RuleFor(x => x.KnowledgeBaseId).GreaterThan(0)
                .WithMessage("Knowledge base Id is not valid");

            RuleFor(x => x.Content).NotEmpty().WithMessage("Content is required"); 
            
            RuleFor(x => x.CaptchaCode).NotEmpty()
               .WithMessage("Nhập mã xác nhận");
        }
    }
}
