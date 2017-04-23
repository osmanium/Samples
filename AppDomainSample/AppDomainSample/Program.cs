using AddOnSdk;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace AppDomainSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Mail started in : " + AppDomain.CurrentDomain.FriendlyName);

            foreach (string pluginDirectory in Directory.GetDirectories("Plugins"))
            {
                //Gets only the plugin entry assembly
                foreach (var assemblyFile in Directory.GetFiles(pluginDirectory, "*_Plugin.dll"))
                {
                    try
                    {
                        var pluginExecuterType = typeof(PluginExecuter);
                        
                        Evidence adevidence = AppDomain.CurrentDomain.Evidence;
                        AppDomainSetup ads = new AppDomainSetup();
                        ads.ApplicationBase = Environment.CurrentDirectory;
                        ads.PrivateBinPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), pluginDirectory);

                        //ApplicationBase   = For common assemblies and loaded only once, shares the same assembly along with sub app domains;
                        //PrivateBinPath    = Only plugin specific assemblies

                        AppDomain ad = AppDomain.CreateDomain(Path.GetFileNameWithoutExtension(assemblyFile), adevidence, ads);

                        //Execute Plugin
                        var pluginExecuter = (PluginExecuter)ad.CreateInstanceAndUnwrap((typeof(PluginExecuter)).Assembly.FullName, pluginExecuterType.FullName);
                        pluginExecuter.ExecutePluginsInAssembly(Path.GetFileNameWithoutExtension(assemblyFile));
                        

                        //Check loaded assemblies in plugin AppDomain,                              ad.GetAssemblies();
                        //Check main AppDomain assemblies, should not contain plugin assemblies,    AppDomain.CurrentDomain.GetAssemblies();
                        AppDomain.Unload(ad);

#if DEBUG
                        //This should not be done in production environment, only to see the effect immediately in development environment
                        //Check the application memory usage in TaskManager, should be high
                        Debugger.Break();
                        GC.Collect();
                        //Check the application memory usage in TaskManager, should be cleared out
#endif
                    }
                    catch (Exception ex)
                    {
                        Debugger.Break();
                    }
                }
            }

            Console.WriteLine("Excution completed..");
            Console.ReadKey();
        }
    }
}
