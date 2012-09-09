namespace AutoProxy.Fluent
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    public class VoidCallProxyBuilder<TSubject, TProxy> : ProxyBuilder<TSubject>,
            IVoidProxyBuilder<TSubject, TProxy>,
            IProxyBuilder<TSubject, TProxy>
        where TSubject : class
        where TProxy : class
    {

        /// <summary>
        /// Specifies the subject member access to redirect
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        /// <returns>The object upon which to specify the return results transformation.</returns>
        public IProxyBuilder<TSubject, TProxy> WithMethod(Expression<Action<TProxy>> invocation)
        {
            return this.SetMethod<object, object>(invocation);
        }

        /// <summary>
        /// Specifies the subject member access to redirect
        /// </summary>
        /// <typeparam name="TProxyParam">The type of the proxy param.</typeparam>
        /// <typeparam name="TSubjectParam">The type of the subject param.</typeparam>
        /// <param name="invocation">The invocation.</param>
        /// <param name="with">The type parameter marker, used when the subject and proxy have the same type parameter.</param>
        /// <returns>The object upon which to specify the return results transformation.</returns>
        public IProxyBuilder<TSubject, TProxy> WithMethod<TProxyParam, TSubjectParam>(Expression<Action<TProxy>> invocation, Func<TProxyParam, TSubjectParam> with)
        {
            return SetMethod<TProxyParam, TSubjectParam>(invocation, with);
        }

        public new IToRedirector<TSubject, TProxy, TResult> Redirect<TResult>(Expression<Func<TSubject, TResult>> invocation)
        {
            var builder = new ProxyBuilder<TSubject, TProxy, TResult>(
                generator: this.generator,
                map: this.map
            );

            builder.SetInvocation(invocation);

            return builder;
        }

        /// <summary>
        /// Redirects the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        /// <returns></returns>
        /// <remarks>TODO: Refine documentation</remarks>
        public new IVoidProxyBuilder<TSubject, TProxy> Redirect(Expression<Action<TSubject>> invocation)
        {
            if (!IsMethod(invocation))
            {
                throw new ArgumentException(string.Format("Cannot redirect a {0}.", invocation.Body.NodeType));
            }

            var voidBuilder = new VoidCallProxyBuilder<TSubject, TProxy>()
            {
                generator = this.generator,
                map = this.map,
                pendingMapping = null,
                voidCall = GetMethodInvoker(invocation)
            };

            return voidBuilder;
        }

        public TProxy Proxy
        {
            get
            {
                return this.Into<TProxy>();
            }
        }

        private IProxyBuilder<TSubject, TProxy> SetMethod<TProxyParam, TSubjectParam>(Expression<Action<TProxy>> invocation, Func<TProxyParam, TSubjectParam> with = null)
        {
            if (IsMethod(invocation.Body))
            {
                if (with != null)
                {
                    Func<object, object[], object> subjectCall = (subject, proxyArgs) =>
                    {
                        TSubjectParam subjectParam = (TSubjectParam)with((TProxyParam)proxyArgs[0]);
                        return this.voidCall(subject, subjectParam);
                    };

                    this.pendingMapping = GetMapping(invocation, subjectCall);
                }
                else
                {
                    this.pendingMapping = GetMapping(invocation);
                }

                this.map.Add(pendingMapping);

                return this;
            }
            else
            {
                throw new ArgumentException(string.Format("Cannot redirect with a {0}.", invocation.Body.NodeType));
            }
        }

        protected MethodMapping GetMapping(Expression<Action<TProxy>> invocation)
        {
            Func<object, object[], object> subjectCall = (subject, args) => this.voidCall(subject, args);

            return GetMapping(invocation, subjectCall);
        }

        protected static MethodMapping GetMapping(Expression<Action<TProxy>> invocation, Func<object, object[], object> subjectCall)
        {
            MethodCallExpression methodExpression = invocation.Body as MethodCallExpression;
            MethodInfo method = methodExpression.Method;
            Type[] argTypes = GetArgumentTypes(method);
            Type[] genericTypes = GetGenericArgumentTypes(method);
            return new MethodMapping()
            {
                Name = method.Name,
                ArgumentTypes = argTypes,
                GenericArgumentTypes = genericTypes,
                Subject = subjectCall
            };
        }
    }
}
