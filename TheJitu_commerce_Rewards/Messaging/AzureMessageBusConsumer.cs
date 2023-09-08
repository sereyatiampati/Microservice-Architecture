using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Text;
using TheJitu_commerce_Rewards.Models.Dto;
using TheJitu_commerce_Rewards.Services;

namespace TheJitu_commerce_Rewards.Messaging
{
    public class AzureMessageBusConsumer:IAzureMessaageBusConsumer
    {
        private readonly IConfiguration _configuration;
        private readonly string Connectionstring;
        private readonly string QueueName;
        private readonly ServiceBusProcessor _rewardsProcessor;
        private readonly RewardsService _rewardService;
        public AzureMessageBusConsumer(IConfiguration configuration, RewardsService rewardService)
        {

            _configuration = configuration;
            Connectionstring= _configuration.GetSection("ServiceBus:ConnectionString").Get<string>();

            var topic= _configuration.GetSection("AzureServices:TopiCName").Get<string>();
            var subscription= _configuration.GetSection("AzureServices:Subscription").Get<string>();

            var serviceBusClient = new ServiceBusClient(Connectionstring);

            _rewardsProcessor = serviceBusClient.CreateProcessor(topic, subscription);
            _rewardService = rewardService;

        }

        public async Task Start()
        {
            //start Processing
            _rewardsProcessor.ProcessMessageAsync += OnPlaceOrder;
            _rewardsProcessor.ProcessErrorAsync += ErrorHandler;
            await _rewardsProcessor.StartProcessingAsync();

    
        }
        public async Task Stop()
        {
            //Stop Processing
            await _rewardsProcessor.StopProcessingAsync();
            await _rewardsProcessor.DisposeAsync();
        }

        private async Task OnPlaceOrder(ProcessMessageEventArgs arg)
        {
            var message = arg.Message;

            var body = Encoding.UTF8.GetString(message.Body);
            var rewards = JsonConvert.DeserializeObject<RewardsDto>(body);
            try
            {
               await  _rewardService.AddRewards(rewards);
               await arg.CompleteMessageAsync(message);

            }catch(Exception ex) { 
            }

        }

       

      
        private Task ErrorHandler(ProcessErrorEventArgs arg)
        {   

            //[Todo] send an email to Admin

           throw new NotImplementedException();
        }

       

       
    }
}
