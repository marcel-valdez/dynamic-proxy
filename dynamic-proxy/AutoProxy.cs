
namespace AutoProxy
{
    using Castle.DynamicProxy;
    using Fluent;

    public static class AutoProxy
    {
        private readonly static ProxyGenerator generator = new ProxyGenerator();

        public static IProxyBuilder<T> Proxify<T>(this T subject)
            where T : class
        {
            return new ProxyBuilder<T>(generator, subject);
        }
    }
}
