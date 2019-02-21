using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using MassTransit;
using System;
using System.Threading;

namespace ServiceBusWindsorTest
{
  class Program
  {
    static void Main(string[] args)
    {
      IWindsorContainer container = new WindsorContainer();
      container.Register(Classes.FromThisAssembly().BasedOn<IConsumer>());
      container.Install(FromAssembly.This());
      var bus = container.Resolve<IBus>();
      int counter = 1;
      while (true)
      {
        Console.WriteLine($"Sending dummy message no: {counter}");
        bus.Publish(new DummyMessage() {counter = counter++ }).Wait();
        Thread.Sleep(1000);
      }
    }
  }
}
