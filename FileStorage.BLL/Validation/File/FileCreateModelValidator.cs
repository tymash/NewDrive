using FileStorage.BLL.Models.FileModels;
using FluentValidation;

namespace FileStorage.BLL.Validation.File;

/// <summary>

/// The file create model validator class

/// </summary>

/// <seealso cref="AbstractValidator{FileCreateModel}"/>

public class FileCreateModelValidator : AbstractValidator<FileCreateModel>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FileCreateModelValidator"/> class
    /// </summary>
    public FileCreateModelValidator()
    {
        RuleFor(f => f.Name).NotEmpty().Length(3, 100);
        RuleFor(f => f.Extension).MaximumLength(10);
        RuleFor(f => f.Path).NotEmpty();
        RuleFor(f => f.UserId).NotEmpty();
        RuleFor(f => f.IsRecycled).NotEmpty();
        RuleFor(f => f.IsPublic).NotEmpty();
    }

}