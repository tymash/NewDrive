using FileStorage.BLL.Models.UserModels;
using FluentValidation;

namespace FileStorage.BLL.Validation.User;

/// <summary>

/// The user login model validator class

/// </summary>

/// <seealso cref="AbstractValidator{UserLoginModel}"/>

public class UserLoginModelValidator : AbstractValidator<UserLoginModel>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserLoginModelValidator"/> class
    /// </summary>
    public UserLoginModelValidator()
    {
        RuleFor(um => um.Email).EmailAddress().Length(3, 255);
        RuleFor(um => um.Password).NotEmpty().Length(6, 16);
    }
}