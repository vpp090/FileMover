using Autofac;
using ServiceLogic.Services;
using System;
using Topshelf;
using ServiceLogic.Contracts;
using System.ServiceProcess;
using Autofac.Core;
using System.Security.Principal;

namespace FileMover
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var container = BuildContainer();

            HostFactory.Run(config =>
            {
                config.SetServiceName("TestService");
                config.StartAutomatically();

                config.Service<WinService>(serviceConfig =>
                {
                    var movingService = container.Resolve<IMovingService>();
                    serviceConfig.ConstructUsing(() => new WinService(movingService));

                    serviceConfig.WhenStarted((service, hostControl) =>
                    {
                        service.Start(hostControl);
                        return true;
                    });

                    serviceConfig.WhenStopped((service, hostControl) =>
                    {
                        service.Stop(hostControl);
                        return true;
                    });

                });
            });

        }

        static private IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<MovingService>().As<IMovingService>();

            return builder.Build();
        }
    }
}
