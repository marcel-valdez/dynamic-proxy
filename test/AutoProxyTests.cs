// -----------------------------------------------------------------------
// <copyright file="AutoProxyTests.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using AutoProxy;
namespace Test.AutoProxy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NUnit.Framework;
    using Test.AutoProxy.HelperClasses;

    [TestFixture]
    public class AutoProxyTests
    {
        [Test]
        public void TestIfItCanMakeRawAutoProxy()
        {
            // Arrange
            IProperties proxy;
            string expectedName = "expected";
            int expectedAge = 1;
            Properties subject = new Properties
            {
                Name = expectedName,
                Age = expectedAge
            };

            // Act
            proxy = subject.Proxify().To<IProperties>();

            // Assert
            Assert.That(proxy, Is.Not.Null);

            // Reset
            Assert.That(proxy.Name, Is.EqualTo(expectedName));
            Assert.That(proxy.Age, Is.EqualTo(expectedAge));
        }

        [Test]
        public void TestIfItCanBuildGetterRedirections()
        {
            // Arrange
            IMoreProperties target;
            string expectedName = "expected";
            int expectedAge = 1;
            int expectedExtraProperty = 5;
            MoreProperties source = new MoreProperties
            {
                Name = expectedName,
                Age = expectedAge,
                UnmappedProperty = expectedExtraProperty.ToString()
            };


            // Act
            target = source.Proxify()
                           .Redirect<string>((subject) => subject.UnmappedProperty)
                           .To<IMoreProperties, int>(proxy => proxy.ExtraGetProperty)
                           .WithGetter<int>(message => int.Parse(message))
                           .To<IMoreProperties>();

            // Assert
            Assert.That(target, Is.Not.Null);
            Assert.That(target.ExtraGetProperty, Is.EqualTo(expectedExtraProperty));
        }


        [Test]
        public void TestIfItCanBuildSetterRedirections()
        {
            // Arrange
            IMoreProperties target;
            string expectedName = "expected";
            int expectedAge = 1;
            int expectedExtraProperty = 5;
            string expectedUnmappedProperty = "5";
            MoreProperties source = new MoreProperties
            {
                Name = expectedName,
                Age = expectedAge,
                UnmappedProperty = expectedExtraProperty.ToString()
            };


            // Act
            target = source.Proxify()
                           .Redirect<string>((MoreProperties subject, string param) => subject.UnmappedProperty = param)
                           .To<IMoreProperties, int>((IMoreProperties proxy) => proxy.ExtraGetSetProperty)
                           .WithSetter((int proxyArgument) => proxyArgument.ToString())
                           .To<IMoreProperties>();

            target.ExtraGetSetProperty = expectedExtraProperty;

            // Assert
            Assert.That(target, Is.Not.Null);
            Assert.That(source.UnmappedProperty, Is.EqualTo(expectedUnmappedProperty));
        }
    }
}
