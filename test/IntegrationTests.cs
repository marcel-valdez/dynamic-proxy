using AutoProxy;
namespace Test.AutoProxy
{
    using Castle.DynamicProxy;
    using TestingTools.Core;
    using TestingTools.Extensions;
    using HelperClasses;
    using NUnit.Framework;
    using System;

    /// <summary>
    /// Summary description for IntegrationTests
    /// </summary>
    [TestFixture]
    public class IntegrationTests
    {
        public IntegrationTests()
        {
        }

        [Test]
        public void CanCreateProxyForInterface()
        {
            // Arrange
            var generator = new ProxyGenerator();
            InterfaceMap map = new InterfaceMap()
            {
                Subject = new Properties()
            };

            MatchingInterceptor<IProperties> interceptor;
            IProperties proxy;

            // Act
            interceptor = new MatchingInterceptor<IProperties>(map);

            proxy = generator.CreateInterfaceProxyWithoutTarget<IProperties>(interceptor);
            proxy.Age = 10;
            proxy.Data = "data";
            proxy.Name = "name";

            // Assert
            Verify.That(proxy).IsNotNull().Now();
            Verify.That(proxy.Age).IsEqualTo(10).Now();
            Verify.That(proxy.Data).IsEqualTo("data").Now();
            Verify.That(proxy.Name).IsEqualTo("name").Now();
        }

        [Test]
        public void TestIfItFailsWhenItCantFindEquivalent()
        {
            // Arrange
            var generator = new ProxyGenerator();
            InterfaceMap map = new InterfaceMap()
            {
                Subject = new Properties()
            };

            MatchingInterceptor<IMoreProperties> interceptor;
            IMoreProperties proxy;

            // Act
            interceptor = new MatchingInterceptor<IMoreProperties>(map);
            proxy = generator.CreateInterfaceProxyWithoutTarget<IMoreProperties>(interceptor);
            Func<int> getter = () =>
            {
                return proxy.ExtraGetProperty;
            };

            // Assert
            Verify.That(proxy).IsNotNull().Now();
            Verify.That(getter).ThrowsException("The property ExtraProperty should fail to be found!");
        }

        [Test]
        public void TestIfANotFoundPropertyCanBeRedirected()
        {
            // Arrange
            var generator = new ProxyGenerator();
            InterfaceMap map = new InterfaceMap()
            {
                Subject = new MoreProperties()
                {
                    UnmappedProperty = "5"
                }
            };

            MethodMapping mapping = new MethodMapping()
            {
                Subject = (proxied, parameters) =>
                {
                    MoreProperties subject = (MoreProperties)proxied;
                    return int.Parse(subject.UnmappedProperty);
                },
                Name = "get_ExtraGetProperty",
                ArgumentTypes = Type.EmptyTypes,
                GenericArgumentTypes = Type.EmptyTypes
            };

            map.Add(mapping);


            MatchingInterceptor<IMoreProperties> interceptor = new MatchingInterceptor<IMoreProperties>(map);

            int expected = 5;
            int actual = 0;

            // Act
            IMoreProperties proxy = generator.CreateInterfaceProxyWithoutTarget<IMoreProperties>(interceptor);
            TestDelegate getter = () =>
            {
                actual = proxy.ExtraGetProperty;
            };

            // Assert
            Assert.That(proxy, Is.Not.Null);
            Assert.That(getter, Throws.Nothing);
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
