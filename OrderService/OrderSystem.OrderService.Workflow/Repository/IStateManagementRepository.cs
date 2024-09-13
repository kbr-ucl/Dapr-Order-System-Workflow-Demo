using OrderSystem.OrderService.Domain.Entities;

namespace OrderSystem.OrderService.Workflow.Repository;

public interface IStateManagementRepository
{
    Task<List<OrderItem>> GetItemsAsync(ItemType[] itemTypes);
    Task SaveItemsAsync(IEnumerable<OrderItem> items);
    Task SaveOrderAsync(Order order);
}