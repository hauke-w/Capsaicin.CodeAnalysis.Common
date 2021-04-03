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
        public void HasBaseTypeOfTest(bool expected, string fullTypeName, params string[] baseTypeHierarchy)
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
    }
}