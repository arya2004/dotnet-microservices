using Azure.Messaging.ServiceBus;
using Mango.Services.RewardsAPI.Message;
using Mango.Services.RewardsAPI.Messaging;
using Mango.Services.RewardsAPI.Models;
using Mango.Services.RewardsAPI.Services;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.RewardsAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string orderCreatedTopic;
        private readonly string orderCreatedRewardSubs;
        private readonly IConfiguration _configuration;
        private readonly RewardService _rewardService;

        private ServiceBusProcessor _rewardProcessor;
        public AzureServiceBusConsumer(IConfiguration configuration, RewardService rewardService)
        {

            _configuration = configuration;
            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");

            orderCreatedTopic = _configuration.GetValue<string>("TopicAndQueueName:OrderCreatedTopic");
            orderCreatedRewardSubs = _configuration.GetValue<string>("TopicAndQueueName:orderCreatedReward_Subscription");

            var client = new ServiceBusClient(serviceBusConnectionString);

            //listen to queue
            _rewardProcessor = client.CreateProcessor(orderCreatedTopic, orderCreatedRewardSubs);
           _rewardService = rewardService;
            

        }

        public async Task Start()
        {
            _rewardProcessor.ProcessMessageAsync += OnNewOrderRewardRequestReceived;
            _rewardProcessor.ProcessErrorAsync += ErrorHandler;
            await _rewardProcessor.StartProcessingAsync();

           
        }

        

        private Task ErrorHandler(ProcessErrorEventArgs arg)
        {   
            Console.WriteLine(arg.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task OnNewOrderRewardRequestReceived(ProcessMessageEventArgs arg)
        {
            var message = arg.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            RewardMessage objmessage = JsonConvert.DeserializeObject<RewardMessage>(body);
            try
            {
                //try to log email
               await _rewardService.UpdateRewards(objmessage);
                await arg.CompleteMessageAsync(arg.Message);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        
        public async Task Stop()
        {
            await _rewardProcessor.StopProcessingAsync();
            await _rewardProcessor.DisposeAsync();

        }
    }
}
