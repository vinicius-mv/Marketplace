using Marketplace.Domain;

namespace Marketplace.Tests;

public class ClassifiedAd_Publish_Spec
{
    private readonly ClassifiedAd _classifiedAd;

    public ClassifiedAd_Publish_Spec()
    {
        _classifiedAd = new ClassifiedAd(
            new ClassifiedAdId(Guid.NewGuid()),
            new UserId(Guid.NewGuid()));
    }

    [Fact]
    public void Can_publish_a_valid_ad()
    {
        _classifiedAd.SetTitle(ClassifiedAdTitle.FromString("Great car for sale"));
        _classifiedAd.UpdateText(ClassifiedAdText.FromString("This is a well-maintained car with low mileage."));
        _classifiedAd.UpdatePrice(Price.FromDecimal(100.10m, "EUR", new FakeCurrencyLookup()));

        _classifiedAd.RequestToPublish();

        Assert.Equal(ClassifiedAd.ClassifiedAdState.PendingReview, _classifiedAd.State);

    }

    [Fact]
    public void Cannot_publish_without_title()
    {
        _classifiedAd.UpdateText(ClassifiedAdText.FromString("This is a well-maintained car with low mileage."));
        _classifiedAd.UpdatePrice(Price.FromDecimal(200.10m, "EUR", new FakeCurrencyLookup()));

        Assert.Throws<InvalidEntityStateException>(() => _classifiedAd.RequestToPublish());
    }

    [Fact]
    public void Cannot_publish_without_text()
    {
        _classifiedAd.SetTitle(ClassifiedAdTitle.FromString("Great car for sale"));
        _classifiedAd.UpdatePrice(Price.FromDecimal(100.10m, "EUR", new FakeCurrencyLookup()));

        Assert.Throws<InvalidEntityStateException>(() => _classifiedAd.RequestToPublish());
    }

    [Fact]
    public void Cannot_publish_without_price()
    {
        _classifiedAd.SetTitle(ClassifiedAdTitle.FromString("Great car for sale"));
        _classifiedAd.UpdateText(ClassifiedAdText.FromString("This is a well-maintained car with low mileage."));

        Assert.Throws<InvalidEntityStateException>(() => _classifiedAd.RequestToPublish());
    }

    [Fact]
    public void Cannot_publish_with_zero_price()
    {
        _classifiedAd.SetTitle(ClassifiedAdTitle.FromString("Great car for sale"));
        _classifiedAd.UpdateText(ClassifiedAdText.FromString("This is a well-maintained car with low mileage."));
        _classifiedAd.UpdatePrice(Price.FromDecimal(0.0m, "EUR", new FakeCurrencyLookup()));

        Assert.Throws<InvalidEntityStateException>(() => _classifiedAd.RequestToPublish());
    }
}
