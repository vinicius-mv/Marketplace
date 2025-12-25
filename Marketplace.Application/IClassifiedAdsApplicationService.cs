
namespace Marketplace.Application
{
    public interface IClassifiedAdsApplicationService
    {
        Task Handle(object command);
    }
}