using System;
using System.Collections.Generic;
using System.Text;
using AutoProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Dynamic;
using Test.AutoProxy.HelperClasses;
using Castle.DynamicProxy;
using TestingTools.Core;
using TestingTools.Extensions;
namespace Test.AutoProxy
{
    using HelperClasses;

    /// <summary>
    /// Summary description for IntegrationTests
    /// </summary>
    [TestClass]
    public class IntegrationTests
    {
        public IntegrationTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

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
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
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
    }
}
