namespace AutoProxy
{
    using System;
    using System.Collections.Generic;
    using Fasterflect;

    /// <summary>
    /// Implementación concreta de IMethodMapping.
    /// </summary>
    public class MethodMapping : IMethodMapping
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodMapping"/> class.
        /// </summary>
        public MethodMapping()
        {

        }

        /// <summary>
        /// Gets or sets the name of the method.
        /// </summary>
        /// <value>
        /// The name of the method.
        /// </value>
        public string Name
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
        public IEnumerable<Type> ArgumentTypes
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
        public IEnumerable<Type> GenericArgumentTypes
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
        public Func<object, object[], object> Subject
        {
            get;
            set;
        }
    }
}
