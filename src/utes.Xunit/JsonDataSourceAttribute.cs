using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit.Sdk;

namespace utes.Xunit
{
    /// <summary>
    /// Class to handle the parameters from a JSON data source.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class JsonDataSourceAttribute : DataAttribute
    {
        /// <summary>
        /// Returns the data to be used to test the theory.
        /// </summary>
        /// <param name="testMethod">The method that is being tested</param>
        /// <returns>One or more sets of theory data. Each invocation of the test method
        /// is represented by a single object array.</returns>
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var jsonDataSource = new Core.JsonDataSource(this.FileLocation, this.ResourceName, this.ResourceType);
            return jsonDataSource.Read(testMethod);
        }

        /// <summary>
        /// The file location.
        /// </summary>
        public string FileLocation { get; set; }

        /// <summary>
        /// The resource type.
        /// </summary>
        public Type ResourceType { get; set; }

        /// <summary>
        /// The resource name.
        /// </summary>
        public string ResourceName { get; set; }
    }
}
