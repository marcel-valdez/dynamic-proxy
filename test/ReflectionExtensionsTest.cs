using AutoProxy.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Fasterflect;
using System.Reflection;
using Test.AutoProxy.HelperClasses;
using Moq;
using TestingTools.Extensions;
using TestingTools.Core;

namespace Test.AutoProxy
{    
    
    /// <summary>
    ///This is a test class for ReflectionExtensionsTest and is intended
    ///to contain all ReflectionExtensionsTest Unit Tests
    ///</summary>
    [TestFixture()]
    public class ReflectionExtensionsTest
    {
        /// <summary>
        ///A test for GetConcreteInvoker
        ///</summary>
        [Test()]
        public void GetConcreteInvokerTest()
        {
            // Arrange
            var target = typeof(TestClass);
            string methodname = "IntOverloaded";
            Type[] argumentTypes = new Type[] { typeof(string) };
            MethodInvoker expected = typeof(TestClass).DelegateForCallMethod(methodname, argumentTypes);
            MethodInvoker actual;

            // Act
            actual = target.GetConcreteInvoker(methodname, argumentTypes);

            // Assert
            Verify.That(actual).IsEqualTo(expected)
                .Now();
        }

        /// <summary>
        ///A test for GetGenericMethodInvoker
        ///</summary>
        [Test()]
        public void GetGenericMethodInvokerTest()
        {
            // Arrange
            var @target = typeof(TestClass);
            string methodname = "IntOverloaded";
            Type[] argumentTypes = new Type[] { typeof(object) };
            Type[] genericTypes = new Type[] { typeof(object) };
            int actual;

            // Act
            MethodInvoker invoker = @target.GetGenericMethodInvoker(methodname, argumentTypes, genericTypes);
            actual = (int)invoker(new TestClass(), new object());

            // Assert
            Verify.That(actual).IsEqualTo(TestClass.IntOverloadedGenericReturn)
                .Now();
        }

        /// <summary>
        ///A test for GetMatchingMethodInfo
        ///</summary>
        // [Ignore]
        [Test()]
        public void GetMatchingMethodInfoTest()
        {
            // Arrange
            Type @this = typeof(TestClass);
            string methodname = "IntOverloaded";
            Type[] argumentTypes = new Type[] { typeof(object) };
            Type[] genericTypes = new Type[] { typeof(object) };
            bool ignoreCase = true;
            MethodInfo actual;

            // Act
            actual = @this.GetMatchingMethodInfo(methodname, argumentTypes, genericTypes, ignoreCase);

            // Assert
            Verify.That(actual).IsNotNull()
                .Now();
        }
    }
}
