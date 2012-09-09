// ***********************************************************************
// Assembly         : AutoProxy
// Author           : Marcel Valdez
// Created          : 09-08-2012
//
// Last Modified By : Marcel Valdez
// Last Modified On : 09-08-2012
// ***********************************************************************
// <copyright file="PropertyRedirector.cs" company="Marcel Valdez">
//     Marcel Valdez. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace AutoProxy.Fluent
{
    using System;
    using Castle.DynamicProxy;

    /// <summary>
    /// Class PropertyRedirector
    /// </summary>
    /// <typeparam name="TSubject">The type of the T subject.</typeparam>
    /// <typeparam name="TSubjectResult">The type of the T subject result.</typeparam>
    /// <typeparam name="TProxyResult">The type of the T proxy result.</typeparam>
    /// <remarks>TODO: Refine documentation</remarks>
    public class PropertyRedirector<TSubject, TSubjectResult, TProxyResult> : ProxyBuilder<TSubject, TSubjectResult>,
            IWithGetOrSetRedirector<TSubject, TSubjectResult, TProxyResult>
            where TSubject : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyBuilder{TSubjectResult}" /> class.
        /// </summary>
        /// <param name="generator">The generator.</param>
        /// <param name="subject">The subject.</param>
        /// <remarks>TODO: Refine documentation</remarks>
        public PropertyRedirector(ProxyGenerator generator, TSubject subject)
            : base(generator, subject)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyRedirector{TProxyResult}" /> class.
        /// </summary>
        /// <remarks>TODO: Refine documentation</remarks>
        public PropertyRedirector()
        {}


        /// <summary>
        /// Specifies how to transform the resulting call of the subject to the proxy result.
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <returns>The proxy build object</returns>
        /// <remarks>TODO: Refine documentation</remarks>
        public IWithSetRedirector<TSubject, TSubjectResult, TProxyResult> WithGetter(Func<TSubjectResult, TProxyResult> transform)
        {
            this.pendingMapping.Name = "get_" + this.pendingMapping.Name;
            this.pendingMapping.Subject = (subject, parameters) =>
            {
                TSubjectResult result = this.accesor(((TSubject)subject));
                return transform(result);
            };

            this.map.Add(this.pendingMapping);

            return this;
        }

        /// <summary>
        /// Sets the function for transforming from the proxy result type to the subject result type
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <returns>The proxy builder object</returns>
        /// <remarks>TODO: Refine documentation</remarks>
        public IWithGetRedirector<TSubject, TSubjectResult, TProxyResult> WithSetter(
            Func<TProxyResult, TSubjectResult> transform)
        {

            this.pendingMapping.Name = "set_" + this.pendingMapping.Name;
            this.pendingMapping.ArgumentTypes = new Type[] { typeof(TProxyResult) };
            this.pendingMapping.Subject = (subject, parameters) =>
            {
                TProxyResult argument = (TProxyResult)(parameters[0]);
                TSubjectResult result = transform(argument);
                TSubject source = (TSubject)subject;
                this.setter(source, result);

                return null;
            };

            this.map.Add(this.pendingMapping);

            return this;
        }
    }
}
