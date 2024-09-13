using Dapr.Workflow;
using OrderSystem.OrderService.Domain.Entities;
using OrderSystem.OrderService.Workflow.Repository;

namespace OrderSystem.OrderService.Workflow.Workflows.Activities
{
    public class CreateOrderActivity : WorkflowActivity<Order, OrderResultDto>
    {
        readonly IStateManagementRepository _stateManagement;

        public CreateOrderActivity(IStateManagementRepository stateManagement)
        {
            _stateManagement = stateManagement;
        }

        public override async Task<OrderResultDto> RunAsync(WorkflowActivityContext context, Order order)
        {
            await _stateManagement.SaveOrderAsync(order);

            return new OrderResultDto(OrderStatus.Received, order);
        }
    }
}
