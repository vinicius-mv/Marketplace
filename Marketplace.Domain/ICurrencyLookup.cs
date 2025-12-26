namespace Marketplace.Domain;

public interface ICurrencyLookup
{
    public Currency FindCurrency(string currencyCode);
}