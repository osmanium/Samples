using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SplashScreen.Events
{
    public class SplashScreenUpdateEvent : PubSubEvent<SplashScreenUpdateEvent>
    {
        public string Text { get; set; }
    }
}
