using AutomationHouseholdDatabase.Models;

namespace HouseholdAutomationLogic.BLL
{
    public class ClientsBLL : IBLL<Client>
    {
        private readonly IRedactor<Client> _clientsRedactor;
        private readonly IRedactor<Order> _ordersRedactor;

        public ClientsBLL(IRedactor<Client> clientsRedactor, IRedactor<Order> ordersRedactor)
        {
            _clientsRedactor = clientsRedactor;
            _ordersRedactor = ordersRedactor;
        }

        public IRedactor<Client> Redactor => _clientsRedactor;

        public async Task<Order> AddOrderAsync(Client client, Order order, CancellationToken cancellationToken = default)
        {
            if (!_ordersRedactor.GetByPredicate(c => c.ClientId == client.ClientId).Any())
            {
                if (!Redactor.GetByPredicate(c => c.ClientId == client.ClientId).Any())
                {
                    client = await Redactor.CreateAndSaveAsync(client, cancellationToken);
                }
                order.ClientId = client.ClientId;
                return await _ordersRedactor.CreateAndSaveAsync(order, cancellationToken);
            }
            else
            {
                throw new ArgumentException($"Order is already in database.", nameof(order));
            }
        }
    }
}
