using Prism.Events;
using SplashScreen.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SplashScreen.ViewModels
{
    public class SplashScreenViewModel : INotifyPropertyChanged
    {
        string _status = string.Empty;

        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                NotifyPropertyChanged("Status");
            }
        }



        UIElement _transitionTextBox = null;

        public UIElement TransitionTextBox
        {
            get { return _transitionTextBox; }
            set
            {
                _transitionTextBox = value;
                NotifyPropertyChanged("TransitionTextBox");
            }
        }


        public SplashScreenViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<SplashScreenUpdateEvent>().Subscribe(e => UpdateMessage(e.Text));
        }


        private void UpdateMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            Status = message;
        }


        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
