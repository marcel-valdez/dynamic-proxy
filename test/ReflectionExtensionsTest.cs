using AutoProxy.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    [TestClass()]
    public class ReflectionExtensionsTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /// <summary>
        ///A test for GetConcreteInvoker
        ///</summary>
        [TestMethod()]
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
        [TestMethod()]
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
        [TestMethod()]
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

        /// <summary>
        ///A test for GetMethod
        ///</summary>
        [Ignore]
        [TestMethod()]
        public void GetMethodTest()
        {
            Type @this = null; // TODO: Initialize to an appropriate value
            string methodname = string.Empty; // TODO: Initialize to an appropriate value
            Type[] argumentTypes = null; // TODO: Initialize to an appropriate value
            Type[] genericTypeArguments = null; // TODO: Initialize to an appropriate value
            MethodInvoker expected = null; // TODO: Initialize to an appropriate value
            MethodInvoker actual;
            actual = ReflectionExtensions.GetMethod(@this, methodname, argumentTypes, genericTypeArguments);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
