using FileStorage.BLL.Models.UserModels;
using FluentValidation;

namespace FileStorage.BLL.Validation.User;

/// <summary>

/// The user register model validator class

/// </summary>

/// <seealso cref="AbstractValidator{UserRegisterModel}"/>

public class UserRegisterModelValidator : AbstractValidator<UserRegisterModel>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserRegisterModelValidator"/> class
    /// </summary>
    public UserRegisterModelValidator()
    {
        RuleFor(um => um.Name).NotEmpty().Length(2, 30);
        RuleFor(um => um.Surname).NotEmpty().Length(2, 30);
        RuleFor(um => um.Email).EmailAddress().Length(3, 255);
        RuleFor(um => um.Password).NotEmpty().Length(6, 16);
    }

}