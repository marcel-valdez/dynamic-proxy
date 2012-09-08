using System;
using System.Collections.Generic;
using AutoProxy.Helpers;
using NUnit.Framework;
using TestingTools.Core;
using TestingTools.Extensions;
using Test.AutoProxy.HelperClasses;

namespace Test.AutoProxy
{
    /// <summary>
    ///This is a test class for MethodDefinitionComparerTest and is intended
    ///to contain all MethodDefinitionComparerTest Unit Tests
    ///</summary>
    [TestFixture()]
    public class MethodDefinitionComparerTest
    {

        /// <summary>
        ///A test for Equals
        ///</summary>
        [Test()]
        public void CanMatchEqualPairs()
        {
            // Arrange
            MethodDefinitionComparer target = new MethodDefinitionComparer();
            Type[] typeArgs = new Type[] { typeof(TestClass), typeof(List<string>), typeof(int) };
            string TestName = "methodname";
            KeyValuePair<string, Type[]> x = new KeyValuePair<string, Type[]>(TestName, typeArgs);
            KeyValuePair<string, Type[]> y = new KeyValuePair<string, Type[]>(TestName, typeArgs);
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
        [Test()]
        public void CanDifferentiateAgainstEmptyArgs()
        {
            // Arrange
            MethodDefinitionComparer target = new MethodDefinitionComparer();
            Type[] typeArgs = new Type[] { typeof(TestClass), typeof(List<string>), typeof(int) };
            string TestName = "methodname";
            KeyValuePair<string, Type[]> x = new KeyValuePair<string, Type[]>(TestName, typeArgs);
            KeyValuePair<string, Type[]> y = new KeyValuePair<string, Type[]>(TestName, Type.EmptyTypes);
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
        [Test()]
        public void CanDifferentiateAgainstDifferentNumber()
        {
            // Arrange
            MethodDefinitionComparer target = new MethodDefinitionComparer();
            Type[] typeArgs = new Type[] { typeof(TestClass), typeof(List<string>), typeof(int) };
            Type[] typeArgs2 = new Type[] { typeof(TestClass), typeof(List<string>), typeof(int), typeof(double) };
            string TestName = "methodname";
            KeyValuePair<string, Type[]> x = new KeyValuePair<string, Type[]>(TestName, typeArgs);
            KeyValuePair<string, Type[]> y = new KeyValuePair<string, Type[]>(TestName, typeArgs2);
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
        [Test()]
        public void GeneratesDifferentHashcodeForDifferentPairs()
        {
            // Arrange
            MethodDefinitionComparer target = new MethodDefinitionComparer();
            Type[] typeArgs = new Type[] { typeof(TestClass), typeof(List<string>), typeof(int) };
            string TestName = "methodname";
            Type[] typeArgs1 = new Type[] { typeof(TestClass), typeof(List<string>), typeof(double) };
            string TestName1 = "methodname1";
            KeyValuePair<string, Type[]> x = new KeyValuePair<string, Type[]>(TestName, typeArgs);
            KeyValuePair<string, Type[]> y = new KeyValuePair<string, Type[]>(TestName1, typeArgs1);
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
        [Test()]
        public void GeneratesSameHashCodeForSamePairs()
        {
            // Arrange
            MethodDefinitionComparer target = new MethodDefinitionComparer();
            Type[] typeArgs = new Type[] { typeof(TestClass), typeof(List<string>), typeof(int) };
            Type[] typeArgs2 = new Type[] { typeof(TestClass), typeof(List<string>), typeof(int) };
            string TestName = "methodname";
            KeyValuePair<string, Type[]> x = new KeyValuePair<string, Type[]>(TestName, typeArgs);
            KeyValuePair<string, Type[]> y = new KeyValuePair<string, Type[]>(TestName, typeArgs2);
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
