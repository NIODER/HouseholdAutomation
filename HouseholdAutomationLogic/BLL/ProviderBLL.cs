using AutomationHouseholdDatabase.Models;

namespace HouseholdAutomationLogic.BLL
{
    public class ProviderBLL : IBLL<Provider>
    {
        private readonly IRedactor<Provider> _providerRedactor;
        private readonly IRedactor<Resource> _resourceRedactor;
        private readonly IRedactor<ProviderToResource> _providerToResourceRedactor;

        public ProviderBLL(IRedactor<Provider> providerRedactor, IRedactor<Resource> resourceRedactor, IRedactor<ProviderToResource> providerToResourceRedactor)
        {
            _providerRedactor = providerRedactor;
            _resourceRedactor = resourceRedactor;
            _providerToResourceRedactor = providerToResourceRedactor;
        }

        public IRedactor<Provider> Redactor => _providerRedactor;

        public async Task<ProviderToResource> AddResourceToProvider(Provider provider, Resource resource, int cost, CancellationToken cancellationToken = default)
        {
            ThrowIfCostLessThanZero(cost);
            if (!_providerRedactor.GetByPredicate(p => p.ProviderId == provider.ProviderId).Any())
            {
                provider = await _providerRedactor.CreateAndSaveAsync(provider, cancellationToken);
            }
            if (!_resourceRedactor.GetByPredicate(r => r.ResourceId == resource.ResourceId).Any())
            {
                resource = await _resourceRedactor.CreateAndSaveAsync(resource, cancellationToken);
            }
            return await _providerToResourceRedactor.CreateAndSaveAsync(new()
            {
                ProviderId = provider.ProviderId,
                ResourceId = resource.ResourceId,
                Cost = cost
            }, cancellationToken);
        }

        public async Task RemoveResourceFromProvider(Provider provider, Resource resource, CancellationToken cancellationToken = default)
        {
            _providerToResourceRedactor
                .GetByPredicate(providerToResource => providerToResource.ProviderId == provider.ProviderId && providerToResource.ResourceId == resource.ResourceId)
                .ToList()
                .ForEach(_providerToResourceRedactor.Delete);
            await _providerToResourceRedactor.SaveChangesAsync(cancellationToken);
        }

        private static void ThrowIfCostLessThanZero(int cost)
        {
            if (cost < 0)
            {
                throw new ArgumentException("Cost cannot be less than zero.", nameof(cost));
            }
        }
    }
}
