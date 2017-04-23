using AddOnSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddOn1_Plugin
{
    public class WorkerProcess1 : PluginExecuter, IWorkerProcess
    {
        private static readonly Dictionary<int, MyCachedObject> myCache = new Dictionary<int, MyCachedObject>();
        public string ProcessRequest(int id)
        {
            if (!myCache.ContainsKey(id))
            {
                myCache.Add(id, new MyCachedObject());
            }
            int cacheSize = myCache[id].Execute(id);
            return $"This is a response message from id {id} with a cache size of {cacheSize} in the AppDomain named: {AppDomain.CurrentDomain.FriendlyName}";
            //return "ok";
        }
    }
}
