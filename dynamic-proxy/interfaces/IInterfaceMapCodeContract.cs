namespace AutoProxy
{
    using System.Diagnostics.Contracts;
    using System;

    [ContractClassFor(typeof(IInterfaceMap))]
    abstract class IInterfaceMapCodeContract : IInterfaceMap
    {
        #region IInterfaceMap Members

        public object Subject
        {
            get
            {
                return default(object);
            }
            set
            {
                Contract.Requires(value != null, "Subject can't be null");
                Contract.Ensures(this.Subject == value);
            }
        }

        public MappedMethod<TResult> GetMappedMethod<TResult, TSubject>(string name, Type[] argTypes, params Type[] genericArgs)
        {
            Contract.Requires(!String.IsNullOrEmpty(name), "name is null or empty.");
            Contract.Ensures(Contract.Result<MappedMethod<TResult>>() != null);
            return default(MappedMethod<TResult>);
        }

        public MappedMethod GetMappedMethod<TSubject>(string name, Type[] argTypes, params Type[] genericArgs)
        {
            Contract.Requires(!String.IsNullOrEmpty(name), "name is null or empty.");
            Contract.Ensures(Contract.Result<MappedMethod>() != null);
            return default(MappedMethod);
        }

        public void Add(IMethodMapping mapping, bool replace = false)
        {
            Contract.Requires(mapping != null, "mapping is null.");
        }

        public bool Remove(IMethodMapping mapping)
        {
            Contract.Requires(mapping != null, "mapping is null.");
            return default(bool);
        }
        #endregion

        #region IEnumerable<IMethodMapping> Members

        public System.Collections.Generic.IEnumerator<IMethodMapping> GetEnumerator()
        {
            return default(System.Collections.Generic.IEnumerator<IMethodMapping>);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return default(System.Collections.IEnumerator);
        }

        #endregion

        #region IInterfaceMap Members


        public MappedMethod GetMappedMethod(Type subjectType, string name, Type[] argTypes, params Type[] genericArgs)
        {
            Contract.Requires(!String.IsNullOrEmpty(name), "name is null or empty.");
            Contract.Ensures(Contract.Result<MappedMethod>() != null);
            return default(MappedMethod);
        }

        #endregion
    }
}
