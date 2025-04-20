using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using RahmanMemberVault.Application.DTOs;

namespace RahmanMemberVault.Application.Validators
{
    public class UpdateMemberDtoValidator : AbstractValidator<UpdateMemberDto>
    {
        public UpdateMemberDtoValidator()
        {
            // Id must be provided and greater than zero
            RuleFor(x => x.Id)
                .GreaterThan(0)
                    .WithMessage("Member ID must be greater than zero.");

            // Name is required and capped at 100 characters
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage("Name is required.")
                .MaximumLength(100)
                    .WithMessage("Name cannot exceed 100 characters.");

            // Email is required and must be in proper format
            RuleFor(x => x.Email)
                .NotEmpty()
                    .WithMessage("Email is required.")
                .EmailAddress()
                    .WithMessage("A valid email address is required.");

            // Phone number must be provided
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                    .WithMessage("Phone number is required.");
        }
    }
}