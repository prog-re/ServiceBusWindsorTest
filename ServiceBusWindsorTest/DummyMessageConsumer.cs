using MassTransit;
using System;
using System.Threading.Tasks;

namespace ServiceBusWindsorTest
{

  public class DummyMessageConsumer : IConsumer<DummyMessage>
  {

     public Task Consume(ConsumeContext<DummyMessage> context)
    {
      return Task.Run(() =>
      {
        var msg = context.Message;
        Console.WriteLine($"Consuming message no: {msg.counter}");
      });
    }
  }
}
