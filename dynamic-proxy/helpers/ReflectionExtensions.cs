namespace DynamicProxy.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using Fasterflect;
    using CommonUtilities.Extensions;

    /// <summary>
    /// Extensiones de ayuda para realizar operaciones de reflexión en el namespace DynamicProxy
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Gets the method.
        /// </summary>
        /// <param name="methodname">The methodname.</param>
        /// <param name="argumentTypes">The argument types.</param>
        /// <returns>The MethodInvoker</returns>
        public static MethodInvoker GetMethod(this Type @this, string methodname, Type[] argumentTypes, Type[] genericTypeArguments)
        {
            Contract.Requires(!String.IsNullOrEmpty(methodname), "methodname is null or empty.");
            Contract.Requires(argumentTypes != null, "argTypes is null.");
            Contract.Ensures(Contract.Result<MethodInvoker>() != null);

            MethodInvoker invoker =
                genericTypeArguments == null || genericTypeArguments.Length == 0 ?
                @this.GetConcreteInvoker(methodname, argumentTypes) :
                @this.GetGenericMethodInvoker(methodname, argumentTypes, genericTypeArguments);

            return invoker;
        }

        /// <summary>
        /// Gets the concrete invoker.
        /// </summary>
        /// <param name="methodname">The methodname.</param>
        /// <param name="argumentTypes">The argument types.</param>
        /// <returns></returns>
        public static MethodInvoker GetConcreteInvoker(this Type @this, string methodname, Type[] argumentTypes)
        {
            Contract.Requires(!String.IsNullOrEmpty(methodname), "methodname is null or empty.");
            Contract.Requires(argumentTypes != null, "argumentTypes is null.");
            Contract.Ensures(Contract.Result<MethodInvoker>() != null);

            MethodInvoker invoker = @this.DelegateForCallMethod(methodname, argumentTypes);
            return invoker;
        }

        /// <summary>
        /// Gets the generic method invoker.
        /// </summary>
        /// <param name="methodname">The methodname.</param>
        /// <param name="argumentTypes">The argument types.</param>
        /// <param name="genericTypes">The generic types.</param>
        /// <returns>A MethodInvoker for the specificed genericTypes</returns>
        public static MethodInvoker GetGenericMethodInvoker(this Type @this, string methodname, Type[] argumentTypes, Type[] genericTypes)
        {
            Contract.Requires(!String.IsNullOrEmpty(methodname), "methodname is null or empty.");
            Contract.Requires(argumentTypes != null, "argumentTypes is null.");
            Contract.Requires(genericTypes != null && genericTypes.Length != 0, "genericTypes is null or empty.");

            MethodInvoker genericInvoker = null;
            MethodInfo methodInfo = @this.GetMatchingMethodInfo(methodname, argumentTypes, genericTypes);
            Contract.Assume(methodInfo != default(MethodInfo), "The type of the proxy object doesn't contain the method " + methodname + ".");
            genericInvoker = methodInfo.MakeGenericMethod(genericTypes).DelegateForCallMethod();

            return genericInvoker;
        }

        /// <summary>
        /// Gets the corresponding method info.
        /// </summary>
        /// <param name="methodname">The methodname.</param>
        /// <param name="argumentTypes">The argument types.</param>
        /// <param name="genericTypes">The generic type arguments.</param>
        /// <returns>A MethodInfo matching the method name, argument types, and generic type arguments</returns>
        public static MethodInfo GetMatchingMethodInfo(
            this Type @this,
            string methodname,
            IEnumerable<Type> argumentTypes,
            IEnumerable<Type> genericTypes,
            bool ignoreCase = false)
        {
            Contract.Requires(!String.IsNullOrEmpty(methodname), "methodname is null or empty.");
            Contract.Requires(argumentTypes!= null, "argumentTypes is null.");
            Contract.Requires(genericTypes != null, "genericTypes is null.");

            Type[] argumentTypesArray = argumentTypes.ToArray();
            Type[] genericTypesArray = genericTypes.ToArray();

            return @this.GetMethods()
                    .Where(mInfo =>
                        {
                            // Fast filter, so we don't enumerate the types
                            bool result = mInfo.IsGenericMethodDefinition
                                && string.Compare(mInfo.Name, methodname, ignoreCase) == 0
                                && mInfo.GetParameters().Length == argumentTypesArray.Length
                                && mInfo.GetGenericArguments().Length == genericTypesArray.Length;

                            // Verifies the generic types
                            result = result && AreAssignableFrom(mInfo.GetGenericArguments(), genericTypesArray);

                            // Verifies the argument types
                            result = result && (argumentTypesArray.Length == 0 
                                || AreAssignableFrom(
                                    mInfo.Parameters()
                                         .Select(paramInfo => paramInfo.ParameterType),
                                    argumentTypesArray));

                            return result;
                        })
                    .SingleOrDefault();
        }

        /// <summary>
        /// Determines whether the base type is assignable from any of the argument types
        /// </summary>
        /// <param name="type">The base type</param>
        /// <param name="argumentTypes">The argument types.</param>
        /// <returns>
        ///   <c>true</c> if [base type is assignable from any] of [the specified argument types]; otherwise, <c>false</c>.
        /// </returns>
        public static bool AreAssignableFrom(IEnumerable<Type> baseTypes, IEnumerable<Type> argumentTypes)
        {
            Contract.Requires(baseTypes != null, "baseTypes is null or empty.");
            Contract.Requires(argumentTypes != null, "argumentTypes is null or empty.");
            Contract.Requires(baseTypes.Count() == argumentTypes.Count());

            // Regresa false si existe algun baseType que no es assignable con el argumentType
            // del mismo índice
            return default(Type) != baseTypes.Where(
               (baseType, index) =>
               {
                   bool result = baseType.IsGenericTypeDefinition && !argumentTypes.ElementAt(index).IsSubclassOrImplementsBaseGeneric(baseType);
                   result = result || !baseType.IsAssignableFrom(argumentTypes.ElementAt(index));
                   
                   return result;
               }).FirstOrDefault();
        }
    }
}
