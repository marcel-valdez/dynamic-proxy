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
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Castle.DynamicProxy;
    using Fasterflect;

    /// <summary>
    /// This class can redirect calls from the Subject to the Proxy
    /// </summary>
    /// <typeparam name="TSubject">The type of the T subject.</typeparam>
    public class ProxyBuilder<TSubject> : IStandardProxy<TSubject>, IIntoRedirector<TSubject>
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
        /// Pending mapping to be built
        /// </summary>
        protected MethodMapping pendingMapping;

        /// <summary>
        /// The void method call
        /// </summary>
        protected MethodInvoker voidCall;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyBuilder{TSubject}" /> class.
        /// </summary>
        public ProxyBuilder()
        {
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
        public TProxy Into<TProxy>()
            where TProxy : class
        {
            var interceptor = new MatchingInterceptor<TProxy>(map);
            return this.generator.CreateInterfaceProxyWithoutTarget<TProxy>(interceptor);
        }

        /// <summary>
        /// Redirects the specified invocation.
        /// </summary>
        /// <typeparam name="TParam">The type of the subject parameter.</typeparam>
        /// <param name="invocation">The invocation.</param>
        /// <returns>The proxy builder object</returns>
        public IIntoRedirector<TSubject, TResult> Redirect<TResult>(Expression<Func<TSubject, TResult>> invocation)
        {
            var child = new ProxyBuilder<TSubject, TResult>()
            {
                generator = this.generator,
                map = this.map
            };

            return child.SetInvocation(invocation);
        }

        public IIntoRedirector<TSubject> Redirect(Expression<Action<TSubject>> invocation)
        {
            if (!IsMethod(invocation.Body))
            {
                throw new ArgumentException(string.Format("Cannot redirect a {0}, only a method", invocation.Body.NodeType));
            }

            MethodInvoker Call = GetMethodInvoker(invocation);
            this.voidCall = Call;
            return this;
        }

        IVoidProxyBuilder<TSubject, TProxy> IIntoRedirector<TSubject>.Into<TProxy>()
        {
            return Prototype<TProxy>();
        }

        protected static Type[] GetArgumentTypes(MethodInfo method)
        {
            return method.GetParameters().Select(arg => arg.ParameterType).ToArray();
        }

        protected static Type[] GetGenericArgumentTypes(MethodInfo method)
        {
            return method.GetGenericArguments().Select(arg => arg.DeclaringType).ToArray();
        }


        protected VoidCallProxyBuilder<TSubject, TProxy> Prototype<TProxy>()
                               where TProxy : class
        {
            return new VoidCallProxyBuilder<TSubject, TProxy>()
            {
                generator = this.generator,
                map = this.map,
                pendingMapping = this.pendingMapping,
                voidCall = this.voidCall
            };
        }


        protected static bool IsMethod(Expression invocation)
        {
            return invocation.NodeType == ExpressionType.Call;
        }

        protected static MethodInvoker GetMethodInvoker(Expression<Action<TSubject>> invocation)
        {
            MethodCallExpression methodCall = (MethodCallExpression)invocation.Body;
            MethodInfo method = methodCall.Method;
            return method.DelegateForCallMethod();
        }
    }
}