using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Cinnamon.Web.Pages.ReservedSeats;
using System.Text;

namespace Cinnamon.Web.Modules.Services
{
    public class QueueServices
    {
        private readonly IConfiguration _configuration;
        private readonly QueueServiceClient _queueServiceClient;
        private static readonly TimeSpan ProcessingTime = TimeSpan.FromMinutes(2);

        public QueueServices(IConfiguration configuration)
        {
            _configuration = configuration;
            var connectionString = _configuration["AppConfig:Authentication:AzureQueue:ConnectionString"];
            _queueServiceClient = new QueueServiceClient(connectionString);
        }

        public async Task EnqueueUserAsync(string handler, string userId)
        {
            string activeQueueName = $"{handler}-active-queue";
            string reservedQueueName = $"{handler}-reserved-queue";

            // Ensure queues exist
            await EnsureQueueExistsAsync(activeQueueName);
            await EnsureQueueExistsAsync(reservedQueueName);
            var activeQueueLength = await GetQueueLengthAsync(activeQueueName);
            if (activeQueueLength >= 2)
            {
                // Move to reserved queue if active queue is full
                var queueClient = _queueServiceClient.GetQueueClient(reservedQueueName);
                await queueClient.SendMessageAsync(Convert.ToBase64String(Encoding.UTF8.GetBytes(userId)));
            }
            else
            {
                await StartUserTimerAsync(userId, activeQueueName);
                var queueClient = _queueServiceClient.GetQueueClient(activeQueueName);
                await queueClient.SendMessageAsync(Convert.ToBase64String(Encoding.UTF8.GetBytes(userId)));
            }
        }
        public async Task<bool> IsUserInQueueAsync(string queueName, string userId)
        {
            var queueClient = _queueServiceClient.GetQueueClient(queueName);
            var messages = await queueClient.PeekMessagesAsync(maxMessages: 32); // Adjust maxMessages as needed

            foreach (var message in messages.Value)
            {
                if (Encoding.UTF8.GetString(Convert.FromBase64String(message.MessageText)) == userId)
                {
                    return true; // User is already in the queue
                }
            }

            return false; // User is not in the queue
        }
        private async Task EnsureQueueExistsAsync(string queueName)
        {
            var queueClient = _queueServiceClient.GetQueueClient(queueName);
            try
            {
                var properties = await queueClient.GetPropertiesAsync();
                // Queue exists
            }
            catch (RequestFailedException ex) when (ex.ErrorCode == "QueueNotFound")
            {
                // Queue does not exist, create it
                await queueClient.CreateAsync();
            }
        }

        public async Task<string> DequeueUserAsync(string queueName)
        {
            var queueClient = _queueServiceClient.GetQueueClient(queueName);
            QueueMessage[] retrievedMessage = await queueClient.ReceiveMessagesAsync(1);

            if (retrievedMessage != null && retrievedMessage.Length > 0)
            {
                string userId = Encoding.UTF8.GetString(Convert.FromBase64String(retrievedMessage[0].MessageText));
                await queueClient.DeleteMessageAsync(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);
                return userId;
            }
            return null;
        }

        public async Task<int> GetQueueLengthAsync(string queueName)
        {
            var queueClient = _queueServiceClient.GetQueueClient(queueName);
            var properties = await queueClient.GetPropertiesAsync();
            return properties.Value.ApproximateMessagesCount;
        }

        private async Task StartUserTimerAsync(string userId, string activeQueueName)
        {
            Timer timer = new Timer(async (state) =>
            {
                await RemoveUserFromActiveQueueAsync(userId, activeQueueName);
            }, null, TimeSpan.FromMinutes(2), Timeout.InfiniteTimeSpan);
        }

