using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace utes.Core
{
    /// <summary>
    /// Class responsible to read from Json.
    /// </summary>
    public class JsonDataSource : IReadDataSource
    {
        private FileStoreType _fileStoreType;
        private readonly string _fileLocation;
        private readonly string _resourceName;
        private readonly Type _resourceType;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="fileLocation">The file location.</param>
        /// <param name="resourceName">The resource name.</param>
        /// <param name="resourceType">The resource type.</param>
        public JsonDataSource(string fileLocation, string resourceName, Type resourceType)
        {
            this._fileLocation = fileLocation;
            this._resourceName = resourceName;
            this._resourceType = resourceType;
        }

        /// <summary>
        /// Method to read from the external data source.
        /// </summary>
        /// <param name="methodInfo">The invoked method.</param>
        /// <returns>The collection of objects.</returns>
        public IEnumerable<object[]> Read(MethodInfo methodInfo)
        {
            // Sanity checks
            CheckAttributeProperties();

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
            var jsonObject = ReadJsonObject();

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

        /// <summary>
        /// Check the attribute properties needed to run the test.
        /// </summary>
        private void CheckAttributeProperties()
        {
            // Check if all properties are null. Performance.
            if (string.IsNullOrWhiteSpace(this._fileLocation)
                && string.IsNullOrWhiteSpace(this._resourceName) && null == this._resourceType)
            {
                throw new AllPropertiesNullOrEmptyException("All properties are null or empty.");
            }

            // Check the file location.
            if (!string.IsNullOrWhiteSpace(this._fileLocation) && !File.Exists(this._fileLocation))
            {
                throw new FileNotExistsException("File does not exists.");
            }

            // Check the file resource.
            if (string.IsNullOrWhiteSpace(this._fileLocation)
                && (string.IsNullOrWhiteSpace(this._resourceName) || null == this._resourceType))
            {
                throw new ResourceNotExistsException("Resource does not exists.");
            }

            // Check the storage type.
            CheckStorageType();
        }

        /// <summary>
        /// Check the storage type base on the properties.
        /// </summary>
        private void CheckStorageType()
        {
            this._fileStoreType = string.IsNullOrEmpty(this._fileLocation) ? FileStoreType.Resource : FileStoreType.FileSystem;
        }

        /// <summary>
        /// Method to read the XML file from the storage.
        /// </summary>
        /// <returns>Returns the XmlDocument.</returns>
        private JObject ReadJsonObject()
        {
            switch (this._fileStoreType)
            {
                case FileStoreType.FileSystem:
                    using (var textReader = File.OpenText(this._fileLocation))
                    {
                        using (var jsonReader = new JsonTextReader(textReader))
                        {
                            return JObject.Load(jsonReader);
                        }
                    }
                case FileStoreType.Resource:
                    using (var stringReader = new StringReader(Resource.GetResourceLookup<string>(this._resourceType, this._resourceName)))
                    {
                        using (var jsonReader = new JsonTextReader(stringReader))
                        {
                            return JObject.Load(jsonReader);
                        }
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
