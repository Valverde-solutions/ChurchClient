using System.Text.RegularExpressions;
using ChurchSaaS.Admin.Domain.Abstractions;

namespace ChurchSaaS.Admin.Domain.ValueObjects;

public sealed class Email : ValueObject
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email is required.", nameof(value));

        value = value.Trim();

        // Validação simples; pode ser melhorada no futuro
        if (!Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            throw new ArgumentException("Invalid email format.", nameof(value));

        return new Email(value);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        // Email case-insensitive
        yield return Value.ToUpperInvariant();
    }

    public override string ToString() => Value;

    public static implicit operator string(Email email) => email.Value;
}
