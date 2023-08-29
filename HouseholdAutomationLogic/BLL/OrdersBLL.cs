using AutomationHouseholdDatabase.Models;

namespace HouseholdAutomationLogic.BLL
{
    public class OrdersBLL : IBLL<Order>
    {
        private readonly IDbEntityRedactor<Order> _ordersRedactor;
        private readonly IDbEntityRedactor<OrdersToResource> _orderToResourceRedactor;
        private readonly IDbEntityRedactor<Resource> _resourceRedactor;

        public OrdersBLL(IDbEntityRedactor<Order> ordersRedactor, IDbEntityRedactor<OrdersToResource> orderToResourceRedactor, IDbEntityRedactor<Resource> resourceRedactor)
        {
            _ordersRedactor = ordersRedactor;
            _orderToResourceRedactor = orderToResourceRedactor;
            _resourceRedactor = resourceRedactor;
        }

        public IDbEntityRedactor<Order> Redactor => _ordersRedactor;

        public Task RemoveResourceAsync(Order order, Resource resource, CancellationToken cancellationToken = default)
        {
            var ordersToResource = _orderToResourceRedactor
                .GetByPredicate(ordersToResource => ordersToResource.ResourceId == resource.ResourceId && ordersToResource.OrderId == order.OrderId)
                .FirstOrDefault();
            if (ordersToResource == null)
            {
                return Task.CompletedTask;
            }
            return _orderToResourceRedactor.DeleteAndSaveAsync(ordersToResource, cancellationToken);
        }

        public async Task<OrdersToResource> AddResourceAsync(Order order, Resource resource, int count, CancellationToken cancellationToken = default)
        {
            if (resource.ResourceId == default)
            {
                resource = await _resourceRedactor.CreateAndSaveAsync(resource, cancellationToken);
            }
            var orderToResorce = new OrdersToResource()
            {
                OrderId = order.OrderId,
                ResourceId = resource.ResourceId,
                Count = count
            };
            return await _orderToResourceRedactor.CreateAndSaveAsync(orderToResorce, cancellationToken);
        }
    }
}
