using AutomationHouseholdDatabase.Models;

namespace HouseholdAutomationLogic.BLL
{
    public class ProviderBLL : IBLL<Provider>
    {
        private readonly IDbEntityRedactor<Provider> _providerRedactor;
        private readonly IDbEntityRedactor<Resource> _resourceRedactor;
        private readonly IDbEntityRedactor<ProviderToResource> _providerToResourceRedactor;

        public ProviderBLL(IDbEntityRedactor<Provider> providerRedactor, IDbEntityRedactor<Resource> resourceRedactor, IDbEntityRedactor<ProviderToResource> providerToResourceRedactor)
        {
            _providerRedactor = providerRedactor;
            _resourceRedactor = resourceRedactor;
            _providerToResourceRedactor = providerToResourceRedactor;
        }

        public IDbEntityRedactor<Provider> Redactor => _providerRedactor;

        public async Task<ProviderToResource> AddResourceToProvider(Provider provider, Resource resource, int cost, CancellationToken cancellationToken = default)
        {
            ThrowIfCostLessThanZero(cost);
            if (!_providerRedactor.GetByPredicate(p => p.ProviderId == provider.ProviderId).Any())
            {
                provider = await _providerRedactor.CreateAndSaveAsync(provider, cancellationToken);
            }
            if (!_providerRedactor.GetByPredicate(r => r.ProviderId == resource.ResourceId).Any())
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
