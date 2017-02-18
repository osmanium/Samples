using Prism.Events;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using System.Windows;
using Prism.Modularity;
using System.Threading;
using SplashScreen.Events;
using SplashScreen;
using StartScreen;
using Infrastucture;
using SampleWPFApp.Views;

namespace SampleWPFApp
{
    public class SampleBootstrapper : UnityBootstrapper
    {
        private IEventAggregator EventAggregator
        {
            get { return Container.Resolve<IEventAggregator>(); }
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterType<IShell, ShellView>(new ContainerControlledLifetimeManager());
        }

        protected override DependencyObject CreateShell()
        {
            var view = this.Container.TryResolve<IShell>();
            return view as DependencyObject; ;
        }

        protected override void InitializeModules()
        {

            SplashScreenModule();

            StartScreenModule(true);

           
        }

        private void SplashScreenModule()
        {
            IModule splashModule = Container.Resolve<SplashScreenModuleInit>();
            splashModule.Initialize();
            Thread.Sleep(1000);
        }
        
        private void StartScreenModule(bool isSplash)
        {
            if (isSplash)
                EventAggregator.GetEvent<SplashScreenUpdateEvent>().Publish(new SplashScreenUpdateEvent { Text = "Loading Components..." });
            IModule startScreenModule = Container.Resolve<StartScreenModuleInit>();
            startScreenModule.Initialize();
            Thread.Sleep(1000);
        }
    }
}
