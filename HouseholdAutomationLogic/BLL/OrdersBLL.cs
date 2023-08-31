using AutomationHouseholdDatabase.Models;

namespace HouseholdAutomationLogic.BLL
{
    public class OrdersBLL : IBLL<Order>
    {
        private readonly IRedactor<Order> _ordersRedactor;
        private readonly IRedactor<OrdersToResource> _orderToResourceRedactor;
        private readonly IRedactor<Resource> _resourceRedactor;

        public OrdersBLL(IRedactor<Order> ordersRedactor, IRedactor<OrdersToResource> orderToResourceRedactor, IRedactor<Resource> resourceRedactor)
        {
            _ordersRedactor = ordersRedactor;
            _orderToResourceRedactor = orderToResourceRedactor;
            _resourceRedactor = resourceRedactor;
        }

        public IRedactor<Order> Redactor => _ordersRedactor;

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
