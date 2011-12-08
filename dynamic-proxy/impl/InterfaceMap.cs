//-----------------------------------------------------------------------
// <copyright file="InterfaceMap.cs" company="Route Manager de México">
//     Copyright 2011 (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace DynamicProxy
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using CommonUtilities.Reflection;
    using DynamicProxy.Extensions;

    /// <summary>
    /// Responsabilidad: Mapear lo métodos abstractos de una interfaz a métodos concretos
    /// Encapsula: La implementación estructural y creacional para contener y crear los métodos concretos.
    /// Detalles: Cuando no se ha mapeado un método manualmente, esta clase intenta buscar un método que
    /// cumpla con la declaración que se intenta invocar.
    /// </summary>
    public class InterfaceMap : IInterfaceMap
    {
        private static readonly TypeArrayComparer comparer = new TypeArrayComparer();
        private readonly IList<IMethodMapping> mappings = new List<IMethodMapping>();
        private object subject = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="InterfaceMap"/> class.
        /// </summary>
        public InterfaceMap()
        {
        }

        /// <summary>
        /// Gets or sets the subject on which methods are called.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        public object Subject
        {
            get
            {
                return this.subject;
            }

            set
            {
                this.subject = value;
            }
        }

        /// <summary>
        /// Processes the method invokation.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <typeparam name="TSubject">The type of the subject.</typeparam>
        /// <param name="name">The method name.</param>
        /// <param name="argTypes">The arg types.</param>
        /// <param name="genericArgs">The generic type arguments</param>
        /// <returns>
        /// The result of the call (null if the method is declared as void)
        /// </returns>
        public MappedMethod<TResult> GetMappedMethod<TResult, TSubject>(string name, Type[] argTypes, params Type[] genericArgs)
        {
            argTypes = argTypes ?? Type.EmptyTypes;

            MappedMethod mapping = this.GetMappedMethod<TSubject>(name, argTypes, genericArgs);
            return (args) => (TResult)mapping(args);
        }

        /// <summary>
        /// Gets the mapped method.
        /// </summary>
        /// <typeparam name="TSubject">The type of the subject.</typeparam>
        /// <param name="name">The method name.</param>
        /// <param name="argTypes">The arguments passed in to the method.</param>
        /// <param name="genericArgs">The generic argument types.</param>
        /// <returns>
        /// The mapped method
        /// </returns>
        public MappedMethod GetMappedMethod<TSubject>(string name, Type[] argTypes, params Type[] genericArgs)
        {
            argTypes = argTypes ?? Type.EmptyTypes;
            Contract.Assume(!string.IsNullOrEmpty(name));
            IMethodMapping mapping = this.Lookup(name, argTypes, genericArgs);
            if (mapping == default(IMethodMapping))
            {
                Fasterflect.MethodInvoker invoker = typeof(TSubject).GetMethod(name, argTypes, genericArgs);

                // TODO: Abstract IMethodMapping creation?
                mapping = new MethodMapping()
                {
                    Name = name,
                    ArgumentTypes = argTypes,
                    GenericArgumentTypes = genericArgs,
                    Subject = (subject, args) => invoker(subject, args)
                };

                this.Add(mapping);
            }

            MappedMethod method = (args) =>
            {
                return mapping.Subject(this.Subject, args);
            };

            return method;
        }

        /// <summary>
        /// Adds the specified mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        /// <param name="replace">if set to <c>true</c> [replaces an IMethodMapping instance, if there is one that matches the signature].</param>
        public void Add(IMethodMapping mapping, bool replace = false)
        {
            Contract.Ensures(this.Contains(mapping));

            if (replace)
            {
                this.Remove(mapping);
            }

            this.mappings.Add(mapping);
        }

        /// <summary>
        /// Removes the specified mapping.
        /// </summary>
        /// <param name="mapping">The mapping.</param>
        /// <returns>
        /// true if removed succesfully, false if it wasn't contained in this IInterfaceMap
        /// </returns>
        public bool Remove(IMethodMapping mapping)
        {
            Contract.Ensures(!this.Contains(mapping));

            IMethodMapping old = this.Lookup(mapping.Name, mapping.ArgumentTypes.ToArray(), mapping.GenericArgumentTypes.ToArray());
            if (old != default(IMethodMapping))
            {
                return this.mappings.Remove(old);
            }

            return false;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<IMethodMapping> GetEnumerator()
        {
            return this.mappings.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.mappings.GetEnumerator();
        }

        /// <summary>
        /// Lookups the specified method mapping.
        /// </summary>
        /// <param name="methodName">Name of the method.</param>
        /// <param name="argumentTypes">The argument types.</param>
        /// <param name="genericArgumentTypes">The generic argument types.</param>
        /// <returns>The MethodMapping</returns>
        private IMethodMapping Lookup(string methodName, Type[] argumentTypes, params Type[] genericArgumentTypes)
        {
            Contract.Requires(!String.IsNullOrEmpty(methodName), "name is null or empty.");
            Contract.Requires(argumentTypes != null, "argTypes is null.");

            return this.mappings.FirstOrDefault(mapping => mapping.Name == methodName
                && (comparer.GetHashCode(argumentTypes) == comparer.GetHashCode(mapping.ArgumentTypes.ToArray()) ||
                    comparer.Equals(argumentTypes, mapping.ArgumentTypes.ToArray()))
                && (comparer.GetHashCode(genericArgumentTypes) == comparer.GetHashCode(mapping.GenericArgumentTypes.ToArray()) ||
                    comparer.Equals(argumentTypes, mapping.GenericArgumentTypes.ToArray())));
        }

        [ContractInvariantMethod]
        private void Invariant()
        {
            Contract.Invariant(this.mappings != null);
            Contract.Invariant(InterfaceMap.comparer != null);
        }
    }
}