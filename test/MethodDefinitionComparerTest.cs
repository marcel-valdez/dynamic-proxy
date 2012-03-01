using AutoProxy.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TestingTools.Extensions;
using TestingTools.Core;

namespace Test.AutoProxy
{
    /// <summary>
    ///This is a test class for MethodDefinitionComparerTest and is intended
    ///to contain all MethodDefinitionComparerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MethodDefinitionComparerTest
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
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        public void CanMatchEqualPairs()
        {
            // Arrange
            MethodDefinitionComparer target = new MethodDefinitionComparer();
            Type[] typeArgs = new Type[] { typeof(GenericParameterHelper), typeof(List<string>), typeof(int) };
            string testMethodName = "methodname";
            KeyValuePair<string, Type[]> x = new KeyValuePair<string, Type[]>(testMethodName, typeArgs);
            KeyValuePair<string, Type[]> y = new KeyValuePair<string, Type[]>(testMethodName, typeArgs);
            bool actual;

            // Act
            actual = target.Equals(x, y);

            // Assert
            Verify.That(actual).IsTrue()
                .Now();
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        public void CanDifferentiateAgainstEmptyArgs()
        {
            // Arrange
            MethodDefinitionComparer target = new MethodDefinitionComparer();
            Type[] typeArgs = new Type[] { typeof(GenericParameterHelper), typeof(List<string>), typeof(int) };
            string testMethodName = "methodname";
            KeyValuePair<string, Type[]> x = new KeyValuePair<string, Type[]>(testMethodName, typeArgs);
            KeyValuePair<string, Type[]> y = new KeyValuePair<string, Type[]>(testMethodName, Type.EmptyTypes);
            bool actual;

            // Act
            actual = target.Equals(x, y);

            // Assert
            Verify.That(actual).IsFalse()
                .Now();
        }

        /// <summary>
        ///A test for Equals
        ///</summary>
        [TestMethod()]
        public void CanDifferentiateAgainstDifferentNumber()
        {
            // Arrange
            MethodDefinitionComparer target = new MethodDefinitionComparer();
            Type[] typeArgs = new Type[] { typeof(GenericParameterHelper), typeof(List<string>), typeof(int) };
            Type[] typeArgs2 = new Type[] { typeof(GenericParameterHelper), typeof(List<string>), typeof(int), typeof(double) };
            string testMethodName = "methodname";
            KeyValuePair<string, Type[]> x = new KeyValuePair<string, Type[]>(testMethodName, typeArgs);
            KeyValuePair<string, Type[]> y = new KeyValuePair<string, Type[]>(testMethodName, typeArgs2);
            bool actual;

            // Act
            actual = target.Equals(x, y);

            // Assert
            Verify.That(actual).IsFalse()
                .Now();
        }

        /// <summary>
        ///A test for GetHashCode
        ///</summary>
        [TestMethod()]
        public void GeneratesDifferentHashcodeForDifferentPairs()
        {
            // Arrange
            MethodDefinitionComparer target = new MethodDefinitionComparer();
            Type[] typeArgs = new Type[] { typeof(GenericParameterHelper), typeof(List<string>), typeof(int) };
            string testMethodName = "methodname";
            Type[] typeArgs1 = new Type[] { typeof(GenericParameterHelper), typeof(List<string>), typeof(double) };
            string testMethodName1 = "methodname1";
            KeyValuePair<string, Type[]> x = new KeyValuePair<string, Type[]>(testMethodName, typeArgs);
            KeyValuePair<string, Type[]> y = new KeyValuePair<string, Type[]>(testMethodName1, typeArgs1);
            int actual;
            int not_expected;

            // Act
            actual = target.GetHashCode(x);
            not_expected = target.GetHashCode(y);

            // Assert
            Verify.That(actual).IsDifferentFrom(not_expected)
                .Now();
        }

        /// <summary>
        ///A test for GetHashCode
        ///</summary>
        [TestMethod()]
        public void GeneratesSameHashCodeForSamePairs()
        {
            // Arrange
            MethodDefinitionComparer target = new MethodDefinitionComparer();
            Type[] typeArgs = new Type[] { typeof(GenericParameterHelper), typeof(List<string>), typeof(int) };
            Type[] typeArgs2 = new Type[] { typeof(GenericParameterHelper), typeof(List<string>), typeof(int) };
            string testMethodName = "methodname";
            KeyValuePair<string, Type[]> x = new KeyValuePair<string, Type[]>(testMethodName, typeArgs);
            KeyValuePair<string, Type[]> y = new KeyValuePair<string, Type[]>(testMethodName, typeArgs2);
            int actual;
            int expected;

            // Act
            actual = target.GetHashCode(x);
            expected = target.GetHashCode(y);

            // Assert
            Verify.That(actual).IsEqualTo(expected).Now();
        }
    }
}
