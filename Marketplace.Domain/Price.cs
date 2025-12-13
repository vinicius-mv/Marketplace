namespace Marketplace.Domain;

public class Price : Money
{
    public Price(decimal amount, string currencyCode, ICurrencyLookup currencyLookup) : base(amount, currencyCode, currencyLookup)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Price cannot be negative");
    }

    public new static Price FromDecimal(decimal amount, string currency,
        ICurrencyLookup currencyLookup) =>
        new Price(amount, currency, currencyLookup);
}
