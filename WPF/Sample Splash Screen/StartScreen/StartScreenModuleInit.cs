using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using StartScreen.ViewModels;
using StartScreen.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartScreen
{
    public class StartScreenModuleInit : IModule
    {
        private readonly IUnityContainer container;
        private readonly IRegionManager regionManager;

        public StartScreenModuleInit(IUnityContainer container, IRegionManager regionManager)
        {
            this.container = container;
            this.regionManager = regionManager;
        }

        public void Initialize()
        {

            // Register an existing object as a named registration with the default
            // container-controlled lifetime.
            this.container.RegisterInstance<StartScreenView>(StartScreenViewModel.SingletonInstanceName, this.container.Resolve<StartScreenView>());


            //Register into region
            //this.regionManager.RegisterViewWithRegion(RegionNames.MainTabControlRegion, () => this.container.Resolve<StartScreenView>(StartScreenViewModel.SingletonInstanceName));



        }
    }

}
