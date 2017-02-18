using Infrastucture;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartScreen.ViewModels
{
    public class StartScreenViewModel : ValidatableBindableBase
    {
        private readonly IUnityContainer container;
        private readonly IEventAggregator eventAggregator;
        

        public static string SingletonInstanceName { get { return Constants.StartScreenViewModel_SingletonInstanceName; } }
        
        
        public StartScreenViewModel(IUnityContainer container, IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            this.container = container;
           
        }

        

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

    }

}
