﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.AspNetCore.Hosting;
using utes.Core;
using utes.Domain;
using utes.Interfaces;
using Assembly = utes.Domain.Assembly;

namespace utes.WebApplicationAssemblyStorage
{
    /// <summary>
    /// Class to handle the assembly storage within the web application.
    /// </summary>
    public class WebApplicationAssemblyStorage : IAssemblyStorage
    {
        private readonly string _assembliesPath;
        private readonly IEnumerable<IMethodAttribute> _methodAttributes;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="appEnvironment">The hosting environment interface.</param>
        /// <param name="methodAttributes">The known implementations of the IMethodAttribute interface.</param>
        public WebApplicationAssemblyStorage(IHostingEnvironment appEnvironment,
            IEnumerable<IMethodAttribute> methodAttributes)
        {
            // Sanity tests.
            if (null == appEnvironment)
            {
                throw new ArgumentNullException(nameof(appEnvironment));
            }

            if (null == methodAttributes)
            {
                throw new ArgumentNullException(nameof(methodAttributes));
            }

            // If the path doesn't exist, create it
            this._assembliesPath = Path.Combine(appEnvironment.ContentRootPath, "assemblies");
            if (!Directory.Exists(this._assembliesPath))
            {
                Directory.CreateDirectory(this._assembliesPath);
            }

            this._methodAttributes = methodAttributes;
        }

        /// <summary>
        /// Method to get the assemblies.
        /// </summary>
        /// <returns>The assemblies.</returns>
        public IEnumerable<Assembly> GetAssemblies()
        {
            // Get all the assemblies
            var assembliesPath = Directory.EnumerateFiles(this._assembliesPath, "*.dll");

            return (from assemblyPath in assembliesPath
                    let assembly = AssemblyLoadContext.GetAssemblyName(assemblyPath)
                    select new Assembly
                    {
                        Path = assemblyPath,
                        Name = assembly.Name,
                        Version = assembly.Version.ToString()
                    }).ToArray();
        }

        /// <summary>
        /// Method to save an assembly.
        /// </summary>
        /// <param name="assembly">The assembly to save.</param>
        public void SaveAssembly(Assembly assembly)
        {
            // Sanity tests.
            // 1. Is DLL?
            if (!assembly.Name.EndsWith(".dll"))
            {
                throw new BadImageFormatException("Unexpected file extension.");
            }

            // Save in order to load from disk.
            var assemblyPath = Path.Combine(this._assembliesPath, assembly.Name);
            using (var fileStream = File.Create(assemblyPath))
            {
                fileStream.Write(assembly.ContentBytes, 0, assembly.ContentBytes.Length);
            }

            try
            {
                // 2. Can we load it?
                var myAssemblyLoadContext = new MyAssemblyLoadContext();
                var assemblyInMemory = myAssemblyLoadContext.LoadFromAssemblyPath(assemblyPath);

                // 3. Any of the methods are marked with the method attribute?
                if (assemblyInMemory.GetTypes().Any(
                        assemblyType => assemblyType.GetMethods().Any(
                            typeMethod => typeMethod.GetCustomAttributes().Any(
                                customAttribute => this._methodAttributes.Any(
                                    methodAttribute => customAttribute.GetType() == methodAttribute.GetType()
                                )
                            )
                        )
                    )
                )
                {
                    return;
                }

                throw new DataSourceAttributeNotFoundException(
                    "The uploaded assembly do not implement IMethodAttribute interface.");
            }
            catch
            {
                try
                {
                    File.SetAttributes(assemblyPath, FileAttributes.Normal);
                    File.Delete(assemblyPath);
                }
                catch
                {
                    // ignored
                }

                throw;
            }
        }

        /// <summary>
        /// Returns all the classes with methods implementing IMethodAttribute interface in the given assembly.
        /// </summary>
        /// <param name="assemblyName">The assembly name.</param>
        /// <returns>The classes with methods implementing IMethodAttribute interface.</returns>
        public IEnumerable<Class> GetClassesInAssembly(string assemblyName)
        {
            // Get the assembly.
            var assemblyPath = Directory.EnumerateFiles(this._assembliesPath, $"{assemblyName}.dll").Single();

            // Load into memory.
            var myAssemblyLoadContext = new MyAssemblyLoadContext();
            var assemblyInMemory = myAssemblyLoadContext.LoadFromAssemblyPath(assemblyPath);

            // Get the classes implementing the IMethodAttribute interface.
            return (from c in assemblyInMemory.GetTypes()
                    from m in c.GetMethods()
                    from t in m.GetCustomAttributes()
                    from ma in this._methodAttributes
                    where t.GetType() == ma.GetType()
                    select new Class
                    {
                        FullName = c.FullName,
                        Name = c.Name,
                        AssemblyName = assemblyName
                    }).ToArray();
        }

        /// <summary>
        /// Returns all the methods implementing IMethodAttribute in the given class.
        /// </summary>
        /// <param name="class">The class.</param>
        /// <returns>The methods implementing IMethodAttribute interface.</returns>
        public IEnumerable<Method> GetMethodsInClass(Class @class)
        {
            // Get the assembly.
            var assemblyPath = Directory.EnumerateFiles(this._assembliesPath, $"{@class.AssemblyName}.dll").Single();

            // Load into memory.
            var myAssemblyLoadContext = new MyAssemblyLoadContext();
            var assemblyInMemory = myAssemblyLoadContext.LoadFromAssemblyPath(assemblyPath);

            // Get the classes implementing the IMethodAttribute interface.
            return (from c in assemblyInMemory.GetTypes()
                    from m in c.GetMethods()
                    from t in m.GetCustomAttributes()
                    from ma in this._methodAttributes
                    where c.Name.Equals(@class.Name)
                    && t.GetType() == ma.GetType()
                    select new Method
                    {
                        Name = m.Name,
                        ClassName = @class.Name,
                        AssemblyName = @class.AssemblyName
                    }).ToArray();
        }

        /// <summary>
        /// Returns all method parameters in the given method implementing IMethodAttribute.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns>The method parameters.</returns>
        public IEnumerable<MethodParameter> GetMethodParametersInMethod(Method method)
        {
            // Get the assembly.
            var assemblyPath = Directory.EnumerateFiles(this._assembliesPath, $"{method.AssemblyName}.dll").Single();

            // Load into memory.
            var myAssemblyLoadContext = new MyAssemblyLoadContext();
            var assemblyInMemory = myAssemblyLoadContext.LoadFromAssemblyPath(assemblyPath);

            // Get the classes implementing the IMethodAttribute interface.
            return (from c in assemblyInMemory.GetTypes()
                    from m in c.GetMethods()
                    from t in m.GetCustomAttributes()
                    from ma in this._methodAttributes
                    from marg in m.GetParameters()
                    where c.Name.Equals(method.ClassName)
                    && m.Name.Equals(method.Name)
                    && t.GetType() == ma.GetType()
                    select new MethodParameter()
                    {
                        Name = marg.Name,
                        Type = marg.ParameterType.ToString(),
                        AssemblyName = method.AssemblyName,
                        ClassName = method.ClassName,
                        MethodName = method.Name
                    }).ToArray();
        }
    }
}
