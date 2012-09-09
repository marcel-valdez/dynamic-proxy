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
    using System.Linq.Expressions;

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
        protected ProxyGenerator generator;

        /// <summary>
        /// The interface map
        /// </summary>
        protected InterfaceMap map;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyBuilder{TSubject}" /> class.
        /// </summary>
        public ProxyBuilder()
        {}


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
        public TProxy Into<TProxy>()
            where TProxy : class
        {
            var interceptor = new MatchingInterceptor<TProxy>(map);
            return this.generator.CreateInterfaceProxyWithoutTarget<TProxy>(interceptor);
        }

        public IToRedirector<TSubject, TResult> Redirect<TResult>(Expression<Func<TSubject, TResult>> invocation)
        {
            var child = new ProxyBuilder<TSubject, TResult>()
            {
                generator = this.generator,
                map = this.map
            };
            return child.SetInvocation(invocation);
        }

        /// <summary>
        /// Redirects the specified invocation.
        /// </summary>
        /// <typeparam name="TParam">The type of the subject parameter.</typeparam>
        /// <param name="invocation">The invocation.</param>
        /// <returns>The proxy builder object</returns>
        public IToRedirector<TSubject, TParam> Redirect<TParam>(Expression<Action<TSubject, TParam>> invocation)
        {
            var child = new ProxyBuilder<TSubject, TParam>()
            {
                generator = this.generator,
                map = this.map
            };
            return child.SetInvocation(invocation);
        }
    }
}
