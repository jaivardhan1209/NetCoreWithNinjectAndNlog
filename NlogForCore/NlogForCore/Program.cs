using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ninject;
using Ninject.Modules;
using NLog.Extensions.Logging;
using System;
using System.Reflection;

namespace NlogForCore
{
    class Program
    {
        static void Main(string[] args)
        {
            var servicesProvider = ServiceLocator.GetServiceProvider();
            //var runner = servicesProvider.GetRequiredService<Runner>();
            //var lover = servicesProvider.GetRequiredService<Lover>();
            var walker = servicesProvider.GetRequiredService<KernelScoped>();
            var walker1 = servicesProvider.GetRequiredService<KernelScoped>();

            //Console.WriteLine(walker.Kernel.GetHashCode());
            //Console.WriteLine(walker1.Kernel.GetHashCode());
            //runner.DoAction("Action1");
            walker.Kernel.Get<Walker>().DoAction();
         //   walker1.DoAction();
            //lover.DoAction();

            Console.WriteLine("Press ANY key to exit");
            Console.ReadLine();

            // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
            NLog.LogManager.Shutdown();
        }
    }

    public class ServiceLocator
    {
        private static bool isBootStrap;

        private static IServiceProvider serviceProvider;
        public static IServiceProvider BuildDi()
        {
            var services = new ServiceCollection();

            //Runner is the custom class You can register as below or do dependency Injection using Ninject
           // services.AddTransient<Runner>();
           
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            services.AddLogging((builder) => builder/*.AddFilter("Microsoft", LogLevel.Warning)
                                                    .AddFilter("System", LogLevel.Warning)
                                                    .AddFilter("NToastNotify", LogLevel.Warning)*/
                                                    .SetMinimumLevel(LogLevel.Trace));

            // Add Ninject Dependency 
            services.AddSingleton<KernelScoped>();

            var serviceProvider = services.BuildServiceProvider();

            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            //configure NLog
            loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
            NLog.LogManager.LoadConfiguration("nlog.config");

            // configure other logger

            return serviceProvider;
        }

        public static IServiceProvider GetServiceProvider()
        {
            if (!isBootStrap)
            {
                serviceProvider = BuildDi();
                isBootStrap = true;
            }

            return serviceProvider;
        }
    }


    public class NinjectBindings : NinjectModule
    {
        public override void Load()
        {
            this.Bind<Runner>().ToSelf().InSingletonScope();
            this.Bind<Walker>().ToSelf().InSingletonScope();
            this.Bind<Lover>().ToSelf().InSingletonScope();
        }
    }

    public sealed class KernelScoped
    {
        /// <summary>
        /// Ninject IKernel object
        /// </summary>
        public IKernel Kernel { get; }
        /// <summary>
        /// Kernel public constructor
        /// </summary>
        public KernelScoped()
        {
            Kernel = new StandardKernel(new NinjectSettings { AllowNullInjection = true });
            Kernel.Load(Assembly.GetExecutingAssembly());
        }
    }
}