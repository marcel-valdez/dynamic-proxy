using System;
using System.Collections.Generic;
using System.Reflection;
using Castle.DynamicProxy;
using AutoProxy;
using Fasterflect;
using NUnit.Framework;
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
    [TestFixture()]
    public class MatchingInterceptorTest
    {

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
        [Test()]
        public void InterceptTestHelper()
        {
            // Arrange
            string methodname = "IntOverloaded";
            Type[] argumentTypes = new Type[] { typeof(string) };
            IInterfaceMap interfaceMap = CreateIInterfaceMap();
            MatchingInterceptor<TestClass> target = new MatchingInterceptor<TestClass>(interfaceMap);

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
