using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BottleSplitter.Model;
using FluentValidation;

namespace BottleSplitter.Infrastructure;


public class BottleSplitFluentValidator : AbstractValidator<BottleSplit>
{
    public BottleSplitFluentValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(1,100);
        RuleFor(x => x.Settings).NotNull();

        RuleFor(x => x.Settings.DetailsUrl)
            .Url();

        RuleFor(x => x.Settings.ImageUrl)
            .Url();
    }

    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<BottleSplit>.CreateWithOptions((BottleSplit)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}
