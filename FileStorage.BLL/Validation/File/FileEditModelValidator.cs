using FileStorage.BLL.Models.FileModels;
using FluentValidation;

namespace FileStorage.BLL.Validation.File;

/// <summary>

/// The file edit model validator class

/// </summary>

/// <seealso cref="AbstractValidator{FileEditModel}"/>

public class FileEditModelValidator : AbstractValidator<FileEditModel>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FileEditModelValidator"/> class
    /// </summary>
    public FileEditModelValidator()
    {
        RuleFor(f => f.Id).NotEmpty();
        RuleFor(f => f.Name).NotEmpty().Length(3, 100);
        RuleFor(f => f.Extension).MaximumLength(10);
        RuleFor(f => f.Path).Empty();
        RuleFor(f => f.IsRecycled).NotEmpty();
        RuleFor(f => f.IsPublic).NotEmpty();
    }
    
}