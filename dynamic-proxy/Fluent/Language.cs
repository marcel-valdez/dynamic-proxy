namespace AutoProxy.Fluent
{
    using System;
    using System.Linq.Expressions;

    public interface IVoidProxyBuilder<TSubject, TProxy>
        where TSubject : class
        where TProxy : class
    {
        /// <summary>
        /// Specifies the subject member access to redirect
        /// </summary>
        /// <typeparam name="TProxyParam">The type of the proxy param.</typeparam>
        /// <typeparam name="TSubjectParam">The type of the subject param.</typeparam>
        /// <param name="invocation">The invocation.</param>
        /// <param name="with">The type parameter marker, used when the subject and proxy have the same type parameter.</param>
        /// <returns>The object upon which to specify the return results transformation.</returns>
        IProxyBuilder<TSubject, TProxy> WithMethod<TProxyParam, TSubjectParam>(
            Expression<Action<TProxy>> invocation,
            Func<TProxyParam, TSubjectParam> with);

        /// <summary>
        /// Specifies the subject member access to redirect
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        /// <returns>The object upon which to specify the return results transformation.</returns>
        IProxyBuilder<TSubject, TProxy> WithMethod(Expression<Action<TProxy>> invocation);
    }

    /// <summary>
    /// Interface IToRedirector has the To redirection verb
    /// </summary>
    /// <typeparam name="TSubject">The type of the T subject.</typeparam>
    public interface IToRedirector<TSubject, TProxy, TSubjectResult>
            where TSubject : class
            where TProxy : class
    {

        /// <summary>
        /// Specifies the subject member access to redirect
        /// </summary>
        /// <typeparam name="TProxy">The type of the Proxy.</typeparam>
        /// <typeparam name="TProxyResult">The type of the Proxy result.</typeparam>
        /// <param name="invocation">The invocation.</param>
        /// <returns>The object upon which to specify the proxy method to redirect.</returns>
        IWithGetOrSetRedirector<TSubject, TProxy, TSubjectResult, TProxyResult> Property<TProxyResult>(Expression<Func<TProxy, TProxyResult>> invocation);

        /// <summary>
        /// Specifies the subject member access to redirect
        /// </summary>
        /// <typeparam name="TProxyResult">The type of the Proxy result.</typeparam>
        /// <typeparam name="TSubjectParam">The type of the subject param.</typeparam>
        /// <typeparam name="TProxyParam">The type of the proxy param.</typeparam>
        /// <param name="invocation">The invocation.</param>
        /// <param name="with">The parameter transformation function.</param>
        /// <returns>The object upon which to specify the return results transformation.</returns>
        /// <remarks>TODO: Refine documentation</remarks>
        IWithReturnRedirector<TSubject, TProxy, TProxyResult, TSubjectResult> WithMethod<TProxyResult, TSubjectParam, TProxyParam>(
            Expression<Func<TProxy, TProxyResult>> invocation,
            Func<TProxyParam, TSubjectParam> with);

        /// <summary>
        /// Specifies the subject member access to redirect
        /// </summary>
        /// <typeparam name="TProxyResult">The type of the Proxy result.</typeparam>
        /// <typeparam name="TSubjectParam">The type of the subject param.</typeparam>
        /// <typeparam name="TProxyParam">The type of the proxy param.</typeparam>
        /// <param name="invocation">The invocation.</param>
        /// <param name="with">The parameter transformation function.</param>
        /// <returns>The object upon which to specify the return results transformation.</returns>
        /// <remarks>TODO: Refine documentation</remarks>
        IWithReturnRedirector<TSubject, TProxy, TProxyResult, TSubjectResult> WithMethod<TProxyResult>(
            Expression<Func<TProxy, TProxyResult>> invocation);
    }


    public interface IWithGetOrSetRedirector<TSubject, TProxy, TSubjectResult, TProxyResult> :
        IWithGetRedirector<TSubject, TProxy, TSubjectResult, TProxyResult>,
        IWithSetRedirector<TSubject, TProxy, TSubjectResult, TProxyResult>
            where TSubject : class
            where TProxy : class
    {

    }

    /// <summary>
    /// Interface IWithGetRedirector is used to specify the getter transformations.
    /// </summary>
    /// <typeparam name="TSubject">The type of the T subject.</typeparam>
    /// <typeparam name="TSubjectResult">The type of the T subject result.</typeparam>
    /// <typeparam name="TProxyResult">The type of the T proxy result.</typeparam>
    /// <remarks>TODO: Refine documentation</remarks>
    public interface IWithGetRedirector<TSubject, TProxy, TSubjectResult, TProxyResult> : IProxyBuilder<TSubject, TProxy>
            where TSubject : class
            where TProxy : class
    {
        /// <summary>
        /// Specifies the function for transforming from the subject result type to the proxy result type
        /// </summary>
        /// <typeparam name="TSubjectParam">The type of the T proxy result.</typeparam>
        /// <param name="transform">The transform.</param>
        /// <returns>The proxy builder object</returns>
        IWithSetRedirector<TSubject, TProxy, TSubjectResult, TProxyResult> WithGetter(Func<TSubjectResult, TProxyResult> transform);
    }

    /// <summary>
    /// Interface IWithSetterRedirector is used to specify the setter transformation.
    /// </summary>
    /// <typeparam name="TSubject">The type of the T subject.</typeparam>
    /// <typeparam name="TSubjectResult">The type of the T subject result.</typeparam>
    /// <typeparam name="TProxyResult">The type of the T proxy result.</typeparam>
    /// <remarks>TODO: Refine documentation</remarks>
    public interface IWithSetRedirector<TSubject, TProxy, TSubjectResult, TProxyResult> : IProxyBuilder<TSubject, TProxy>
        where TSubject : class
        where TProxy : class
    {
        /// <summary>
        /// Sets the function for transforming from the proxy result type to the subject result type
        /// </summary>
        /// <param name="transform">The transform.</param>
        /// <returns>The proxy builder object</returns>
        IWithGetRedirector<TSubject, TProxy, TSubjectResult, TProxyResult> WithSetter(Func<TProxyResult, TSubjectResult> transform);
    }

    /// <summary>
    /// Interface IWithReturnRedirector is in charge of specifying the return transformations
    /// </summary>
    /// <typeparam name="TSubject">The type of the subject.</typeparam>
    /// <typeparam name="TSubjectResult">The type of the subject result.</typeparam>
    public interface IWithReturnRedirector<TSubject, TProxy, TProxyResult, TSubjectResult>
        where TSubject : class
        where TProxy : class
    {
        /// <summary>
        /// Sets the function to transform the subject result type into the proxy result type
        /// </summary>
        /// <typeparam name="TProxyResult">The type of the proxy result.</typeparam>
        /// <param name="transform">The transform.</param>
        /// <returns>The proxy builder object</returns>
        IProxyBuilder<TSubject, TProxy> WithReturn(Func<TSubjectResult, TProxyResult> transform);
    }

    /// <summary>
    /// Interface IIntoRedirector is the mid fluent expression to determine the type of the proxy
    /// </summary>
    /// <typeparam name="TSubject">The type of the T subject.</typeparam>
    /// <typeparam name="TSubjectResult">The type of the T subject result.</typeparam>
    /// <remarks>TODO: Refine documentation</remarks>
    public interface IIntoRedirector<TSubject>
        where TSubject : class
    {
        IVoidProxyBuilder<TSubject, TProxy> Into<TProxy>()
            where TProxy : class;
    }

    /// <summary>
    /// Interface IIntoRedirector is the mid fluent expression to determine the type of the proxy
    /// </summary>
    /// <typeparam name="TSubject">The type of the T subject.</typeparam>
    /// <typeparam name="TSubjectResult">The type of the T subject result.</typeparam>
    /// <remarks>TODO: Refine documentation</remarks>
    public interface IIntoRedirector<TSubject, TSubjectResult>
        where TSubject : class
    {
        IToRedirector<TSubject, TProxy, TSubjectResult> Into<TProxy>()
            where TProxy : class;
    }

    public interface IProxifier<TSubject, TProxy>
        where TSubject : class
        where TProxy : class
    {
        TProxy Proxy
        {
            get;
        }
    }

    /// <summary>
    /// Interface IStandardProxy
    /// </summary>
    /// <typeparam name="TSubject">The type of the T subject.</typeparam>
    /// <remarks>TODO: Refine documentation</remarks>
    public interface IStandardProxy<TSubject> : IProxyBuilder<TSubject>, IProxifier<TSubject>
        where TSubject : class
    {
    }

    /// <summary>
    /// Interface IProxifier has the To verb for returning the new proxy
    /// </summary>
    /// <typeparam name="TSubject">The type of the T subject.</typeparam>
    public interface IProxifier<TSubject>
    {
        TProxy Into<TProxy>() where TProxy : class;
    }

    public interface IProxyBuilder<TSubject, TProxy> : IProxifier<TSubject, TProxy>
        where TSubject : class
        where TProxy : class
    {
        /// <summary>
        /// Redirects the specified invocation.
        /// </summary>
        /// <typeparam name="TParam">The type of the T param.</typeparam>
        /// <param name="invocation">The invocation.</param>
        /// <returns>The proxy builder object</returns>
        IToRedirector<TSubject, TProxy, TResult> Redirect<TResult>(Expression<Func<TSubject, TResult>> invocation);

        /// <summary>
        /// Redirects the specified invocation.
        /// </summary>
        /// <typeparam name="TParam">The type of the T param.</typeparam>
        /// <param name="invocation">The invocation.</param>
        /// <returns>The proxy builder object</returns>
        IVoidProxyBuilder<TSubject, TProxy> Redirect(Expression<Action<TSubject>> invocation);
    }

    /// <summary>
    /// Interface IProxyBuilder has the Redirect verb for setting up new proxies
    /// </summary>
    /// <typeparam name="TSubject">The type of the T subject.</typeparam>
    public interface IProxyBuilder<TSubject>
        where TSubject : class
    {
        /// <summary>
        /// Redirects the specified invocation.
        /// </summary>
        /// <typeparam name="TParam">The type of the T param.</typeparam>
        /// <param name="invocation">The invocation.</param>
        /// <returns>The proxy builder object</returns>
        IIntoRedirector<TSubject, TResult> Redirect<TResult>(Expression<Func<TSubject, TResult>> invocation);

        /// <summary>
        /// Redirects the specified invocation.
        /// </summary>
        /// <typeparam name="TParam">The type of the T param.</typeparam>
        /// <param name="invocation">The invocation.</param>
        /// <returns>The proxy builder object</returns>
        IIntoRedirector<TSubject> Redirect(Expression<Action<TSubject>> invocation);
    }
}
