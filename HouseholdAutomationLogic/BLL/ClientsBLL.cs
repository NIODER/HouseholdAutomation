using AutomationHouseholdDatabase.Data;
using AutomationHouseholdDatabase.Models;
using Microsoft.EntityFrameworkCore;

namespace HouseholdAutomationLogic.BLL
{
    public class ClientsBLL : IBLL<Client>
    {
        private readonly IDbEntityRedactor<Client> _clientsRedactor;
        private readonly IDbEntityRedactor<Order> _ordersRedactor;

        public ClientsBLL(IDbEntityRedactor<Client> clientsRedactor, IDbEntityRedactor<Order> ordersRedactor)
        {
            _clientsRedactor = clientsRedactor;
            _ordersRedactor = ordersRedactor;
        }

        public IDbEntityRedactor<Client> Redactor => _clientsRedactor;

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
