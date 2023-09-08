namespace TheJitu_commerce_Rewards.Messaging
{
    public interface IAzureMessaageBusConsumer
    {
        Task Start();


        Task Stop();
    }
}
