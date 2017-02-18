using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SplashScreen.Behaviours
{
    public class SplashScreenBehaviour
    {
        public static readonly DependencyProperty EnabledProperty = DependencyProperty.RegisterAttached(
          "Enabled", typeof(bool), typeof(SplashScreenBehaviour), new PropertyMetadata(OnEnabledChanged));

        public static bool GetEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnabledProperty);
        }
        public static void SetEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(EnabledProperty, value);
        }

        private static void OnEnabledChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var splash = obj as Window;
            if (splash != null && args.NewValue is bool && (bool)args.NewValue)
            {
                splash.Closed += (s, e) =>
                {
                    splash.DataContext = null;
                    splash.Dispatcher.InvokeShutdown();
                };
                splash.MouseLeftButtonDown += (s, e) => splash.DragMove();
            }
        }
    }
}
