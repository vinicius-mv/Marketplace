using Marketplace.Domain;
using Marketplace.Framework;
using static Marketplace.Application.Contracts.ClassifiedAds;

namespace Marketplace.Application;

public class ClassifiedAdsApplicationService : IApplicationService
{
    private readonly IEntityStore _store;
    private readonly ICurrencyLookup _currencyLookup;

    public ClassifiedAdsApplicationService(IEntityStore store, ICurrencyLookup currencyLookup)
    {
        _store = store;
        _currencyLookup = currencyLookup;
    }

    public Task Handle(object command)
    {
        return command switch
        {
            V1.Create cmd => HandleCreate(cmd),
            V1.SetTitle cmd => HandleSetTitle(cmd),
            V1.UpdateText cmd => HandleUpdateText(cmd),
            V1.UpdatePrice cmd => HandleUpdatePrice(cmd),
            V1.RequestToPublish cmd => HandleRequestToPublish(cmd),
            _ => throw new InvalidOperationException($"Unknown command type: {command.GetType().FullName}"),
        };
    }

    private async Task HandleCreate(V1.Create cmd)
    {
        if (await _store.Exists<ClassifiedAd>(cmd.Id.ToString()))
            throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");

        var classifiedAd = new ClassifiedAd(
            new ClassifiedAdId(cmd.Id),
            new UserId(cmd.OwnerId)
        );
        await _store.Save(classifiedAd);
    }

    private async Task HandleSetTitle(V1.SetTitle cmd)
    {
        var classifiedAd = await _store.Load<ClassifiedAd>(cmd.Id.ToString());
        if (classifiedAd == null)
            throw new InvalidOperationException($"Entity with id {cmd.Id} does not exist");

        classifiedAd.SetTitle(
            ClassifiedAdTitle.FromString(cmd.Title)
        );
        await _store.Save(classifiedAd);
    }

    private async Task HandleUpdateText(V1.UpdateText cmd)
    {
        var classifiedAd = await _store.Load<ClassifiedAd>(cmd.Id.ToString());
        if (classifiedAd == null)
            throw new InvalidOperationException($"Entity with id {cmd.Id} does not exist");

        classifiedAd.UpdateText(
            ClassifiedAdText.FromString(cmd.Text)
        );
        await _store.Save(classifiedAd);
    }

    private async Task HandleUpdatePrice(V1.UpdatePrice cmd)
    {
        var classifiedAd = await _store.Load<ClassifiedAd>(cmd.Id.ToString());
        if (classifiedAd == null)
            throw new InvalidOperationException($"Entity with id {cmd.Id} does not exist");

        classifiedAd.UpdatePrice(
            Price.FromDecimal(cmd.Price, cmd.Currency, _currencyLookup)
        );
        await _store.Save(classifiedAd);
    }

    private async Task HandleRequestToPublish(V1.RequestToPublish cmd)
    {
        var classifiedAd = await _store.Load<ClassifiedAd>(cmd.Id.ToString());
        if (classifiedAd == null)
            throw new InvalidOperationException($"Entity with id {cmd.Id} does not exist");

        classifiedAd.RequestToPublish();
        await _store.Save(classifiedAd);
    }
}
