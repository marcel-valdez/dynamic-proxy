// ***********************************************************************
// Assembly         : AutoProxy
// Author           : Marcel
// Created          : 09-08-2012
//
// Last Modified By : Marcel
// Last Modified On : 09-08-2012
// ***********************************************************************
// <copyright file="ProxyBuilderTSubject.cs" company="">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace AutoProxy.Fluent
{
    using System;
    using Castle.DynamicProxy;

    /// <summary>
    /// This class can redirect calls from the Subject to the Proxy
    /// </summary>
    /// <typeparam name="TSubject">The type of the T subject.</typeparam>
    public class ProxyBuilder<TSubject> :
        IProxyBuilder<TSubject>
        where TSubject : class
    {
        /// <summary>
        /// Inside builder instance, for Fluent API purposes.
        /// </summary>
        protected readonly ProxyGenerator generator;

        /// <summary>
        /// The interface map
        /// </summary>
        protected readonly InterfaceMap map;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyBuilder{TSubject}" /> class.
        /// </summary>
        /// <param name="generator">The generator.</param>
        /// <param name="map">The map.</param>
        public ProxyBuilder(ProxyGenerator generator, InterfaceMap map)
        {
            this.generator = generator;
            this.map = map;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyBuilder{TSubject}" /> class.
        /// </summary>
        /// <param name="generator">The generator.</param>
        /// <param name="subject">The subject.</param>
        public ProxyBuilder(ProxyGenerator generator, TSubject subject)
        {
            this.generator = generator;
            this.map = new InterfaceMap
            {
                Subject = subject
            };
        }

        /// <summary>
        /// Creates the proxy
        /// </summary>
        /// <typeparam name="TProxy">The type of the T proxy.</typeparam>
        /// <returns></returns>
        public TProxy To<TProxy>()
            where TProxy : class
        {
            var interceptor = new MatchingInterceptor<TProxy>(map);
            return this.generator.CreateInterfaceProxyWithoutTarget<TProxy>(interceptor);
        }

        /// <summary>
        /// Starts a new method redirection
        /// </summary>
        /// <typeparam name="TReturn">The type of the T return.</typeparam>
        /// <param name="invocation">The invocation.</param>
        /// <returns></returns>
        public IToRedirector<TSubject, TReturn> Redirect<TReturn>(Func<TSubject, TReturn> invocation)
        {
            var child = new ProxyBuilder<TSubject, TReturn>(this.generator, this.map);
            return child.SetCall(invocation);
        }

        public IToRedirector<TSubject, TParam> Redirect<TParam>(Action<TSubject, TParam> invocation)
        {
            var child = new ProxyBuilder<TSubject, TParam>(this.generator, this.map);
            return child.SetCall(invocation);
        }
    }
}
