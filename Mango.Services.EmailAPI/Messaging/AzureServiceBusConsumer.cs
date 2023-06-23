using Azure.Messaging.ServiceBus;
using Mango.Services.EmailAPI.Models;
using Mango.Services.EmailAPI.Services;
using Newtonsoft.Json;
using System.Text;

namespace Mango.Services.EmailAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string emailCartQueue;
        private readonly string regEmailQueue;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        private ServiceBusProcessor _processor;
        private ServiceBusProcessor _regUserProcessor;
        public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
        {

            _configuration = configuration;
            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBusConnectionString");
            emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCart");
            regEmailQueue = _configuration.GetValue<string>("EmailCartRequest:RegUserQueue");

            var client = new ServiceBusClient(serviceBusConnectionString);

            //listen to queue
            _processor = client.CreateProcessor(emailCartQueue);
            _regUserProcessor = client.CreateProcessor(regEmailQueue);
            _emailService = emailService;
            

        }

        public async Task Start()
        {
            _processor.ProcessMessageAsync += OnEmailCartReceived;
            _processor.ProcessErrorAsync += ErrorHandler;
            await _processor.StartProcessingAsync();

            _regUserProcessor.ProcessMessageAsync += OnUserRegReceived;
            _regUserProcessor.ProcessErrorAsync += ErrorHandler;
            await _regUserProcessor.StartProcessingAsync();
        }

        

        private Task ErrorHandler(ProcessErrorEventArgs arg)
        {   
            Console.WriteLine(arg.Exception.ToString());
            return Task.CompletedTask;
        }

        private async Task OnEmailCartReceived(ProcessMessageEventArgs arg)
        {
            var message = arg.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            CartDto objmessage = JsonConvert.DeserializeObject<CartDto>(body);
            try
            {
                //try to log email
               await _emailService.SendEmail(objmessage);
                await arg.CompleteMessageAsync(arg.Message);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private async Task OnUserRegReceived(ProcessMessageEventArgs arg)
        {
            var message = arg.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            string objmessage = JsonConvert.DeserializeObject<string>(body);
            try
            {
                //try to log email
                await _emailService.RegisterUSerEmailLog(objmessage);
                await arg.CompleteMessageAsync(arg.Message);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public async Task Stop()
        {
            await _processor.StopProcessingAsync();
            await _processor.DisposeAsync();

            await _regUserProcessor.StopProcessingAsync();
            await _regUserProcessor.DisposeAsync();
        }
    }
}
