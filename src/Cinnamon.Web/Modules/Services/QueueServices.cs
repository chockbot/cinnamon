using RabbitMQ.Client;
using System.Text;

namespace Cinnamon.Web.Modules.Services
{
    public class QueueServices
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        public QueueServices()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" , UserName ="user", Password="password", VirtualHost ="my_vhost"};
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            // Initialize a default queue if needed
            _channel.QueueDeclare(queue: "cinnamon-queue",
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);
        }
        public void EnqueueUser(string handler, string userId)
        {
            string activeQueueName = $"{handler}-active-queue";
            string reservedQueueName = $"{handler}-reserved-queue";

            //Create queue based on the event handler add checking if existing or not
            // Ensure queues exist
            EnsureQueueExists(activeQueueName);
            EnsureQueueExists(reservedQueueName);

            if (GetQueueLength(activeQueueName) >= 2)
            {
                // Move to reserved queue if active queue is full
                var body = Encoding.UTF8.GetBytes(userId);
                _channel.BasicPublish(exchange: "",
                                     routingKey: reservedQueueName,
                                     basicProperties: null,
                                     body: body);
            }
            else
            {
                StartUserTimer(userId, activeQueueName);
                var body = Encoding.UTF8.GetBytes(userId);
                _channel.BasicPublish(exchange: "",
                                     routingKey: activeQueueName,
                                     basicProperties: null,
                                     body: body);
            }
        }

        private void EnsureQueueExists(string queueName)
        {
            // Declare queue with default parameters
            _channel.QueueDeclare(queue: queueName,
                                  durable: true,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
        }


        public string DequeueUser(string queueName)
        {
            var result = _channel.BasicGet(queueName, true);
            if (result != null)
            {
                var userId = Encoding.UTF8.GetString(result.Body.ToArray());
                return userId;
            }
            return null;
        }

        public int GetQueueLength(string queueName)
        {
            var result = _channel.QueueDeclarePassive(queueName);
            return (int)result.MessageCount;
        }

        private void StartUserTimer(string userId, string activeQueueName)
        {
            Timer timer = new Timer((state) =>
            {
                RemoveUserFromActiveQueue(userId, activeQueueName);
            }, null, TimeSpan.FromMinutes(1), Timeout.InfiniteTimeSpan);
        }

        private void RemoveUserFromActiveQueue(string userId, string activeQueueName)
        {
            // Dequeue from the active queue
            DequeueUser(activeQueueName);
            // Move next user from reserved queue to active queue
            string reservedQueueName = $"{activeQueueName.Replace("-active-queue", "-reserved-queue")}";
            // Check if there are users in the reserved queue
            if (GetQueueLength(reservedQueueName) > 0)
            {
                var nextUser = DequeueUser(reservedQueueName);
                if (nextUser != null)
                {
                    EnqueueUser(activeQueueName, nextUser);
                }
            }
        }

        public (int numberAhead, TimeSpan estimatedWaitingTime, DateTime lastStatusUpdate) GetReservedQueueUserInfo(string reservedQueueName, string activeQueueName, string userId)
        {
            var queueLength = GetQueueLength(reservedQueueName);
            var numberAhead = GetPositionInQueue(reservedQueueName, userId);

            TimeSpan estimatedWaitingTime;
            if (numberAhead >= 0)
            {
                var usersAheadInActiveQueue = Math.Max(0, GetQueueLength(activeQueueName) - 2);
                var totalUsersAhead = usersAheadInActiveQueue + numberAhead;
                estimatedWaitingTime = TimeSpan.FromMinutes(totalUsersAhead * 10);
            }
            else
            {
                estimatedWaitingTime = TimeSpan.Zero;
            }

            var lastStatusUpdate = DateTime.UtcNow;

            return (numberAhead, estimatedWaitingTime, lastStatusUpdate);
        }

        private int GetPositionInQueue(string queueName, string userId)
        {
            // Since RabbitMQ does not support direct querying of the position in a queue,
            // you may need to maintain a separate list or database to track the position.
            // This is a placeholder to illustrate the need for such a mechanism.

            // Placeholder logic: always return -1 (unknown position)
            return -1;
        }
    }
}
