using FileStorage.BLL.Models.FileModels;
using FluentValidation;

namespace FileStorage.BLL.Validation.File;

public class FileCreateModelValidator : AbstractValidator<FileCreateModel>
{
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