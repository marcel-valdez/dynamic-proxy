// -----------------------------------------------------------------------
// <copyright file="AutoProxyTests.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using AutoProxy;
using AutoProxy.Fluent;

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
            proxy = subject.Proxify().Into<IProperties>();

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
                           .Redirect(subject => subject.UnmappedProperty)
                           .To((IMoreProperties proxy) => proxy.ExtraGetProperty)
                           .WithGetter(argument => int.Parse(argument))
                           .Into<IMoreProperties>();

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
                           .Redirect(subject => subject.UnmappedProperty)
                           .To((IMoreProperties proxy) => proxy.ExtraGetSetProperty)
                           .WithSetter((int proxyArgument) => proxyArgument.ToString())
                           .Into<IMoreProperties>();

            target.ExtraGetSetProperty = expectedExtraProperty;

            // Assert
            Assert.That(target, Is.Not.Null);
            Assert.That(source.UnmappedProperty, Is.EqualTo(expectedUnmappedProperty));
        }

        [Test]
        public void TestIfItCanRedirectMethodWithArgumentAndReturnType()
        {
            // Arrange
            int expectedNumber = 5;
            TestClass source = new TestClass();
            IExtraTestInterface<int> target;

            // Act
            target = source.Proxify()
                           .Redirect(subject => subject.IntOverloaded(With.Arg<string>()))
                           .To((IExtraTestInterface<int> proxy) => proxy.ExtraMethod(With.Arg<int>()),
                               (int number) => number.ToString())
                           .WithReturn(result => result.ToString())
                           .Into<IExtraTestInterface<int>>();

            string actual = target.ExtraMethod(expectedNumber);

            // Assert
            Assert.That(target, Is.Not.Null);
            Assert.That(actual, Is.EqualTo(TestClass.IntOverloadedStringReturn.ToString()));
        }

        [Test]
        public void TestIfItCanRedirectAMethodCall()
        {
            // Arrange
            int expectedNumber = 5;
            TestClass source = new TestClass();
            IExtraTestInterface<int> target;

            // Act
            target = source.Proxify()
                           .Redirect(subject => subject.IntOverloaded(With.Arg<string>()))
                           .To((IExtraTestInterface<int> proxy) => proxy.ExtraMethod(With.Arg<int>()),
                               (int number) => number.ToString())
                           .WithReturn(result => result.ToString())
                           .Into<IExtraTestInterface<int>>();

            string actual = target.ExtraMethod(expectedNumber);

            // Assert
            Assert.That(target, Is.Not.Null);
            Assert.That(actual, Is.EqualTo(TestClass.IntOverloadedStringReturn.ToString()));
        }

        [Test]
        public void TestIfItCanRedirectAVoidMethodCall()
        {
            // Arrange
            

            // Act


            // Assert


            // Reset

        }
    }
}
