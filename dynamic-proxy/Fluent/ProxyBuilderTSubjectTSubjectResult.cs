// ***********************************************************************
// Assembly         : AutoProxy
// Author           : Marcel
// Created          : 09-08-2012
//
// Last Modified By : Marcel
// Last Modified On : 09-08-2012
// ***********************************************************************
// <copyright file="Redirector.cs" company="Marcel Valdez">
//     Marcel Valdez 2012 (r). All rights reserved.
// </copyright>
// <summary>This file is intellectual property of its author.</summary>
// ***********************************************************************
namespace AutoProxy.Fluent
{
    using System;
    using System.Linq.Expressions;
    using Castle.DynamicProxy;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// This class can redirect calls from the Subject to the Proxy
    /// </summary>
    /// <typeparam name="TSubject">The type of the T subject.</typeparam>
    /// <typeparam name="TSubjectResult">The type of the T subject result.</typeparam>
    /// <remarks>TODO: Refine documentation</remarks>
    public class ProxyBuilder<TSubject, TSubjectResult> : ProxyBuilder<TSubject>,
        IToRedirector<TSubject, TSubjectResult>, 
        IWithRedirector<TSubject, TSubjectResult>
        where TSubject : class
    {
        /// <summary>
        /// The subject invocation to redirect
        /// </summary>
        private Func<TSubject, TSubjectResult> memberAccess;

        /// <summary>
        /// The subject invocation to redirect
        /// </summary>
        private Action<TSubject, TSubjectResult> memberSet;

        private MethodMapping pendingMapping;
        

        /// <summary>
        /// Initializes a new instance of the <see cref="Redirector{TSubject}" /> class.
        /// </summary>
        /// <param name="generator">The generator.</param>
        /// <param name="map">The interface map.</param>
        public ProxyBuilder(ProxyGenerator generator, InterfaceMap map)
            : base (generator, map)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyBuilder{TSubjectResult}" /> class.
        /// </summary>
        /// <param name="generator">The generator.</param>
        /// <param name="subject">The subject.</param>
        public ProxyBuilder(ProxyGenerator generator, TSubject subject)
            : base (generator, subject)
        {            
        }

        /// <summary>
        /// Sets the call.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        /// <returns>Returns itself</returns>
        public ProxyBuilder<TSubject, TSubjectResult> SetCall(Func<TSubject, TSubjectResult> invocation)
        {
            this.memberAccess = invocation;
            return this;
        }

        /// <summary>
        /// Sets the call.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        /// <returns>Returns itself</returns>
        public ProxyBuilder<TSubject, TSubjectResult> SetCall(Action<TSubject, TSubjectResult> invocation)
        {
            this.memberSet = invocation;
            return this;
        }

        /// <summary>
        /// Specifies the subject member to redirect
        /// </summary>
        /// <typeparam name="TProxy">The type of the Proxy.</typeparam>
        /// <typeparam name="TProxyResult">The type of the Proxy result.</typeparam>
        /// <param name="invocation">The invocation.</param>
        /// <returns>The object upon which to specify the proxy method to redirect.</returns>
        public IWithRedirector<TSubject, TSubjectResult> To<TProxy, TResult>(Expression<Func<TProxy, TResult>> invocation)
            where TProxy : class
        {
            this.pendingMapping = GetMemberAccessInfo(invocation);
            return this;
        }

        /// <summary>
        /// Specifies how to transform the resulting call of the subject to the proxy result.
        /// </summary>
        /// <typeparam name="TSubjectResult">The type of the T subject result.</typeparam>
        /// <typeparam name="TProxyResult">The type of the T proxy result.</typeparam>
        /// <param name="transform">The transform.</param>
        /// <returns>The proxy build object</returns>
        public IProxyBuilder<TSubject> WithGetter<TProxyResult>(Func<TSubjectResult, TProxyResult> transform)
        {
            this.pendingMapping.Name = "get_" + this.pendingMapping.Name;
            this.pendingMapping.Subject = (subject, parameters) => {
                TSubjectResult result = this.memberAccess(((TSubject)subject));
                return transform(result);
            };

            this.map.Add(this.pendingMapping);

            return this;
        }

        /// <summary>
        /// Sets the function for transforming from the proxy result type to the subject result type
        /// </summary>
        /// <typeparam name="TProxyParam">The type of the Proxy parameter.</typeparam>
        /// <param name="transform">The transform.</param>
        /// <returns>The proxy builder object</returns>
        public IProxyBuilder<TSubject> WithSetter<TProxyParam>(Func<TProxyParam, TSubjectResult> transform)
        {

            this.pendingMapping.Name = "set_" + this.pendingMapping.Name;
            this.pendingMapping.ArgumentTypes = new Type[] { typeof(TProxyParam) };
            this.pendingMapping.Subject = (subject, parameters) =>
            {
                TProxyParam argument = (TProxyParam)(parameters[0]);
                TSubjectResult result = transform(argument);
                TSubject source = (TSubject)subject;
                this.memberSet(source, result);

                return null;
            };

            this.map.Add(this.pendingMapping);

            return this;
        }

        /// <summary>
        /// Gets the expression invocation info.
        /// </summary>
        /// <typeparam name="TProxy">The type of the Proxy.</typeparam>
        /// <typeparam name="TResult">The type of the Result.</typeparam>
        /// <param name="invocation">The invocation expression.</param>
        /// <returns>The filled in MethodMapping with pending Subject</returns>
        /// <exception cref="System.InvalidOperationException">If the invocation is not a method or property call</exception>
        private static MethodMapping GetMemberAccessInfo<TProxy, TResult>(Expression<Func<TProxy, TResult>> invocation)
                        where TProxy : class
        {
            MethodMapping info = new MethodMapping();
            Expression expression = invocation.Body;
            switch (expression.NodeType)
            {

                case ExpressionType.MemberAccess:
                    MemberExpression specificExpression = (MemberExpression)invocation.Body;
                    MemberInfo member = specificExpression.Member;
                    if (member.MemberType == MemberTypes.Property)
                    {
                        info.ArgumentTypes = Type.EmptyTypes;
                        info.GenericArgumentTypes = Type.EmptyTypes;
                        info.Name = member.Name;
                    }
                    break;
                default:
                    throw new InvalidOperationException(string.Format("Cant redirect expression with a node: {0}", invocation.Body.NodeType));
            }

            return info;
        }       
    }
}
