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
    using System.Linq;
    using System.Linq.Expressions;
    using Castle.DynamicProxy;
    using System.Collections.Generic;
    using System.Reflection;
    using Fasterflect;

    /// <summary>
    /// This class can redirect calls from the Subject to the Proxy
    /// </summary>
    /// <typeparam name="TSubject">The type of the T subject.</typeparam>
    /// <typeparam name="TSubjectResult">The type of the T subject result.</typeparam>
    public class ProxyBuilder<TSubject, TSubjectResult> : ProxyBuilder<TSubject>,
        IToRedirector<TSubject, TSubjectResult>
        where TSubject : class
    {
        /// <summary>
        /// The subject invocation to redirect
        /// </summary>
        protected Func<TSubject, TSubjectResult> accesor;

        /// <summary>
        /// The method call to get the results on the subject
        /// </summary>
        protected Func<TSubject, object[], TSubjectResult> methodCall;

        /// <summary>
        /// The subject invocation to redirect
        /// </summary>
        protected Action<TSubject, TSubjectResult> setter;

        /// <summary>
        /// Pending mapping to be build
        /// </summary>
        protected MethodMapping pendingMapping;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyBuilder{TSubjectResult}" /> class.
        /// </summary>
        /// <param name="generator">The generator.</param>
        /// <param name="subject">The subject.</param>
        public ProxyBuilder(ProxyGenerator generator, TSubject subject)
            : base(generator, subject)
        {
        }

        public ProxyBuilder()
        {

        }

        /// <summary>
        /// Sets the invocation to redirect
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        /// <returns>Returns itself</returns>
        public ProxyBuilder<TSubject, TSubjectResult> SetInvocation(Expression<Func<TSubject, TSubjectResult>> invocation)
        {
            if (IsProperty(invocation.Body))
            {
                this.accesor = invocation.Compile();
                this.setter = GetSetter(invocation);
            }
            else
            {
                if (invocation.Body.NodeType == ExpressionType.Call)
                {
                    this.methodCall = GetMethodCall(invocation);
                }
            }

            return this;
        }

        /// <summary>
        /// Sets the invocation to redirect.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        /// <returns>Returns itself</returns>
        public ProxyBuilder<TSubject, TSubjectResult> SetInvocation(Expression<Action<TSubject, TSubjectResult>> invocation)
        {
            //this.memberSet = invocation.Compile();
            return this;
        }


        public IWithGetOrSetRedirector<TSubject, TSubjectResult, TProxyResult> To<TProxy, TProxyResult>(
            Expression<Func<TProxy, TProxyResult>> invocation) where TProxy : class
        {
            MethodMapping mapping = GetMemberAccessInfo(invocation);
            var propertyRedirector = new PropertyRedirector<TSubject, TSubjectResult, TProxyResult>()
            {
                generator = this.generator,
                map = this.map,
                setter = this.setter,
                accesor = this.accesor,
                pendingMapping = mapping
            };

            return propertyRedirector;
        }

        public IWithReturnRedirector<TSubject, TProxyResult, TSubjectResult> To<TProxy, TProxyResult, TSubjectParam, TProxyParam>(
            Expression<Func<TProxy, TProxyResult>> invocation, Func<TProxyParam, TSubjectParam> with) where TProxy : class
        {
            MethodMapping mapping = GetMemberAccessInfo(invocation);
            var returnRedirector = new ProxyReturnRedirector<TSubject, TProxyResult, TSubjectResult>()
            {
                generator = this.generator,
                map = this.map,
                pendingMapping = mapping,
                accesor = this.accesor,
                setter = this.setter,
                methodCall = this.methodCall,
                ParametersTransformation = (arguments) => new object[] { with((TProxyParam)arguments[0]) }
            };

            // That's about it. (leave a pending mapping)
            // Make a new Proxy ReturnRedirector with previous data.
            return returnRedirector;
        }

        public IWithReturnRedirector<TSubject, TSubjectResult, TSubjectResult> To<TProxy, TProxyResult, TSubjectParam>(
            Expression<Func<TProxy, TProxyResult>> invocation,
            TSubjectParam with) where TProxy : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines whether the specified body is a property.
        /// </summary>
        /// <param name="body">The expression body.</param>
        /// <returns><c>true</c> if the specified body is property; otherwise, <c>false</c>.</returns>
        private static bool IsProperty(Expression body)
        {
            if (body.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression expression = body as MemberExpression;
                MemberInfo member = expression.Member;

                return member.MemberType == MemberTypes.Property;
            }

            return false;
        }

        /// <summary>
        /// Gets the setter of a property, given its getter expression.
        /// </summary>
        /// <param name="invocation">The invocation of the setter (if any).</param>
        /// <returns>The setter, null if not found.</returns>
        private static Action<TSubject, TSubjectResult> GetSetter(Expression<Func<TSubject, TSubjectResult>> invocation)
        {
            Expression body = invocation.Body;
            if (body.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression expression = body as MemberExpression;
                MemberInfo member = expression.Member;

                if (member.MemberType == MemberTypes.Property)
                {
                    PropertyInfo property = member as PropertyInfo;
                    if (property.CanWrite)
                    {
                        // Create and compile the member setter
                        var method = property.GetSetMethod();
                        return (subject, args) => method.Invoke(subject, new object[] { args });
                    }
                }
            }

            return null;
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
                    {

                        MemberExpression specificExpression = (MemberExpression)invocation.Body;
                        MemberInfo member = specificExpression.Member;
                        if (member.MemberType == MemberTypes.Property)
                        {
                            info.ArgumentTypes = Type.EmptyTypes;
                            info.GenericArgumentTypes = Type.EmptyTypes;
                            info.Name = member.Name;
                        }
                    }
                    break;

                case ExpressionType.Call:
                    {
                        MethodCallExpression specificExpression = (MethodCallExpression)invocation.Body;
                        MethodInfo method = specificExpression.Method;

                        info.ArgumentTypes = GetArgumentTypes(method);
                        info.GenericArgumentTypes = GetGenericArgumentTypes(method);
                        info.Name = method.Name;
                    }
                    break;
                default:
                    throw new InvalidOperationException(string.Format("Cant redirect expression with a node: {0}", invocation.Body.NodeType));
            }

            return info;
        }

        /// <summary>
        /// Gets the method call.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        /// <returns>The translated method call</returns>
        private Func<TSubject, object[], TSubjectResult> GetMethodCall(Expression<Func<TSubject, TSubjectResult>> invocation)
        {
            MethodCallExpression methodExpression = invocation.Body as MethodCallExpression;
            MethodInfo method = methodExpression.Method;

            Func<TSubject, object[], TSubjectResult> methodCall = (subject, parameters) =>
            {
                return (TSubjectResult)method.Invoke(subject, parameters);
            };

            return methodCall;
        }

        private static Type[] GetArgumentTypes(MethodInfo method)
        {
            return method.GetParameters().Select(arg => arg.ParameterType).ToArray();
        }

        private static Type[] GetGenericArgumentTypes(MethodInfo method)
        {
            return method.GetGenericArguments().Select(arg => arg.DeclaringType).ToArray();
        }
    }
}