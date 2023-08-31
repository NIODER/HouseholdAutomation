using AutomationHouseholdDatabase.Models;

namespace HouseholdAutomationLogic.BLL
{
    public class ResourceBLL : IBLL<Resource>
    {
        private readonly IRedactor<Resource> _resourceRedactor;
        private readonly IRedactor<Provider> _providerRedactor;
        private readonly IRedactor<ProviderToResource> _providerToResourceRedactor;

        public IRedactor<Resource> Redactor => _resourceRedactor;

        public ResourceBLL(IRedactor<Resource> resourceRedactor, IRedactor<Provider> providerRedactor, IRedactor<ProviderToResource> providerToResourceRedactor)
        {
            _resourceRedactor = resourceRedactor;
            _providerRedactor = providerRedactor;
            _providerToResourceRedactor = providerToResourceRedactor;
        }

        public ProviderToResource GetCheapestProvider(Resource resource)
        {
            return _providerToResourceRedactor.GetByPredicate(p => p.ResourceId == resource.ResourceId)
                .MinBy(providerToResource => providerToResource.Cost) ?? throw new NullReferenceException("No providers found for this resource.");
        }

        public async Task<ProviderToResource> UpdateProviderToResourceAsync(ProviderToResource providerToResource)
        {
            ThrowIfCostLessThanZero(providerToResource.Cost);
            return await _providerToResourceRedactor.UpdateAndSaveAsync(providerToResource);
        }

        public async Task<ProviderToResource> AddProviderAsync(Resource resource, Provider provider, int cost, CancellationToken cancellationToken = default)
        {
            ThrowIfCostLessThanZero(cost);
            if (!_resourceRedactor.GetByPredicate(r => r.ResourceId == resource.ResourceId).Any())
            {
                resource = await _resourceRedactor.CreateAndSaveAsync(resource, cancellationToken);
            }
            if (!_providerRedactor.GetByPredicate(p => p.ProviderId == provider.ProviderId).Any())
            {
                provider = await _providerRedactor.CreateAndSaveAsync(provider, cancellationToken);
            }
            return await _providerToResourceRedactor.CreateAndSaveAsync(new ProviderToResource()
            {
                Provider = provider,
                Resource = resource,
                Cost = cost
            }, cancellationToken);
        }

        public Task RemoveProviderAsync(Resource resource, Provider provider)
        {
            _providerToResourceRedactor
                .GetByPredicate(providerToResource => providerToResource.ProviderId == provider.ProviderId && providerToResource.ResourceId == resource.ResourceId)
                .ToList()
                .ForEach(_providerToResourceRedactor.Delete);
            return _providerToResourceRedactor.SaveChangesAsync();
        }

        public IEnumerable<ProviderToResource> GetResourceProviders(Resource resource)
        {
            return _providerToResourceRedactor.GetByPredicate(p => p.ResourceId == resource.ResourceId);
        }

        private static void ThrowIfCostLessThanZero(int cost)
        {
            if (cost < 0)
            {
                throw new ArgumentException("Resource cannot costs less than zero.", nameof(cost));
            }
        }
    }
}
