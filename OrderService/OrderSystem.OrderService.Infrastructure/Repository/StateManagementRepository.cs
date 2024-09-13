using System.Text.Json;
using Dapr.Client;
using OrderSystem.OrderService.Domain.Entities;
using OrderSystem.OrderService.Workflow.Repository;

namespace OrderSystem.OrderService.Infrastructure.Repository;

public class StateManagementRepository : IStateManagementRepository
{
    private static readonly string StoreName = "kvstore";
    private readonly DaprClient _client;

    public StateManagementRepository(DaprClient client)
    {
        _client = client;
    }

    async Task<List<OrderItem>> IStateManagementRepository.GetItemsAsync(ItemType[] itemTypes)
    {
        var bulkStateItems = await _client.GetBulkStateAsync(
            StoreName,
            itemTypes.Select(p => FormatKey(nameof(ItemType), p.ToString())).ToList().AsReadOnly(),
            1);


        var items = new List<OrderItem>();
        foreach (var item in bulkStateItems)
            if (!string.IsNullOrEmpty(item.Value))
            {
                var orderItem = JsonSerializer.Deserialize<OrderItem>(
                    item.Value,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (orderItem == null) throw new Exception($"Item {item.Key} not found in inventory!");
                items.Add(orderItem);
            }
            else
            {
                items.Add(new OrderItem(Enum.Parse<ItemType>(GetItemTypeFromKey(item.Key)), 0));
            }

        return items;
    }

    async Task IStateManagementRepository.SaveItemsAsync(IEnumerable<OrderItem> items)
    {
        var saveStateItems = new List<SaveStateItem<OrderItem>>();

        foreach (var item in items)
        {
            var stateKey = FormatKey(nameof(ItemType), item.PizzaType.ToString());
            Console.WriteLine($"Saving state with key: {stateKey}");
            await _client.SaveStateAsync(
                StoreName,
                stateKey,
                item);
        }

        ;
    }

    async Task IStateManagementRepository.SaveOrderAsync(Order order)
    {
        var stateKey = FormatKey(nameof(Order), order.OrderId);
        Console.WriteLine($"Saving orderDto {order.OrderId} with status {order.Status} and key {stateKey}.");
        await _client.SaveStateAsync(
            StoreName,
            stateKey,
            order);
    }

    private static string FormatKey(string typeName, string key)
    {
        return $"{typeName}-{key}";
    }

    private static string GetItemTypeFromKey(string key)
    {
        return key.Split("-")[1];
    }
}