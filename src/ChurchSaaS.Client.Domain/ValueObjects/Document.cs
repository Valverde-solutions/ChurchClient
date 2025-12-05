using System.Text.RegularExpressions;
using ChurchSaaS.Admin.Domain.Abstractions;

namespace ChurchSaaS.Client.Domain.ValueObjects;

public sealed class Document : ValueObject
{
    public string Value { get; }
    public DocumentType Type { get; }

    private Document(string value, DocumentType type)
    {
        Value = value;
        Type = type;
    }

    public static Document Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Document is required.", nameof(value));

        // Mantém apenas dígitos
        var digits = Regex.Replace(value, @"\D", "");

        if (digits.Length < 5)
            throw new ArgumentException("Document is too short.", nameof(value));

        var type = digits.Length switch
        {
            11 => IsValidCpf(digits) ? DocumentType.Cpf : throw new ArgumentException("Invalid CPF.", nameof(value)),
            14 => IsValidCnpj(digits) ? DocumentType.Cnpj : throw new ArgumentException("Invalid CNPJ.", nameof(value)),
            _  => DocumentType.Other
        };

        return new Document(digits, type);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
        yield return Type;
    }

    public override string ToString() => Value;

    public static implicit operator string(Document document) => document.Value;

    private static bool IsValidCpf(string digits)
    {
        if (digits.Length != 11) return false;
        if (digits.Distinct().Count() == 1) return false;

        int CalculateDigit(int[] numbers, int factorStart)
        {
            var sum = 0;
            var factor = factorStart;
            for (var i = 0; i < numbers.Length; i++)
            {
                sum += numbers[i] * factor--;
            }
            var remainder = sum % 11;
            return remainder < 2 ? 0 : 11 - remainder;
        }

        var numArray = digits.Select(c => c - '0').ToArray();

        var firstDigit = CalculateDigit(numArray[..9], 10);
        if (numArray[9] != firstDigit) return false;

        var secondDigit = CalculateDigit(numArray[..10], 11);
        return numArray[10] == secondDigit;
    }

    private static bool IsValidCnpj(string digits)
    {
        if (digits.Length != 14) return false;
        if (digits.Distinct().Count() == 1) return false;

        int CalculateDigit(int[] numbers, int[] factors)
        {
            var sum = 0;
            for (var i = 0; i < numbers.Length; i++)
            {
                sum += numbers[i] * factors[i];
            }
            var remainder = sum % 11;
            return remainder < 2 ? 0 : 11 - remainder;
        }

        var numArray = digits.Select(c => c - '0').ToArray();

        var firstFactors = new[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        var firstDigit = CalculateDigit(numArray[..12], firstFactors);
        if (numArray[12] != firstDigit) return false;

        var secondFactors = new[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        var secondDigit = CalculateDigit(numArray[..13], secondFactors);
        return numArray[13] == secondDigit;
    }
}

public enum DocumentType
{
    Unknown = 0,
    Cpf = 1,
    Cnpj = 2,
    Other = 99
}
