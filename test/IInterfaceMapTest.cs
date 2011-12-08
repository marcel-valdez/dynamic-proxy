using System;
using DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Test.DynamicProxy.HelperClasses;
using TestingTools.Extensions;
using TestingTools.Core;
using System.Linq;
using System.Collections.Generic;

namespace Test.DynamicProxy
{

    /// <summary>
    ///This is a test class for IInterfaceMapTest and is intended
    ///to contain all IInterfaceMapTest Unit Tests
    ///</summary>
    [TestClass()]
    public class IInterfaceMapTest
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
        ///A test for Add
        ///</summary>
        [TestMethod()]
        public void CanAddWithReplaceSetToFalse()
        {
            // Arrange
            IInterfaceMap target = CreateIInterfaceMap();
            IMethodMapping mapping = CreateIMethodMapping();
            mapping.Name = "test";
            mapping.GenericArgumentTypes = new Type[] { typeof(string), typeof(object) };
            mapping.ArgumentTypes = new Type[] { typeof(bool) };
            mapping.Subject = (arg, args) =>
            {
                System.Console.Out.WriteLine("Subject called");
                return true;
            };
            bool replace = false;

            // Act
            target.Add(mapping, replace);

            // Assert
            Verify.That(target.AsEnumerable()).DoesContain(mapping)
                .Now();
        }

        [TestMethod]
        public void CanReplaceAMethodMapping()
        {
            // Arrange
            IInterfaceMap target = CreateIInterfaceMap();
            
            IMethodMapping mapping = CreateIMethodMapping();
            mapping.Name = "test";
            mapping.GenericArgumentTypes = new Type[] { typeof(string), typeof(object) };
            mapping.ArgumentTypes = new Type[] { typeof(bool) };
            mapping.Subject = (arg, args) =>
            {
                System.Console.Out.WriteLine("Subject called");
                return true;
            };
            IMethodMapping replacement = CreateIMethodMapping();
            replacement.Name = "test";
            replacement.GenericArgumentTypes = new Type[] { typeof(string), typeof(object) };
            replacement.ArgumentTypes = new Type[] { typeof(bool) };
            replacement.Subject = (arg, args) =>
            {
                System.Console.Out.WriteLine("Subject replacement called");
                return true;
            };

            // Act
            target.Add(mapping, false);
            target.Add(replacement, true);

            // Assert
            Verify.That(target.AsEnumerable()).DoesContain(replacement, "Should contain the replacement")
                .Now();
            Verify.That(target.AsEnumerable()).DoesNotContain(mapping, "Should not contain the original mapping")
                .Now();
        }

        /// <summary>
        /// A test for GetMappedMethod
        /// </summary>
        /// <param name="genericArgs"></param>
        /// <param name="argTypes"></param>
        [TestMethod()]
        public void GetMappedMethodTestWithoutReturnType()
        {
            // Arrange
            Type[] argTypes = new Type[] { typeof(float) };
            Type[] genericArgs = new Type[] { typeof(float) };
            IInterfaceMap target = CreateIInterfaceMap();
            target.Subject = new TestClass();
            const string name = "IntOverloaded";
            MappedMethod actual;

            // Act
            actual = target.GetMappedMethod<TestClass>(name, argTypes, genericArgs);

            // Assert
            int result = (int)actual(1F);
            Verify.That(result).IsEqualTo(TestClass.IntOverloadedGenericReturn).Now();
        }

        /// <summary>
        ///A test for GetMappedMethod
        ///</summary>
        [TestMethod()]
        public void GetMappedMethodTestWithReturnType()
        {
            // Arrange
            IInterfaceMap target = CreateIInterfaceMap();
            target.Subject = new TestClass();
            string name = "IntOverloaded";
            Type[] argTypes = new Type[] { typeof(double) };
            Type[] genericArgs = Type.EmptyTypes;
            MappedMethod<int> actual;

            // Act
            actual = target.GetMappedMethod<int, TestClass>(name, argTypes, genericArgs);

            // Assert
            Verify.That(actual(1.0)).IsEqualTo(TestClass.IntOverloadedDoubleReturn)
                .Now();
        }

        /// <summary>
        ///A test for Remove
        ///</summary>
        [TestMethod()]
        public void RemoveNonContainedTest()
        {
            // Arrange
            IInterfaceMap target = CreateIInterfaceMap();
            IMethodMapping mapping = CreateIMethodMapping();
            mapping.Name = "test";
            mapping.GenericArgumentTypes = Type.EmptyTypes;
            mapping.ArgumentTypes = Type.EmptyTypes;
            bool actual;

            // Act
            actual = target.Remove(mapping);

            // Assert
            Verify.That(actual).IsFalse()
                .Now();
        }

        /// <summary>
        ///A test for Remove
        ///</summary>
        [TestMethod()]
        public void RemoveContainedTest()
        {
            // Arrange
            IInterfaceMap target = CreateIInterfaceMap();
            IMethodMapping mapping = CreateIMethodMapping();
            mapping.Name = "test";
            mapping.GenericArgumentTypes = Type.EmptyTypes;
            mapping.ArgumentTypes = Type.EmptyTypes;
            bool actual;
            target.Add(mapping);

            // Act
            actual = target.Remove(mapping);

            // Assert
            Verify.That(actual).IsTrue()
                .Now();
        }
    }
}
