using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddOnSdk
{
    public class MyCachedObject
    {
        private static Random generator = new Random();
        private byte[] processedData;
        public MyCachedObject()
        {
            //Allocates nonsense memory
            processedData = new byte[generator.Next(25, 70) * 1024];
        }
        public int Execute(int id)
        {
            return processedData.Length;
        }
    }
}
