using Marketplace.Domain;
using Raven.Client.Documents.Session;

namespace Marketplace.Infrastructure;

public class ClassifiedAdRepository : IClassifiedAdRepository
{
    private readonly IAsyncDocumentSession _session;

    public ClassifiedAdRepository(IAsyncDocumentSession session)
    {
        _session = session;
    }

    public async Task Add(ClassifiedAd entity)
    {
        await _session.StoreAsync(entity, EntityId(entity.Id));
    }

    public async Task<bool> Exists(ClassifiedAdId id)
    {
        return await _session.Advanced.ExistsAsync(EntityId(id));
    }

    public async Task<ClassifiedAd> Load(ClassifiedAdId id)
    {
        return await _session.LoadAsync<ClassifiedAd>(EntityId(id));
    }

    private static string EntityId(ClassifiedAdId id) => $"ClassifiedAd/{id.Value}";
}