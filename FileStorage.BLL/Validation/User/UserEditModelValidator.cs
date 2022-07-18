using FileStorage.BLL.Models.UserModels;
using FluentValidation;

namespace FileStorage.BLL.Validation.User;

/// <summary>

/// The user edit model validator class

/// </summary>

/// <seealso cref="AbstractValidator{UserEditModel}"/>

public class UserEditModelValidator : AbstractValidator<UserEditModel>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserEditModelValidator"/> class
    /// </summary>
    public UserEditModelValidator()
    {
        RuleFor(um => um.Id).NotEmpty();
        RuleFor(um => um.Name).NotEmpty().Length(2, 30);
        RuleFor(um => um.Surname).NotEmpty().Length(2, 30);
        RuleFor(um => um.Email).EmailAddress().Length(3, 255);
    }

}