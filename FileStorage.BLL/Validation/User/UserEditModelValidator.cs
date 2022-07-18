using FileStorage.BLL.Models.UserModels;
using FluentValidation;

namespace FileStorage.BLL.Validation.User;

public class UserEditModelValidator : AbstractValidator<UserEditModel>
{
    public UserEditModelValidator()
    {
        RuleFor(um => um.Id).NotEmpty();
        RuleFor(um => um.Name).NotEmpty().Length(2, 30);
        RuleFor(um => um.Surname).NotEmpty().Length(2, 30);
        RuleFor(um => um.Email).EmailAddress().Length(3, 255);
    }

}