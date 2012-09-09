namespace AutoProxy.Fluent
{
    using System;

    public class ProxyReturnRedirector<TSubject, TProxy, TProxyResult, TSubjectResult> : ProxyBuilder<TSubject, TProxy, TSubjectResult>,
        IWithReturnRedirector<TSubject, TProxy, TProxyResult, TSubjectResult>
            where TSubject : class
            where TProxy : class
    {

        /// <summary>
        /// Gets or sets the parameters transformation.
        /// </summary>
        /// <value>The parameters transformation.</value>
        public Func<object[], object[]> ParametersTransformation
        {
            get;
            set;
        }

        public IProxyBuilder<TSubject, TProxy> WithReturn(Func<TSubjectResult, TProxyResult> transform)
        {
            this.pendingMapping.Subject = (subject, args) =>
            {
                object[] subjectParams = this.ParametersTransformation(args);
                object result = this.methodCall((TSubject)subject, subjectParams);
                object proxyResult = transform((TSubjectResult)result);
                return proxyResult;
            };

            this.map.Add(pendingMapping);

            return this;
        }
    }
}
