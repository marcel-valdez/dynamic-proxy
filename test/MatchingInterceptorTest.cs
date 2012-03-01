using System;
using System.Collections.Generic;
using System.Reflection;
using Castle.DynamicProxy;
using AutoProxy;
using Fasterflect;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Test.AutoProxy.HelperClasses;
using TestingTools.Extensions;
using TestingTools.Core;

namespace Test.AutoProxy
{
    /// <summary>
    ///This is a test class for MatchingProxyTest and is intended
    ///to contain all MatchingProxyTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MatchingInterceptorTest
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
        internal virtual IInterfaceMap CreateIInterfaceMap()
        {
            IInterfaceMap target = new InterfaceMap()
            {
                Subject = new TestClass()
            };
            return target;
        }

        internal virtual IMethodMapping CreateIMethodMapping()
        {
            Mock<IMethodMapping> mock = new Mock<IMethodMapping>();
            mock.SetupAllProperties();
            return mock.Object;
        }

        /// <summary>
        ///A test for Intercept
        ///</summary>
        [TestMethod()]
        public void InterceptTestHelper()
        {
            // Arrange
            string methodname = "IntOverloaded";
            Type[] argumentTypes = new Type[] { typeof(string) };
            IInterfaceMap interfaceMap = CreateIInterfaceMap();
            MatchingInterceptor_Accessor<TestClass> target = new MatchingInterceptor_Accessor<TestClass>(interfaceMap);

            Mock<IInvocation> mock = new Mock<IInvocation>();
            mock.SetupAllProperties();
            mock.Setup(mocked => mocked.Arguments)
                .Returns(new object[] { "parameter" });
            mock.Setup(mocked => mocked.Method)
                .Returns(typeof(TestClass)
                        .GetMethod(methodname, argumentTypes));
            IInvocation invocation = mock.Object;

            // Act
            target.Intercept(invocation);

            // Assert
            Verify.That(invocation.ReturnValue).IsEqualTo(TestClass.IntOverloadedStringReturn)
                .Now();
        }
    }
}
