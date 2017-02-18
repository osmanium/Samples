using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SampleWPFApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        SampleBootstrapper bootstrapper = null;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            bootstrapper = new SampleBootstrapper();
            bootstrapper.Run();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            GC.Collect();

            base.OnExit(e);
        }
    }
}
