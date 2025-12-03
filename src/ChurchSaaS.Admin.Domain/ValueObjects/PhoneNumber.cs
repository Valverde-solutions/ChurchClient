using System.Text.RegularExpressions;
using ChurchSaaS.Admin.Domain.Abstractions;

namespace ChurchSaaS.Admin.Domain.ValueObjects;

public sealed class PhoneNumber : ValueObject
{
    public string Value { get; }

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static PhoneNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Phone number is required.", nameof(value));

        // Normaliza: remove espaços, hífens e parênteses
        var digits = Regex.Replace(value, @"\s+|[-()]", "");

        if (digits.Length < 8)
            throw new ArgumentException("Phone number is too short.", nameof(value));

        return new PhoneNumber(digits);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;

    public static implicit operator string(PhoneNumber phone) => phone.Value;
}
