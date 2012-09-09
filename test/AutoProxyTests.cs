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
                           .Into<IMoreProperties>()
                           .Property(proxy => proxy.ExtraGetProperty)
                           .WithGetter(argument => int.Parse(argument))
                           .Proxy;

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
                           .Into<IMoreProperties>()
                           .Property(proxy => proxy.ExtraGetSetProperty)
                           .WithSetter((int proxyArgument) => proxyArgument.ToString())
                           .Proxy;

            target.ExtraGetSetProperty = expectedExtraProperty;

            // Assert
            Assert.That(target, Is.Not.Null);
            Assert.That(source.UnmappedProperty, Is.EqualTo(expectedUnmappedProperty));
        }


        [Test]
        public void TestIfItCanBuildPropertyRedirections()
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
                           .Into<IMoreProperties>()
                           .Property(proxy => proxy.ExtraGetSetProperty)
                           .WithGetter(subjectResult => int.Parse(subjectResult))
                           .WithSetter(proxyArgument => proxyArgument.ToString())
                           .Proxy;

            target.ExtraGetSetProperty = expectedExtraProperty;
            int getterResult = target.ExtraGetSetProperty;

            // Assert
            Assert.That(target, Is.Not.Null);
            Assert.That(source.UnmappedProperty, Is.EqualTo(expectedUnmappedProperty));
            Assert.That(getterResult, Is.EqualTo(expectedExtraProperty));
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
                           .Into<IExtraTestInterface<int>>()
                           .WithMethod((IExtraTestInterface<int> proxy) => proxy.ExtraMethod(With.Arg<int>()),
                               (int number) => number.ToString())
                           .WithReturn(result => result.ToString())
                           .Proxy;

            string actual = target.ExtraMethod(expectedNumber);

            // Assert
            Assert.That(target, Is.Not.Null);
            Assert.That(actual, Is.EqualTo(TestClass.IntOverloadedStringReturn.ToString()));

            // Reset
            TestClass.IntOverloadedStringReturn = int.MinValue;
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
                           .Into<IExtraTestInterface<int>>()
                           .WithMethod(proxy => proxy.ExtraMethod(With.Arg<int>()),
                               (int number) => number.ToString())
                           .WithReturn(result => result.ToString())
                           .Proxy;
                           

            string actual = target.ExtraMethod(expectedNumber);

            // Assert
            Assert.That(target, Is.Not.Null);
            Assert.That(actual, Is.EqualTo(TestClass.IntOverloadedStringReturn.ToString()));

            // Reset
            TestClass.IntOverloadedStringReturn = int.MinValue;
        }

        [Test]
        public void TestIfItCanRedirectAVoidMethodCall()
        {
            // Arrange
            int expectedNumber = 5;
            TestClass source = new TestClass();
            IExtraTestInterface<int> target;

            // Act
            target = source.Proxify()
                           .Redirect(subject => subject.VoidOverloaded(With.Arg<String>()))
                           .Into<IExtraTestInterface<int>>()
                           .WithMethod(proxy => proxy.ExtraVoidMethod(With.Arg<int>()),
                               (int number) => number.ToString())
                           .Proxy;
                           

            target.ExtraVoidMethod(expectedNumber);

            // Assert
            Assert.That(target, Is.Not.Null);
            Assert.That(source.VoidOverloadedStringCalled, Is.True);
        }

        [Test]
        public void TestIfItCanRedirectAVoidMethodNoParams()
        {
            // Arrange
            TestClass source = new TestClass();
            IExtraTestInterface<int> target;

            // Act
            target = source.Proxify()
                           .Redirect(subject => subject.VoidOverloaded())
                           .Into<IExtraTestInterface<int>>()
                           .WithMethod(proxy => proxy.ExtraVoidMethodNoParams())
                           .Proxy;


            target.ExtraVoidMethodNoParams();

            // Assert
            Assert.That(target, Is.Not.Null);
            Assert.That(source.VoidOverloadedCalled, Is.True);
        }

        [Test]
        public void TestIfItCanRedirectAMethodWithNoParams()
        {
            // Arrange
            TestClass source = new TestClass();
            IExtraTestInterface<int> target;

            // Act
            target = source.Proxify()
                           .Redirect(subject => subject.IntOverloaded())
                           .Into<IExtraTestInterface<int>>()
                           .WithMethod(proxy => proxy.ExtraStringMethodNoParams())
                           .WithReturn(number => number.ToString())
                           .Proxy;

            string result = target.ExtraStringMethodNoParams();

            // Assert
            Assert.That(target, Is.Not.Null);
            Assert.That(result, Is.EqualTo(TestClass.IntOverloadedNoParamReturn.ToString()));
        }
    }
}
