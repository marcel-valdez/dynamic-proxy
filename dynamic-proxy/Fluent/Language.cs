namespace AutoProxy.Fluent
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Interface IToRedirector has the To redirection verb
    /// </summary>
    /// <typeparam name="TSubject">The type of the T subject.</typeparam>
    public interface IToRedirector<TSubject, TSubjectResult>
            where TSubject : class
    {

        /// <summary>
        /// Specifies the subject member access to redirect
        /// </summary>
        /// <typeparam name="TProxy">The type of the Proxy.</typeparam>
        /// <typeparam name="TProxyResult">The type of the Proxy result.</typeparam>
        /// <param name="invocation">The invocation.</param>
        /// <returns>The object upon which to specify the proxy method to redirect.</returns>
        IWithRedirector<TSubject, TSubjectResult> To<TProxy, TProxyResult>(Expression<Func<TProxy, TProxyResult>> invocation)
            where TProxy : class;
    }

    /// <summary>
    /// Interface IWithRedirector has the With verb for creating proxies
    /// </summary>
    /// <typeparam name="TSubject">The type of the T subject.</typeparam>
    public interface IWithRedirector<TSubject, TSubjectResult>
            where TSubject : class
    {
        /// <summary>
        /// Specifies the function for transforming from the subject result type to the proxy result type
        /// </summary>
        /// <typeparam name="TProxyResult">The type of the T proxy result.</typeparam>
        /// <param name="transform">The transform.</param>
        /// <returns>The proxy builder object</returns>
        IProxyBuilder<TSubject> WithGetter<TProxyResult>(Func<TSubjectResult, TProxyResult> transform);

        /// <summary>
        /// Sets the function for transforming from the proxy result type to the subject result type
        /// </summary>
        /// <typeparam name="TProxyResult">The type of the T proxy result.</typeparam>
        /// <param name="transform">The transform.</param>
        /// <returns>The proxy builder object</returns>
        IProxyBuilder<TSubject> WithSetter<TProxyResult>(Func<TProxyResult, TSubjectResult> transform);
    }


    /// <summary>
    /// Interface IProxifier has the To verb for returning the new proxy
    /// </summary>
    /// <typeparam name="TSubject">The type of the T subject.</typeparam>
    public interface IProxifier<TSubject>
    {
        TProxy To<TProxy>() where TProxy : class;
    }

    /// <summary>
    /// Interface IProxyBuilder has the Redirect verb for setting up new proxies
    /// </summary>
    /// <typeparam name="TSubject">The type of the T subject.</typeparam>
    public interface IProxyBuilder<TSubject> : IProxifier<TSubject>
        where TSubject : class
    {
        /// <summary>
        /// Redirects the specified invocation.
        /// </summary>
        /// <typeparam name="TReturn">The type of the T return.</typeparam>
        /// <param name="invocation">The invocation.</param>
        /// <returns>The proxy builder object</returns>
        IToRedirector<TSubject, TReturn> Redirect<TReturn>(Func<TSubject, TReturn> invocation);

        /// <summary>
        /// Redirects the specified invocation.
        /// </summary>
        /// <typeparam name="TParam">The type of the T param.</typeparam>
        /// <param name="invocation">The invocation.</param>
        /// <returns>The proxy builder object</returns>
        IToRedirector<TSubject, TParam> Redirect<TParam>(Action<TSubject, TParam> invocation);
    }
}
