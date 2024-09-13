﻿using Dapr.Workflow;
using OrderSystem.OrderService.BusinessLogic.Infrastructure;
using Workflow.Shared.Dtos;

namespace OrderSystem.OrderService.Workflows.Activities
{
    public class CreateOrderActivity : WorkflowActivity<OrderDto, OrderResultDto>
    {
        readonly StateManagement _stateManagement;

        public CreateOrderActivity(StateManagement stateManagement)
        {
            _stateManagement = stateManagement;
        }

        public override async Task<OrderResultDto> RunAsync(WorkflowActivityContext context, OrderDto orderDto)
        {
            await _stateManagement.SaveOrderAsync(orderDto);

            return new OrderResultDto(OrderStatusDto.Received, orderDto);
        }
    }
}
