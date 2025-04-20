using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using RahmanMemberVault.Application.DTOs;
using RahmanMemberVault.Application.Validators;
using Xunit;

namespace RahmanMemberVault.Tests.Unit.Validators
{
    public class CreateMemberDtoValidatorTests
    {
        // This test class is for the CreateMemberDtoValidator
        // It uses FluentValidation's TestHelper to validate the CreateMemberDto
        // The CreateMemberDtoValidator is responsible for validating the CreateMemberDto
        private readonly CreateMemberDtoValidator _validator;

        public CreateMemberDtoValidatorTests()
        {
            _validator = new CreateMemberDtoValidator();
        }

        
        [Fact]
        public void Should_Pass_When_ValidDto() // Valid DTO Test
        {
            // Arrange
            var dto = new CreateMemberDto
            {
                Name = "Mustafizur",
                Email = "Mustafizur@gmail.com",
                PhoneNumber = "+1234567890"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_Fail_When_NameMissing() // Missing Name Test
        {
            // Arrange
            var dto = new CreateMemberDto
            {
                Name = string.Empty,
                Email = "Mustafizur@gmail.com",
                PhoneNumber = "+1234567890"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("Name is required.");
        }

        [Fact]
        public void Should_Fail_When_NameTooLong() // Name Too Long Test
        {
            // Arrange
            var longName = new string('A', 101); // 101 characters long
            var dto = new CreateMemberDto
            {
                Name = longName,
                Email = "Mustafizur@gmail.com",
                PhoneNumber = "+1234567890"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name)
                  .WithErrorMessage("Name cannot exceed 100 characters.");
        }

        [Fact]
        public void Should_Fail_When_EmailInvalid() // Invalid Email Test
        {
            // Arrange
            var dto = new CreateMemberDto
            {
                Name = "Mustafizur",
                Email = "Mustafizur", // Invalid email format
                PhoneNumber = "+1234567890"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email)
                  .WithErrorMessage("A valid email address is required.");
        }

        [Fact]
        public void Should_Fail_When_PhoneMissing() // Missing Phone Number Test
        {
            // Arrange
            var dto = new CreateMemberDto
            {
                Name = "Mustafizur",
                Email = "Mustafizur@gmail.com",
                PhoneNumber = string.Empty // Missing phone number
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
                  .WithErrorMessage("Phone number is required.");
        }
    }
}
