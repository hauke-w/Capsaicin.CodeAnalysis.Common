using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capsaicin.CodeAnalysis.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Moq;

namespace Capsaicin.CodeAnalysis.Extensions
{
    [TestClass]
    public class NamedTypeSymbolExtensionsTests
    {
        [TestMethod]
        [DataRow(true, "My.Class", "My.Class")]
        [DataRow(false, "Class", "My.Class")]
        [DataRow(false, "My.Class", "Class")]
        public void EqualsFullNameTest(bool expected, string thisToDisplayString, string fullTypeName)
        {
            var namedTypeSymbolMock = new Mock<INamedTypeSymbol>(MockBehavior.Strict);
            namedTypeSymbolMock.Setup(it => it.ToDisplayString(It.Is<SymbolDisplayFormat>(x => x == null))).Returns(thisToDisplayString);
            var actual = NamedTypeSymbolExtensions.EqualsFullName(namedTypeSymbolMock.Object, fullTypeName);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(false, "My.Class1")]
        [DataRow(true, "My.Class1", "My.Class1")]
        [DataRow(false, "My.Class1", "Class1")]
        [DataRow(false, "Class1", "My.Class1")]
        [DataRow(true, "My.Class1", "Root", "My.Class1")]
        public void HasBaseTypeOfTest1(bool expected, string fullTypeName, params string[] baseTypeHierarchy)
        {
            var namedTypeSymbolMock = new Mock<INamedTypeSymbol>(MockBehavior.Strict);
            var current = namedTypeSymbolMock;
            foreach (var baseTypeFullName in baseTypeHierarchy.Reverse())
            {
                var baseTypeMock = new Mock<INamedTypeSymbol>(MockBehavior.Strict);
                baseTypeMock.Setup(it => it.ToDisplayString(It.Is<SymbolDisplayFormat>(x => x == null))).Returns(baseTypeFullName);
                current.SetupGet(it => it.BaseType).Returns(baseTypeMock.Object);
                current = baseTypeMock;
            }
            current.SetupGet(it => it.BaseType).Returns((INamedTypeSymbol?)null);
            var actual = NamedTypeSymbolExtensions.HasBaseTypeOf(namedTypeSymbolMock.Object, fullTypeName);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(false, new[] { "Class1" }, "Class2")]
        [DataRow(false, new[] { "Class1" }, "Class1")]
        [DataRow(false, new[] { "Class1", "Class2" }, "Class3")]
        [DataRow(true, new[] { "Class1", "Class2" }, "Class2")]
        [DataRow(true, new[] { "Class1", "Class2", "Class3" }, "Class3")]
        public void HasBaseTypeOfTest(bool expected, string[] typeHierarchy, string baseTypeName)
        {
            Assert.IsTrue(typeHierarchy.Length > 0, $"invalid test data: {nameof(typeHierarchy)} must have at least one item");
            ITypeSymbol type, baseType;
            SetupTypes();
            var actual = NamedTypeSymbolExtensions.HasBaseTypeOf(type, baseType);
            Assert.AreEqual(expected, actual);

            void SetupTypes()
            {
                var types = typeHierarchy.Append(baseTypeName)
                    .Distinct()
                    .ToDictionary(it => it, CreateTypeMock);
                var typeName = typeHierarchy[0];
                var currentSpecial = types[typeName];
                for (int i = 1; i < typeHierarchy.Length; i++)
                {
                    var name = typeHierarchy[i];
                    var currentBaseType = types[name];
                    currentSpecial.SetupGet(it => it.BaseType).Returns(currentBaseType.Object);
                    currentSpecial = currentBaseType;
                }
                currentSpecial.SetupGet(it => it.BaseType).Returns((INamedTypeSymbol?)null);
                type = types[typeName].Object;
                baseType = types[baseTypeName].Object;
            }

            static Mock<INamedTypeSymbol> CreateTypeMock(string name)
            {
                var mock = new Mock<INamedTypeSymbol>(MockBehavior.Strict);
                mock.Setup(it => it.Equals(mock.Object, SymbolEqualityComparer.Default)).Returns(true);
                mock.Setup(it => it.Equals(It.IsNotIn(mock.Object), SymbolEqualityComparer.Default)).Returns(false);
                return mock;
            }
        }

        [TestMethod]
        [DataRow(false, new[] { "Class1" }, "Class2")]
        [DataRow(true, new[] { "Class1" }, "Class1")]
        [DataRow(false, new[] { "Class1", "Class2" }, "Class3")]
        [DataRow(true, new[] { "Class1", "Class2" }, "Class2")]
        [DataRow(true, new[] { "Class1", "Class2", "Class3" }, "Class3")]
        public void EqualsOrHasBaseTypeOf(bool expected, string[] typeHierarchy, string baseTypeName)
        {
            Assert.IsTrue(typeHierarchy.Length > 0, $"invalid test data: {nameof(typeHierarchy)} must have at least one item");
            ITypeSymbol type, baseType;
            SetupTypes();
            var actual = NamedTypeSymbolExtensions.EqualsOrHasBaseTypeOf(type, baseType);
            Assert.AreEqual(expected, actual);

            void SetupTypes()
            {
                var types = typeHierarchy.Append(baseTypeName)
                    .Distinct()
                    .ToDictionary(it => it, CreateTypeMock);
                var typeName = typeHierarchy[0];
                var currentSpecial = types[typeName];
                for (int i = 1; i < typeHierarchy.Length; i++)
                {
                    var name = typeHierarchy[i];
                    var currentBaseType = types[name];
                    currentSpecial.SetupGet(it => it.BaseType).Returns(currentBaseType.Object);
                    currentSpecial = currentBaseType;
                }
                currentSpecial.SetupGet(it => it.BaseType).Returns((INamedTypeSymbol?)null);
                type = types[typeName].Object;
                baseType = types[baseTypeName].Object;
            }

            static Mock<INamedTypeSymbol> CreateTypeMock(string name)
            {
                var mock = new Mock<INamedTypeSymbol>(MockBehavior.Strict);
                mock.Setup(it => it.Equals(mock.Object, SymbolEqualityComparer.Default)).Returns(true);
                mock.Setup(it => it.Equals(It.IsNotIn(mock.Object), SymbolEqualityComparer.Default)).Returns(false);
                return mock;
            }
        }
    }
}