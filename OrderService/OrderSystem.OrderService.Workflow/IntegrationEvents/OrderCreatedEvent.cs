namespace OrderSystem.OrderService.Workflow.IntegrationEvents
{
    public abstract record IntegrationEvent
    {
        public Guid CorrelationId { get; init; } = Guid.NewGuid();
    }

    public record OrderCreatedEvent : IntegrationEvent
    {
        
    }

    public record PaymentProcessedEvent : IntegrationEvent
    {
    }

    public record ItemsReservedEvent : IntegrationEvent
    {
    }

    public record ItemsShippedEvent : IntegrationEvent
    {
    }

    public record OrderCompletedEvent : IntegrationEvent
    {
    }


    public abstract record FailedEvent : IntegrationEvent
    {
        public string Reason { get; init; } = string.Empty;
    }

    public record OrderFailedEvent : FailedEvent
    {
    }

    public record PaymentFailedEvent : FailedEvent
    {
    }

    public record ItemsReservationFailedEvent : FailedEvent
    {
    }

    public record ItemsShipmentFailedEvent : FailedEvent
    {
    }


}
