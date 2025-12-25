using Marketplace.Domain;
using Marketplace.Framework;
using static Marketplace.Application.Contracts.ClassifiedAds;

namespace Marketplace.Application;

public class ClassifiedAdsApplicationService : IClassifiedAdsApplicationService, IApplicationService
{
    private readonly IClassifiedAdRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrencyLookup _currencyLookup;

    public ClassifiedAdsApplicationService(
        IClassifiedAdRepository repository,
        IUnitOfWork unitOfWork,
        ICurrencyLookup currencyLookup)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currencyLookup = currencyLookup;
    }

    public Task Handle(object command)
    {
        return command switch
        {
            V1.Create cmd => HandleCreate(cmd),

            V1.SetTitle cmd =>
                HandleUpdate(
                    classifiedAdId: cmd.Id,
                    operation: c => c.SetTitle(ClassifiedAdTitle.FromString(cmd.Title))
                ),

            V1.UpdateText cmd =>
                HandleUpdate(
                    classifiedAdId: cmd.Id,
                    operation: c => c.UpdateText(ClassifiedAdText.FromString(cmd.Text))
                ),

            V1.UpdatePrice cmd =>
                HandleUpdate(
                    classifiedAdId: cmd.Id,
                    operation: c => c.UpdatePrice(Price.FromDecimal(cmd.Price, cmd.Currency, _currencyLookup))
                ),

            V1.RequestToPublish cmd => HandleUpdate(
                cmd.Id,
                c => c.RequestToPublish()
            ),

            _ => throw new InvalidOperationException($"Unknown command type: {command.GetType().FullName}"),
        };
    }

    private async Task HandleCreate(V1.Create cmd)
    {
        if (await _repository.Exists(cmd.Id.ToString()))
            throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");

        var classifiedAd = new ClassifiedAd(
            new ClassifiedAdId(cmd.Id),
            new UserId(cmd.OwnerId)
        );
        await _repository.Add(classifiedAd);
        await _unitOfWork.Commit();
    }

    private async Task HandleUpdate(Guid classifiedAdId, Action<ClassifiedAd> operation)
    {
        var classifiedAd = await _repository.Load(classifiedAdId.ToString());
        if (classifiedAd == null)
            throw new InvalidOperationException($"Entity with id {classifiedAdId} does not exist");

        operation(classifiedAd);

        await _unitOfWork.Commit();

    }
}
