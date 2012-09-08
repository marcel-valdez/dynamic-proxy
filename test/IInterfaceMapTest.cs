using System;
using AutoProxy;
using NUnit.Framework;
using Moq;
using Test.AutoProxy.HelperClasses;
using TestingTools.Extensions;
using TestingTools.Core;
using System.Linq;
using System.Collections.Generic;

namespace Test.AutoProxy
{

    /// <summary>
    ///This is a test class for IInterfaceMapTest and is intended
    ///to contain all IInterfaceMapTest Unit Tests
    ///</summary>
    [TestFixture()]
    public class IInterfaceMapTest
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
        ///A test for Add
        ///</summary>
        [Test()]
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
                Console.Out.WriteLine("Subject called");
                return true;
            };
            bool replace = false;

            // Act
            target.Add(mapping, replace);

            // Assert
            Verify.That(target.AsEnumerable()).DoesContain(mapping)
                .Now();
        }

        [Test]
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
                Console.Out.WriteLine("Subject called");
                return true;
            };
            IMethodMapping replacement = CreateIMethodMapping();
            replacement.Name = "test";
            replacement.GenericArgumentTypes = new Type[] { typeof(string), typeof(object) };
            replacement.ArgumentTypes = new Type[] { typeof(bool) };
            replacement.Subject = (arg, args) =>
            {
                Console.Out.WriteLine("Subject replacement called");
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
        [Test()]
        public void CanGetMappedMethodTestWithoutReturnType()
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
        /// A test for GetMappedMethod
        /// </summary>
        /// <param name="genericArgs"></param>
        /// <param name="argTypes"></param>
        [Test()]
        public void CanGetMappedMethodTestWithWithouhtGenericArguments()
        {
            // Arrange
            Type[] argTypes = new Type[] { typeof(double) };
            IInterfaceMap target = CreateIInterfaceMap();
            target.Subject = new TestClass();
            const string name = "IntOverloaded";
            MappedMethod actual;

            // Act
            actual = target.GetMappedMethod<TestClass>(name, argTypes);
            int result = (int)actual(1D);

            // Assert
            Verify.That(result).IsEqualTo(TestClass.IntOverloadedDoubleReturn).Now();
        }

        /// <summary>
        ///A test for GetMappedMethod
        ///</summary>
        [Test()]
        public void CanGetMappedMethodTestWithReturnType()
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
        [Test()]
        public void CanRemoveNonContained()
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
        [Test()]
        public void CanRemoveContained()
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
