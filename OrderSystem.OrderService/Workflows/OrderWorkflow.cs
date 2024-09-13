using Dapr.Workflow;
using OrderSystem.OrderService.Workflows.Activities;
using Workflow.Shared.Dtos;
using Workflow.Shared.IntegrationEvents;

namespace OrderSystem.OrderService.Workflows;

public class OrderWorkflow : Workflow<OrderDto, OrderResultDto>
{
    public override async Task<OrderResultDto> RunAsync(WorkflowContext context, OrderDto order)
    {
        var newOrder = order with { Status = OrderStatusDto.Received };

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
            newOrder = newOrder with { Status = OrderStatusDto.Error };
        }

        return new OrderResultDto(newOrder.Status, newOrder);
    }
}