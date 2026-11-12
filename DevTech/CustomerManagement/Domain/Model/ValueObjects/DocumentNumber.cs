namespace DevTech.CustomerManagement.Domain.Model.ValueObjects;

public readonly record struct DocumentNumber
{
    public DocumentType Type { get; }
    public string Value { get; }

    public DocumentNumber(DocumentType type, string value)
    {
        if (!Enum.IsDefined(typeof(DocumentType), type))
            throw new ArgumentException(
                "Invalid document type.", nameof(type));

        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(
                "Document number is required.", nameof(value));

        Type = type;
        Value = value.Trim();
    }

    public DocumentNumber(string value)
        : this(DocumentType.DNI, value)
    {
    }
}