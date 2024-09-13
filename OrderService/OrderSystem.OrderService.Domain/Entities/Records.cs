using System.Text.Json.Serialization;

namespace OrderSystem.OrderService.Domain.Entities;

public record OrderItem(ItemType PizzaType, int Quantity)
{
    public OrderItem() : this(default, 1)
    {
    }
}

public record Order(
    string OrderId,
    OrderItem[] OrderItems,
    DateTime OrderDate,
    Customer CustomerDto,
    OrderStatus Status = OrderStatus.Received)
{
    public string ShortId => OrderId.Substring(0, 8);
}

public record Customer(string Name, string Email);

public record OrderResultDto(OrderStatus Status, Order OrderDto, string? Message = null);

public record InventoryRequestDto(OrderItem[] PizzasRequested);

public record InventoryResultDto(bool IsSufficientInventory, OrderItem[] PizzasInStock);

public record NotificationDto(string Message, Order OrderDto);

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ItemType
{
    Computer = 1,
    Monitor = 2,
    Keyboard = 3,
    Mouse = 4
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OrderStatus
{
    Received = 0,
    CheckingInventory = 1,
    SufficientInventory = 2,
    InsufficientInventory = 3,
    CheckingPayment = 4,
    PaymentFailing = 5,
    ShipingItems = 6,
    ShipingItemsFailing = 7,
    Error = 8
}