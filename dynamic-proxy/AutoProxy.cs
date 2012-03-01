
namespace AutoProxy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Castle.DynamicProxy;

    public static class AutoProxy
    {
        private static ProxyGenerator generator = new ProxyGenerator();

        public static T Proxify<T>(this object subject)
            where T : class
        {
            InterfaceMap map = new InterfaceMap()
            {
                Subject = subject
            };

            var interceptor = new MatchingInterceptor<T>(map);

            return generator.CreateInterfaceProxyWithoutTarget<T>(interceptor);
        }
    }
}
