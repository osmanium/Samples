using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AddOnSdk
{
    public class PluginExecuter : MarshalByRefObject
    {
        public void ExecutePluginsInAssembly(string assemblyName)
        {
            //Loads along with references, base path should be defined in AppDomain, this path will be used for unresolved references
            Assembly pluginAssembly = Assembly.Load(assemblyName);


            var type = typeof(IWorkerProcess);
            var types = pluginAssembly.GetTypes()
                                      .Where(p => type.IsAssignableFrom(p))
                                      .ToList();
            
            types.ForEach(x =>
            {
                try
                {
                    var worker = (IWorkerProcess)Activator.CreateInstance(x);
                    for (int index = 0; index < 10000; index++)
                    {
                        int id = index % 5000;
                        Console.WriteLine(worker.ProcessRequest(id));
                    }
                }
                catch (Exception ex)
                {
                    Debugger.Break();
                }
            });
        }
    }
}