        private async Task RemoveUserFromActiveQueueAsync(string userId, string activeQueueName)
        {
            // Dequeue from the active queue
            await DequeueUserAsync(activeQueueName);

            // Move next user from reserved queue to active queue
            string reservedQueueName = $"{activeQueueName.Replace("-active-queue", "-reserved-queue")}";
            string activeQueueHandler = reservedQueueName.Replace("-reserved-queue", "");

            // Check if there are users in the reserved queue
            if (await GetQueueLengthAsync(reservedQueueName) > 0)
            {
                var nextUser = await DequeueUserAsync(reservedQueueName);
                if (nextUser != null)
                {
                    await EnqueueUserAsync(activeQueueHandler, nextUser);
                }
            }
        }
        public async Task<(DateTime insertionTime, TimeSpan remainingTime)?> GetUserInActiveQueueAsync(string queueName, string userId)
        {
            var queueClient = _queueServiceClient.GetQueueClient(queueName);
            var messages = await queueClient.PeekMessagesAsync(maxMessages: 32); // Adjust maxMessages as needed

            foreach (var message in messages.Value)
            {
                if (Encoding.UTF8.GetString(Convert.FromBase64String(message.MessageText)) == userId)
                {
                    var insertionTime = message.InsertedOn.GetValueOrDefault().LocalDateTime;
                    var processingTimeElapsed = DateTime.Now - insertionTime;
                    // Remove milliseconds
                    processingTimeElapsed = TimeSpan.FromSeconds(Math.Floor(processingTimeElapsed.TotalSeconds));
                    TimeSpan remainingTime = ProcessingTime - processingTimeElapsed;

                    return (insertionTime, remainingTime);
                }
            }

            return null; // User not found in the active queue
        }

        public async Task<ReservedQueuingInfo> GetReservedQueueUserInfoAsync(string reservedQueueName, string activeQueueName, string userId)
        {
            //Get Active user estimated time
            TimeSpan processingTimeElapsed;
            TimeSpan estimatedWaitingTime;
            DateTime expectedTimeOfArrival;

            var queueLength = await GetQueueLengthAsync(activeQueueName);
            var message = await GetPositionInQueueAsync(reservedQueueName, userId);

            if (message.numberAhead >= 0)
            {
                var totalUsersAhead    = message.numberAhead;
                var firstUserRemaining = await GetFirstActiveUserInsertionTimeAsync(activeQueueName);
                processingTimeElapsed  = firstUserRemaining.Value + TimeSpan.FromMinutes(2) - DateTime.Now;
                processingTimeElapsed  = TimeSpan.FromSeconds(Math.Floor(processingTimeElapsed.TotalSeconds));

                estimatedWaitingTime   = TimeSpan.FromMinutes(totalUsersAhead * 2) + processingTimeElapsed;
                expectedTimeOfArrival  = DateTime.Now.Add(estimatedWaitingTime);
            }
            else
            {
                estimatedWaitingTime = TimeSpan.Zero;
                expectedTimeOfArrival = DateTime.Now;
            }

            var lastStatusUpdate = DateTime.Now;

            return new ReservedQueuingInfo
            {
                QueueId               = message.QueueId,
                numberAhead           = message.numberAhead,
                estimatedWaitingTime  = estimatedWaitingTime,
                lastStatusUpdate      = lastStatusUpdate,
                expectedTimeOfArrival = expectedTimeOfArrival
            };
        }
        public async Task<DateTime?> GetFirstActiveUserInsertionTimeAsync(string activeQueueName)
        {
            var queueClient = _queueServiceClient.GetQueueClient(activeQueueName);

            // Peek at the first message in the queue
            var response = await queueClient.PeekMessagesAsync(1);

            if (response.Value == null || response.Value.Length == 0)
            {
                return null;
            }

            // Extract the message and its insertion time
            var firstMessage = response.Value[0];
            var insertionTime = firstMessage.InsertedOn.GetValueOrDefault().LocalDateTime;

            return insertionTime;
        }

        private async Task<ReservedQueuingInfo> GetPositionInQueueAsync(string queueName, string userId)
        {
            int position = -1;
            string queueId = string.Empty;
            var queueClient = _queueServiceClient.GetQueueClient(queueName);

            var messages = await queueClient.PeekMessagesAsync(maxMessages: 32); // Adjust maxMessages as needed

            for (int i = 0; i < messages.Value.Length; i++)
            {
                if (Encoding.UTF8.GetString(Convert.FromBase64String(messages.Value[i].MessageText)) == userId)
                {
                    position = i;
                    queueId = messages.Value[i].MessageId;
                    break;
                }
            }
            return new ReservedQueuingInfo
            {
                numberAhead = position,
                QueueId = queueId,
            };
        }
        public class ReservedQueuingInfo
        {
            public string QueueId { get; set; }
            public int numberAhead { get; set; }
            public TimeSpan estimatedWaitingTime { get; set; }
            public DateTime lastStatusUpdate { get; set; }
            public DateTime expectedTimeOfArrival { get; set; }
        }
    }
}
