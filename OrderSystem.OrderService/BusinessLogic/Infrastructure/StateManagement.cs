using System.Text.Json;
using Dapr.Client;
using Workflow.Shared.Dtos;

namespace OrderSystem.OrderService.BusinessLogic.Infrastructure;

public class StateManagement
{
    private static readonly string StoreName = "kvstore";
    private readonly DaprClient _client;

    public StateManagement(DaprClient client)
    {
        _client = client;
    }

    public async Task<List<OrderItemDto>> GetItemsAsync(ItemTypeDto[] itemTypes)
    {
        var bulkStateItems = await _client.GetBulkStateAsync(
            StoreName,
            itemTypes.Select(p => FormatKey(nameof(ItemTypeDto), p.ToString())).ToList().AsReadOnly(),
            1);


        var items = new List<OrderItemDto>();
        foreach (var item in bulkStateItems)
            if (!string.IsNullOrEmpty(item.Value))
            {
                var orderItem = JsonSerializer.Deserialize<OrderItemDto>(
                    item.Value,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (orderItem == null) throw new Exception($"Item {item.Key} not found in inventory!");
                items.Add(orderItem);
            }
            else
            {
                items.Add(new OrderItemDto(Enum.Parse<ItemTypeDto>(GetItemTypeFromKey(item.Key)), 0));
            }

        return items;
    }

    public async Task SaveItemsAsync(IEnumerable<OrderItemDto> items)
    {
        var saveStateItems = new List<SaveStateItem<OrderItemDto>>();

        foreach (var item in items)
        {
            var stateKey = FormatKey(nameof(ItemTypeDto), item.PizzaType.ToString());
            Console.WriteLine($"Saving state with key: {stateKey}");
            await _client.SaveStateAsync(
                StoreName,
                stateKey,
                item);
        }

        ;
    }

    public async Task SaveOrderAsync(OrderDto orderDto)
    {
        var stateKey = FormatKey(nameof(OrderDto), orderDto.OrderId);
        Console.WriteLine($"Saving orderDto {orderDto.OrderId} with status {orderDto.Status} and key {stateKey}.");
        await _client.SaveStateAsync(
            StoreName,
            stateKey,
            orderDto);
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