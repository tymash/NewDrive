using FileStorage.BLL.Models.UserModels;
using FluentValidation;

namespace FileStorage.BLL.Validation.User;

public class UserLoginModelValidator : AbstractValidator<UserLoginModel>
{
    public UserLoginModelValidator()
    {
        RuleFor(um => um.Email).EmailAddress().Length(3, 255);
        RuleFor(um => um.Password).NotEmpty().Length(6, 16);
    }
}