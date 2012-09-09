namespace AutoProxy.Fluent
{
    using System;

    public class ProxyReturnRedirector<TSubject, TProxyResult, TSubjectResult> : ProxyBuilder<TSubject, TSubjectResult>,
        IWithReturnRedirector<TSubject, TProxyResult, TSubjectResult>
            where TSubject : class
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

        public IProxyBuilder<TSubject> WithReturn(Func<TSubjectResult, TProxyResult> transform)
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
