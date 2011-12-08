// ----------------------------------------------------------------------
// <copyright file="MatchingInterceptor.cs" company="Route Manager de México">
//     Copyright Route Manager de México(c) 2011. All rights reserved.
// </copyright>
// ------------------------------------------------------------------------
namespace DynamicProxy
{
    using System;
    using System.Diagnostics.Contracts;
    using Castle.DynamicProxy;

    /// <summary>
    /// Clase encargada de interceptar llamadas de método y redirigirlas
    /// a un objeto específico.
    /// </summary>
    /// <typeparam name="T">Tipo de objeto que recibirá las llamadas</typeparam>
    public class MatchingInterceptor<T> : IInterceptor
        where T : class
    {
        private readonly IInterfaceMap interfaceMap = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MatchingInterceptor&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="proxy">The proxy.</param>
        public MatchingInterceptor(IInterfaceMap interfaceMap)
        {
            Contract.Requires(interfaceMap != null);
            Contract.Ensures(this.interfaceMap == interfaceMap);
            this.interfaceMap = interfaceMap;
        }

        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        public void Intercept(IInvocation invocation)
        {
            Contract.Assume(invocation != null);
            Contract.Assume(invocation.Arguments != null);
            Contract.Assume(invocation.Method != null);

            Type[] argumentTypes = Type.GetTypeArray(invocation.Arguments);
            string methodName = invocation.Method.Name;
            MappedMethod mapping = this.interfaceMap.GetMappedMethod<T>(methodName, argumentTypes, invocation.GenericArguments);
            Contract.Assume(mapping != null);
            invocation.ReturnValue = mapping(invocation.Arguments);
            invocation.Proceed();
        }

        [ContractInvariantMethod]
        private void InvariantMethod()
        {
            Contract.Invariant(this.interfaceMap != null);
        }
    }
}
