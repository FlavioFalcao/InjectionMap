﻿using System;
using System.Linq;
using System.Reflection;
using InjectionMap.Expressions;
using InjectionMap.Internals;
using InjectionMap.Mapping;

namespace InjectionMap
{
    public class InjectionMapper : IDisposable
    {
        IMappableContainer _container;

        /// <summary>
        /// Creates a InjectionMapper that mapps to the common mappingcontainer
        /// </summary>
        public InjectionMapper()
        {
        }

        /// <summary>
        /// Creates a InjectionMapper with a custom container to map to
        /// </summary>
        /// <param name="container">The <see cref="IMappableContainer"/> to add the mappings to</param>
        public InjectionMapper(IMappableContainer container)
        {
            Ensure.ArgumentNotNull(container, "container");

            _container = container;
        }

        #region Static Implementation

        /// <summary>
        /// Initializes all mappings in the <see cref="IInjectionMapping"/>
        /// </summary>
        /// <param name="mapper"></param>
        public static void Initialize(IInjectionMapping mapper)
        {
            mapper.InitializeMap(MappingContainerManager.MappingContainer);
        }

        /// <summary>
        /// Initializes all implementations of <see cref="IInjectionMapping"/>
        /// </summary>
        /// <param name="assembly"></param>
        public static void Initialize(Assembly assembly)
        {
            var type = typeof(IInjectionMapping);
            var types = assembly.GetTypes().Where(p => type.IsAssignableFrom(p) && !p.IsInterface);

            foreach (var t in types)
            {
                var obj = Activator.CreateInstance(t) as IInjectionMapping;
                if (obj == null)
                    continue;

                obj.InitializeMap(MappingContainerManager.MappingContainer);
            }
        }

        #endregion

        #region Implementation

        /// <summary>
        /// Creates a Mapping to TSvc
        /// </summary>
        /// <typeparam name="TSvc">The type to map</typeparam>
        /// <returns>The expression for the mapping</returns>
        public IMappingExpression<TSvc> Map<TSvc>()
        {
            using (var provider = new ComponentMapper())
            {
                return provider.Map<TSvc>();
            }
        }

        /// <summary>
        /// Creates a mapping to TSvc with TImpl
        /// </summary>
        /// <typeparam name="TSvc">The key type to map</typeparam>
        /// <typeparam name="TImpl">The instance type to map</typeparam>
        /// <returns>The expression for the mapping</returns>
        public IBindingExpression<TImpl> Map<TSvc, TImpl>() where TImpl : TSvc
        {
            using (var provider = new ComponentMapper(_container))
            {
                return provider.Map<TSvc, TImpl>();
            }
        }

        /// <summary>
        /// Removes all mappings of type T
        /// </summary>
        /// <typeparam name="T">The type of mappings to remove</typeparam>
        public void Clean<T>()
        {
            using (var provider = new ComponentMapper(_container))
            {
                provider.Clean<T>();
            }
        }

        #endregion

        #region IDisposeable Implementation

        /// <summary>
        /// Gets a value indicating whether this instance is disposed.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases resources held by the object.
        /// </summary>
        public virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                if (disposing && !IsDisposed)
                {
                    _container = null;

                    IsDisposed = true;
                    GC.SuppressFinalize(this);
                }
            }
        }

        /// <summary>
        /// Releases resources before the object is reclaimed by garbage collection.
        /// </summary>
        ~InjectionMapper()
        {
            Dispose(false);
        }

        #endregion
    }
}