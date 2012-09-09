// -----------------------------------------------------------------------
// <copyright file="ProxyBuilderTSubjectTProxyTResult.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace AutoProxy.Fluent
{
    using System;
    using System.Linq.Expressions;
    using Castle.DynamicProxy;


    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ProxyBuilder<TSubject, TProxy, TSubjectResult> : ProxyBuilder<TSubject, TSubjectResult>,
        IToRedirector<TSubject, TProxy, TSubjectResult>,
        IProxyBuilder<TSubject, TProxy>
        where TSubject : class
        where TProxy : class
    {
        public ProxyBuilder()
        {

        }

        public ProxyBuilder(ProxyGenerator generator, InterfaceMap map)
        {
            this.generator = generator;
            this.map = map;
        }

        public IWithGetOrSetRedirector<TSubject, TProxy, TSubjectResult, TProxyResult> Property<TProxyResult>(Expression<Func<TProxy, TProxyResult>> invocation)
        {

            MethodMapping mapping = GetMemberAccessInfo(invocation);
            var propertyRedirector = new PropertyRedirector<TSubject, TProxy, TSubjectResult, TProxyResult>()
            {
                generator = this.generator,
                map = this.map,
                setter = this.setter,
                accesor = this.accesor,
                pendingMapping = mapping
            };

            return propertyRedirector;
        }


        public IWithReturnRedirector<TSubject, TProxy, TProxyResult, TSubjectResult> WithMethod<TProxyResult>(Expression<Func<TProxy, TProxyResult>> invocation)
        {
            return SetMethod<TProxyResult, object, object>(invocation);
        }

        public IWithReturnRedirector<TSubject, TProxy, TProxyResult, TSubjectResult> WithMethod<TProxyResult, TSubjectParam, TProxyParam>(
           Expression<Func<TProxy, TProxyResult>> invocation,
           Func<TProxyParam, TSubjectParam> with)
        {
            return SetMethod<TProxyResult, TProxyParam, TSubjectParam>(invocation, with);
        }

        private IWithReturnRedirector<TSubject, TProxy, TProxyResult, TSubjectResult> SetMethod<TProxyResult, TProxyParam, TSubjectParam>
            (Expression<Func<TProxy, TProxyResult>> invocation, 
            Func<TProxyParam, TSubjectParam> with = null)
        {
            MethodMapping mapping = GetMemberAccessInfo(invocation);
            Func<object[], object[]> transform;

            if (with != null)
            {
                transform = (arguments) => new object[] { with((TProxyParam)arguments[0]) };
            }
            else
            {
                transform = (args) => args;
            }

            var returnRedirector = new ProxyReturnRedirector<TSubject, TProxy, TProxyResult, TSubjectResult>()
            {
                generator = this.generator,
                map = this.map,
                pendingMapping = mapping,
                accesor = this.accesor,
                setter = this.setter,
                methodCall = this.methodCall,
                ParametersTransformation = transform
            };

            // That's about it. (leave a pending mapping)
            // Make a new Proxy ReturnRedirector with previous data.
            return returnRedirector;
        }

        IToRedirector<TSubject, TProxy, TResult> IProxyBuilder<TSubject, TProxy>.Redirect<TResult>(Expression<Func<TSubject, TResult>> invocation)
        {
            var child = new ProxyBuilder<TSubject, TProxy, TResult>()
            {
                generator = this.generator,
                map = this.map,
            };

            return child.SetInvocation(invocation) as IToRedirector<TSubject, TProxy, TResult>;
        }

        IVoidProxyBuilder<TSubject, TProxy> IProxyBuilder<TSubject, TProxy>.Redirect(Expression<Action<TSubject>> invocation)
        {
            throw new NotImplementedException();
        }


        public TProxy Proxy
        {
            get
            {
                return (this as ProxyBuilder<TSubject>).Into<TProxy>();
            }
        }
    }
}
