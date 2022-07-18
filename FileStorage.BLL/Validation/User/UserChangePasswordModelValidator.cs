using FileStorage.BLL.Models.UserModels;
using FluentValidation;

namespace FileStorage.BLL.Validation.User;

public class UserChangePasswordModelValidator : AbstractValidator<UserChangePasswordModel>
{
    public UserChangePasswordModelValidator()
    {
        RuleFor(um => um.Id).NotEmpty();
        RuleFor(um => um.Password).NotEmpty().Length(6, 16);
    }

}