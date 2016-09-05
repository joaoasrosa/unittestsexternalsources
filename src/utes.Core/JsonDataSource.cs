using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace utes.Core
{
    /// <summary>
    /// Class responsible to read from Json.
    /// </summary>
    public class JsonDataSource : IReadDataSource
    {
        /// <summary>
        /// Method to read from the external data source.
        /// </summary>
        /// <param name="methodInfo">The invoked method.</param>
        /// <returns>The collection of objects.</returns>
        public IEnumerable<object[]> Read(MethodInfo methodInfo)
        {
            // Sanity checks
            // TODO: check the initialization of the calss
            //CheckAttributeProperties();

            var returnObjects = new List<object[]>();

            if (null == methodInfo.DeclaringType)
            {
                return returnObjects;
            }

            // Get the arguments names
            var args = methodInfo.GetParameters();

            if (null == args || !args.Any())
            {
                return returnObjects;
            }

            // Get the parameters base on the method signature
            // Json like:
            // { 'Namespace' : { 'class' : { 'methodname' : { 'param1' : [ { object }] , 'param2' : [ { object } ] } } }
            // TODO load from multiple sources...
            // TODO more performant option please...
            var jsonObject = JObject.Parse(File.ReadAllText(@"c:\temp\test.json"));


            // get JSON result objects into a list
            var jsonTokens = jsonObject[methodInfo.DeclaringType.Namespace][methodInfo.DeclaringType.Name][methodInfo.Name];

            for (var i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                var jsonArgTokens = jsonTokens[arg.Name].Children().ToArray();

                for (var j = 0; j < jsonArgTokens.Length; j++)
                {
                    object[] argObjects;

                    if (returnObjects.Count - 1 > j)
                    {
                        argObjects = returnObjects.ElementAt(j);
                    }
                    else
                    {
                        argObjects = new object[args.Length];
                        returnObjects.Add(argObjects);
                    }

                    argObjects[i] = JsonConvert.DeserializeObject(jsonArgTokens[j].ToString(), arg.ParameterType);
                }
            }

            return returnObjects;
        }
    }
}
