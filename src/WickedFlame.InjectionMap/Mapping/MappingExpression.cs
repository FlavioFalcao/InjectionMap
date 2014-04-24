﻿using System;
using System.Linq.Expressions;
using WickedFlame.InjectionMap.Exceptions;
using WickedFlame.InjectionMap.Expressions;
using WickedFlame.InjectionMap.Internals;

namespace WickedFlame.InjectionMap.Mapping
{
    internal class MappingExpression<T> : BindableComponent, IMappingExpression<T>
    {
        public MappingExpression(IComponentCollection container, IMappingComponent component)
            : base(container, component)
        {
        }

        #region IMappingExpression Implementation

        public IBindingExpression<TImpl> For<TImpl>()
        {
            return CreateBinding<TImpl>(null);
        }

        public IBindingExpression<TImpl> For<TImpl>(TImpl value)
        {
            return CreateBinding<TImpl>(() => value);
        }

        public IBindingExpression<TImpl> For<TImpl>(Expression<Func<TImpl>> callback)
        {
            return CreateBinding<TImpl>(callback);
        }

        public IBindingExpression<T> ToSelf()
        {
            return CreateBinding();
        }

        public IMappingExpression<T> OnResolved(Action<T> callback)
        {
            throw new NotImplementedException();

            //var component = Component.CreateComponent<T>();
            //component.OnResolvedCallback = callback;

            //return component.CreateBinding<T>(Container);
        }

        /// <summary>
        /// Adds a substitute mapping for the mapping
        /// </summary>
        /// <typeparam name="TImpl">The implementing type of the substitue</typeparam>
        /// <returns>New IBindingExpression with the substitute</returns>
        public IBindingExpression<TImpl> Substitute<TImpl>()
        {
            Ensure.MappingTypesMatch(Component.KeyType, typeof(TImpl));

            var component = Component.CreateComponent<TImpl>();
            component.IsSubstitute = true;

            return component.CreateBinding<TImpl>(Container);
        }

        /// <summary>
        /// Adds a substitute mapping for the mapping
        /// </summary>
        /// <typeparam name="T">Key type to create a substitute for</typeparam>
        /// <param name="callback">Callback expression to generate substitute</param>
        /// <returns>New IBindingExpression with the substitute</returns>
        public IBindingExpression<T> Substitute(Expression<Func<T>> callback)
        {
            Ensure.MappingTypesMatch(Component.KeyType, typeof(T));

            var component = Component.CreateComponent<T>();
            component.IsSubstitute = true;
            component.ValueCallback = callback;

            return component.CreateBinding<T>(Container);
        }


        #endregion

        #region Implementation

        internal IBindingExpression<TImpl> CreateBinding<TImpl>(Expression<Func<TImpl>> callback)
        {
            Ensure.MappingTypesMatch(Component.KeyType, typeof(T));

            var component = Component.CreateComponent<TImpl>();
            component.ValueCallback = callback;
            return component.CreateBinding<TImpl>(Container);
        }

        internal IBindingExpression<T> CreateBinding()
        {
            Ensure.MappingTypesMatch(Component.KeyType, typeof(T));

            var component = Component.CreateComponent<T>();
            return component.CreateBinding<T>(Container);
        }

        #endregion
    }
}
