namespace AutoProxy
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text.RegularExpressions;

    [ContractClassFor(typeof(IMethodMapping))]
    internal abstract class IMethodMappingCodeContract : IMethodMapping
    {
        #region IMethodMapping Members

        public string Name
        {
            get
            {
                return default(string);
            }

            set
            {
                Contract.Requires(!string.IsNullOrWhiteSpace(value));
                Contract.Requires(Regex.IsMatch(value, "[a-zA-Z0-9_]+"));
            }
        }

        public IEnumerable<Type> ArgumentTypes
        {
            get
            {
                return default(IEnumerable<Type>);
            }

            set
            {
                Contract.Requires(value != null);
            }
        }

        public IEnumerable<Type> GenericArgumentTypes
        {
            get
            {
                return default(IEnumerable<Type>);
            }

            set
            {
                Contract.Requires(value != null);
            }
        }

        public Func<object, object[], object> Subject
        {
            get
            {
                Contract.Ensures(Contract.Result<Func<object, object[], object>>() != null);
                return default(Func<object, object[], object>);
            }

            set
            {
                Contract.Requires(value != null);
            }
        }

        #endregion
    }
}
