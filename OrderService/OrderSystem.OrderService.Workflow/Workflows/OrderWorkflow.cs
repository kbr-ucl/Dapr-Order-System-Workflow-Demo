using Dapr.Workflow;
using OrderSystem.OrderService.Domain.Entities;
using OrderSystem.OrderService.Workflow.IntegrationEvents;
using OrderSystem.OrderService.Workflow.Workflows.Activities;

namespace OrderSystem.OrderService.Workflow.Workflows;

public class OrderWorkflow : Workflow<Order, OrderResultDto>
{
    public override async Task<OrderResultDto> RunAsync(WorkflowContext context, Order order)
    {
        var newOrder = order with { Status = OrderStatus.Received };

        await context.CallActivityAsync(
            nameof(NotifyActivity),
            new NotificationDto($"Received order {order.ShortId} from {order.CustomerDto.Name}.", newOrder));

        await context.CallActivityAsync(
            nameof(CreateOrderActivity),
            newOrder);


        var orderPaymentSuccess = await context.WaitForExternalEventAsync<bool>(nameof(PaymentProcessedEvent));

        if (orderPaymentSuccess)
        {
        }
        else
        {
            newOrder = newOrder with { Status = OrderStatus.Error };
        }

        return new OrderResultDto(newOrder.Status, newOrder);
    }
}