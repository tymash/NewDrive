using FileStorage.BLL.Models.UserModels;
using FluentValidation;

namespace FileStorage.BLL.Validation.User;

/// <summary>

/// The user change password model validator class

/// </summary>

/// <seealso cref="AbstractValidator{UserChangePasswordModel}"/>

public class UserChangePasswordModelValidator : AbstractValidator<UserChangePasswordModel>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserChangePasswordModelValidator"/> class
    /// </summary>
    public UserChangePasswordModelValidator()
    {
        RuleFor(um => um.Id).NotEmpty();
        RuleFor(um => um.Password).NotEmpty().Length(6, 16);
    }

}