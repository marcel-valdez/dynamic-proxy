namespace DynamicProxy
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the relation of a reflected method to a Method Invokation
    /// </summary>
    public interface IMethodMapping
    {
        /// <summary>
        /// Gets or sets the name of the method.
        /// </summary>
        /// <value>
        /// The name of the method.
        /// </value>
        string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the argument types.
        /// </summary>
        /// <value>
        /// The argument types.
        /// </value>
        IEnumerable<Type> ArgumentTypes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the generic argument types.
        /// </summary>
        /// <value>
        /// The generic argument types.
        /// </value>
        IEnumerable<Type> GenericArgumentTypes
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the subject method to be called.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        Func<object, object[], object> Subject
        {
            get;
            set;
        }
    }
}