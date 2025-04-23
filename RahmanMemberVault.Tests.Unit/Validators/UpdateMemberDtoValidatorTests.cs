using FluentValidation.TestHelper;
using Xunit;
using RahmanMemberVault.Application.DTOs;
using RahmanMemberVault.Application.Validators;

namespace RahmanMemberVault.Tests.Unit.Validators
{
    public class UpdateMemberDtoValidatorTests
    {
        private readonly UpdateMemberDtoValidator _validator;

        public UpdateMemberDtoValidatorTests()
        {
            _validator = new UpdateMemberDtoValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Id_Is_Less_Than_Or_Equal_To_Zero()  // Invalid ID Test
        {
            // Arrange
            var dto = new UpdateMemberDto
            {
                Id = 0,
                Name = "Test",
                Email = "test@test.com",
                PhoneNumber = "1234567890"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_Have_Error_When_Name_Is_Null_Or_Empty(string name) // Invalid Name Test
        {
            // Arrange
            var dto = new UpdateMemberDto
            {
                Id = 1,
                Name = name,
                Email = "test@test.com",
                PhoneNumber = "1234567890"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("not-an-email")]
        public void Should_Have_Error_When_Email_Is_Invalid(string email) // Invalid Email Test
        {
            // Arrange
            var dto = new UpdateMemberDto
            {
                Id = 1,
                Name = "Test",
                Email = email,
                PhoneNumber = "1234567890"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("12345")]
        [InlineData("phone-number")]
        public void Should_Have_Error_When_PhoneNumber_Is_Invalid(string phone) // Invalid Phone Number Test
        {
            //  Arrange
            var dto = new UpdateMemberDto
            {
                Id = 1,
                Name = "Test",
                Email = "test@test.com",
                PhoneNumber = phone
            };

            //  Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
        }

        [Fact]
        public void Should_Not_Have_Errors_For_Valid_Dto() // Valid DTO Test
        {
            //  Arrange
            var dto = new UpdateMemberDto
            {
                Id = 1,
                Name = "Valid Name",
                Email = "valid@example.com",
                PhoneNumber = "1234567890"
            };

            // Act
            var result = _validator.TestValidate(dto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
