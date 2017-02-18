using Infrastucture;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Modularity;
using SplashScreen.Events;
using SplashScreen.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SplashScreen
{
    public class SplashScreenModuleInit : IModule
    {
        private readonly IUnityContainer container;
        private readonly IShell shell;
        private readonly IEventAggregator eventAggregator;

        private AutoResetEvent WaitForCreation { get; set; }

        public SplashScreenModuleInit(IUnityContainer container,
                                      IEventAggregator eventAggregator,
                                      IShell shell)
        {
            this.container = container;
            this.eventAggregator = eventAggregator;
            this.shell = shell;
        }


        public void Initialize()
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(
                (Action)(() =>
                {
                    shell.Show();
                    eventAggregator.GetEvent<SplashScreenCloseEvent>().Publish(new SplashScreenCloseEvent());
                })
            );

            WaitForCreation = new AutoResetEvent(false);

            ThreadStart showSplash =
              () =>
              {
                  Dispatcher.CurrentDispatcher.BeginInvoke(
                    (Action)(() =>
                    {
                        container.RegisterType<SplashScreenViewModel, SplashScreenViewModel>();
                        container.RegisterType<SplashScreen.Views.SplashScreen, SplashScreen.Views.SplashScreen>();

                        var splash = container.Resolve<SplashScreen.Views.SplashScreen>();
                        eventAggregator.GetEvent<SplashScreenCloseEvent>().Subscribe(e => splash.Dispatcher.BeginInvoke((Action)splash.Close), ThreadOption.PublisherThread, true);

                        splash.Show();

                        WaitForCreation.Set();
                    }));

                  Dispatcher.Run();
              };

            //TODO : Developer Panel -> Move to constants
            var thread = new Thread(showSplash) { Name = "Splash Thread", IsBackground = true };
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            WaitForCreation.WaitOne();
        }
    }

}
