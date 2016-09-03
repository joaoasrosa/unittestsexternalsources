using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace utes.Core.Xml
{
    /// <summary>
    /// Class responsible to read from XML.
    /// </summary>
    /// <typeparam name="T">The object type.</typeparam>
    public class ReadXml<T> : IRead<T>
    {
        public IEnumerable<T> Read()
        {
            throw new NotImplementedException();
        }
    }
}
