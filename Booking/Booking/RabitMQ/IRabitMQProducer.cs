namespace Booking.RabitMQ
{
    public interface IRabitMQProducer
    {
        public void SendProductMessage(string message);
    }
}
