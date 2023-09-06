namespace TheJitu_commerce_EmailService.Messaging
{
    public interface IAzureMessaageBusConsumer
    {
        Task Start();


        Task Stop();
    }
}
