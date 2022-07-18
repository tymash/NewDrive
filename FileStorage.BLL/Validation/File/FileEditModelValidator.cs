using FileStorage.BLL.Models.FileModels;
using FluentValidation;

namespace FileStorage.BLL.Validation.File;

public class FileEditModelValidator : AbstractValidator<FileEditModel>
{
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