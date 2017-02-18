using Infrastucture;
using Microsoft.Practices.Unity;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleWPFApp.ViewModels
{
    public class ShellViewModel : ValidatableBindableBase
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IUnityContainer container;
    
    }
}
