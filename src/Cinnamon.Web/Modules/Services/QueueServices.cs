using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cinnamon.Web.Modules.Services
{
    public class QueueServices
    {
        private readonly IConfiguration _configuration;
        private readonly QueueServiceClient _queueServiceClient;
        private static readonly TimeSpan ProcessingTime = TimeSpan.FromMinutes(1);

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

            if (await GetQueueLengthAsync(activeQueueName) >= 2)
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

        private async Task EnsureQueueExistsAsync(string queueName)
        {
            var queueClient = _queueServiceClient.GetQueueClient(queueName);
            await queueClient.CreateIfNotExistsAsync();
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
            await Task.Delay(ProcessingTime);
            await RemoveUserFromActiveQueueAsync(userId, activeQueueName);
        }

        private async Task RemoveUserFromActiveQueueAsync(string userId, string activeQueueName)
        {
            // Dequeue from the active queue
            await DequeueUserAsync(activeQueueName);

            // Move next user from reserved queue to active queue
            string reservedQueueName = $"{activeQueueName.Replace("-active-queue", "-reserved-queue")}";

            // Check if there are users in the reserved queue
            if (await GetQueueLengthAsync(reservedQueueName) > 0)
            {
                var nextUser = await DequeueUserAsync(reservedQueueName);
                if (nextUser != null)
                {
                    await EnqueueUserAsync(activeQueueName, nextUser);
                }
            }
        }

        public async Task<(int numberAhead, TimeSpan estimatedWaitingTime, DateTime lastStatusUpdate)> GetReservedQueueUserInfoAsync(string reservedQueueName, string activeQueueName, string userId)
        {
            var queueLength = await GetQueueLengthAsync(reservedQueueName);
            var numberAhead = await GetPositionInQueueAsync(reservedQueueName, userId);

            TimeSpan estimatedWaitingTime;
            if (numberAhead >= 0)
            {
                var usersAheadInActiveQueue = Math.Max(0, await GetQueueLengthAsync(activeQueueName) - 2);
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

        private async Task<int> GetPositionInQueueAsync(string queueName, string userId)
        {
            int position = -1;
            var queueClient = _queueServiceClient.GetQueueClient(queueName);

            var messages = await queueClient.PeekMessagesAsync(maxMessages: 100); // Adjust maxMessages as needed

            for (int i = 0; i < messages.Value.Length; i++)
            {
                if (Encoding.UTF8.GetString(Convert.FromBase64String(messages.Value[i].MessageText)) == userId)
                {
                    position = i;
                    break;
                }
            }

            return position;
        }
    }
}
