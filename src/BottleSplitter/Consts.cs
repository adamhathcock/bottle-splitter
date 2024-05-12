using System;
using FluentValidation;
using FluentValidation.Validators;

namespace BottleSplitter;

public static class Consts
{
    public const string ClientId = "bottlespliter";

    public const string GitHubHome = "https://github.com/adamhathcock/bottle-splitter";

    public static IRuleBuilderOptions<T, TProperty> Url<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
        => ruleBuilder.SetValidator(new UrlValidator<T,TProperty>());
}

public class UrlValidator<T, TProperty> : PropertyValidator<T, TProperty>
{
    public override bool IsValid(ValidationContext<T> context, TProperty value)
    {
        if (value is string s && Uri.TryCreate(s, UriKind.Absolute, out _)) {
            return false;
        }

        return true;
    }

    public override string Name => "Url";
}
