using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capsaicin.CodeAnalysis.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Moq;
using System.Collections.Immutable;

namespace Capsaicin.CodeAnalysis.Extensions
{
    [TestClass]
    public class SymbolExtensionsTests
    {
        [TestMethod]
        public void GetContainingTypesTest()
        {
            var symbolMocks = Enumerable.Range(0, 4)
                .Select(i => new Mock<INamedTypeSymbol>(MockBehavior.Strict))
                .ToList();

            for (var i = 0; i < symbolMocks.Count; i++)
            {
                var current = symbolMocks[i];
                var parent = i > 0
                    ? symbolMocks[i - 1].Object
                    : null;
                current.SetupGet(it => it.ContainingType).Returns(parent!);
            }

            var expected = symbolMocks
                .Take(3)
                .Select(it => it.Object)
                .ToList();
            var symbolMock = symbolMocks[^1].Object;
            var actual = SymbolExtensions.GetContainingTypes(symbolMock);
            Assert.IsNotNull(actual);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EnumerateContainingTypesOutwardsTest()
        {
            var symbolMocks = Enumerable.Range(0, 6)
                .Select(i => new Mock<INamedTypeSymbol>(MockBehavior.Strict))
                .ToList();

            for (var i = 0; i < symbolMocks.Count; i++)
            {
                var current = symbolMocks[i];
                var parent = i > 0
                    ? symbolMocks[i - 1].Object
                    : null;
                current.SetupGet(it => it.ContainingType).Returns(parent!);
            }

            var expected = symbolMocks
                .Take(4)
                .Select(it => it.Object)
                .Reverse()
                .ToList();
            var symbolMock = symbolMocks[^2].Object;
            var actual = SymbolExtensions.EnumerateContainingTypesOutwards(symbolMock);
            Assert.IsNotNull(actual);
            var actualAsList = actual.ToList();
            CollectionAssert.AreEqual(expected, actualAsList);
        }

        [TestMethod]
        public void HasAttributeTest()
        {
            var symbolMock = new Mock<ISymbol>(MockBehavior.Strict);
            var attributes = new List<AttributeData>
            {
                CreateAttributeDataMock("MyNs.Attribute1"),
                CreateAttributeDataMock("My.Attribute2")
            };

            symbolMock.Setup(it => it.GetAttributes()).Returns(() => ImmutableArray.Create(attributes.ToArray()));
            var actual = SymbolExtensions.HasAttribute(symbolMock.Object, "Attribute1");
            Assert.AreEqual(false, actual);

            actual = SymbolExtensions.HasAttribute(symbolMock.Object, "MyNs.Attribute1");
            Assert.AreEqual(true, actual);

            attributes.Clear();
            actual = SymbolExtensions.HasAttribute(symbolMock.Object, "MyNs.Attribute1");
            Assert.AreEqual(false, actual);

            AttributeDataMock CreateAttributeDataMock(string classFullName)
            {
                var attributeClass = new Mock<INamedTypeSymbol>();
                attributeClass.Setup(it => it.ToDisplayString(It.Is<SymbolDisplayFormat>(x => x == null))).Returns(classFullName);
                return new AttributeDataMock(attributeClass.Object);
            }
        }
    }
}