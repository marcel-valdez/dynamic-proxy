using System.Diagnostics.Contracts;
namespace AutoProxy
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Responsabilidad: Mapear métodos de una interfaz a métodos definidos dinámicamente
    /// Encapsula: La estructura del mapeo, y posiblemente la creación automática de tales métodos.
    /// </summary>
    [ContractClass(typeof(IInterfaceMapCodeContract))]
    public interface IInterfaceMap : IEnumerable<IMethodMapping>
    {
        /// <summary>
        /// Gets or sets the subject on which methods are called.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        object Subject
        {
            get;
            set;
        }

        /// <summary>
        /// Processes the method invokation.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="TSubject">The type of the subject.</typeparam>
        /// <param name="name">The method name.</param>
        /// <param name="argTypes">The types of the arguments for the method</param>
        /// <param name="genericArgs">The generic type arguments</param>
        /// <returns>
        /// The result of the call (null if the method is declared as void)
        /// </returns>
        MappedMethod<TResult> GetMappedMethod<TResult, TSubject>(string name, Type[] argTypes, params Type[] genericArgs);

        /// <summary>
        /// Gets the mapped method.
        /// </summary>
        /// <typeparam name="TSubject">The type of the subject.</typeparam>
        /// <param name="name">The method name.</param>
        /// <param name="argTypes">The types of the arguments passed in to the method.</param>
        /// <param name="genericArgs">The generic argument types.</param>
        /// <returns>
        /// The mapped method
        /// </returns>
        MappedMethod GetMappedMethod<TSubject>(string name, Type[] argTypes, params Type[] genericArgs);

        /// <summary>
        /// Gets the mapped method.
        /// </summary>
        /// <param name="subjectType">Type of the subject.</param>
        /// <param name="name">The method name.</param>
        /// <param name="argTypes">The types of the arguments passed in to the method.</param>
        /// <param name="genericArgs">The generic argument types.</param>
        /// <returns>
        /// The mapped method
        /// </returns>
        MappedMethod GetMappedMethod(Type subjectType, string name, Type[] argTypes, params Type[] genericArgs);

        /// <summary>
        /// Adds the specified mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        /// <param name="replace">if set to <c>true</c> [replaces an IMethodMapping instance, if there is one that matches the signature].</param>
        void Add(IMethodMapping mapping, bool replace = false);

        /// <summary>
        /// Removes the specified mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        /// <returns>true if removed succesfully, false if it wasn't contained in this IInterfaceMap</returns>
        bool Remove(IMethodMapping mapping);
    }
}