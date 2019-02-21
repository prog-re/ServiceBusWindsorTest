using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using GreenPipes;
using MassTransit;
using MassTransit.AzureServiceBusTransport;
using System;
using System.Configuration;

namespace ServiceBusWindsorTest
{
  public class ServiceBusInstaller : IWindsorInstaller
  {
    public void Install(IWindsorContainer container, IConfigurationStore store)
    {
      string bustype = "azuresb";
      string connectionstring = null;
      string queue = "DummyMessageQueue";

      connectionstring = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
      container.Register(Component.For<IBusControl, IBus > ()
        .UsingFactoryMethod(() => CreateBusControlForAzureServicebus(container, bustype, connectionstring, queue))
        .OnDestroy(x => x.Stop())
        .LifestyleSingleton());
    }


    private static IBusControl CreateBusControlForAzureServicebus(IWindsorContainer container, string bustype, string connectionstring, string queue)
    {
      IBusControl busControl;

      busControl = Bus.Factory.CreateUsingAzureServiceBus(cfg =>
      {
        var host = cfg.Host(connectionstring, h =>
        {
          h.OperationTimeout = TimeSpan.FromSeconds(5);
          h.RetryMinBackoff = TimeSpan.FromMilliseconds(100);
          h.RetryMaxBackoff = TimeSpan.FromMilliseconds(1000);
        });

        cfg.ReceiveEndpoint(queue, endpoint =>
        {
          endpoint.Consumer<DummyMessageConsumer>(container.Kernel);
        });
      });



      busControl.Start();


      return busControl;
    }
  }
}
