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

            static AttributeDataMock CreateAttributeDataMock(string classFullName)
            {
                var attributeClass = new Mock<INamedTypeSymbol>();
                attributeClass.Setup(it => it.ToDisplayString(It.Is<SymbolDisplayFormat>(x => x == null))).Returns(classFullName);
                return new AttributeDataMock(attributeClass.Object);
            }
        }

        [TestMethod]
        [DataRow("My.Attribute1", false, 0)]
        [DataRow("My.Attribute1", true, 0, 1, 2)]
        [DataRow("MySpecial.SpecialAttribute", false, 1)]
        [DataRow("MySpecial.SpecialAttribute", true, 1, 2)]
        [DataRow("Special", false, 2)]
        [DataRow("Special", true, 2)]
        [DataRow("Other.Special", false, 4)]
        [DataRow("Other.Special", true, 4)]
        public void GetAttributesTest(string attributeFullName, bool includeSpecializing, params int[] expectedIndexes)
        {
            var symbolMock = new Mock<ISymbol>(MockBehavior.Strict);
            var attributeClasses = new List<Mock<INamedTypeSymbol>>
            {
                CreateNamedTypeSymbolMock("My.Attribute1"),
                CreateNamedTypeSymbolMock("MySpecial.SpecialAttribute"),
                CreateNamedTypeSymbolMock("Special"),

                CreateNamedTypeSymbolMock("Other.Attribute1"),
                CreateNamedTypeSymbolMock("Other.Special"),
                CreateNamedTypeSymbolMock("Attribute1")
            };
            SetupBaseType(attributeClasses[1], attributeClasses[0]);
            SetupBaseType(attributeClasses[2], attributeClasses[1]);
            SetupBaseType(attributeClasses[4], attributeClasses[3]);

            var attributes = attributeClasses
                .Select(c => new AttributeDataMock(c.Object))
                .ToList<AttributeData>();

            symbolMock.Setup(it => it.GetAttributes()).Returns(() => ImmutableArray.Create(attributes.ToArray()));

            var actual = SymbolExtensions.GetAttributes(symbolMock.Object, attributeFullName, includeSpecializing);
            Assert.IsNotNull(actual);
            var actualAsList = actual.ToList();
            var expected = expectedIndexes.Select(i => attributes[i]).ToList();
            CollectionAssert.AreEqual(expected, actualAsList);

            static Mock<INamedTypeSymbol> CreateNamedTypeSymbolMock(string fullName)
            {
                var namedTypeSymbol = new Mock<INamedTypeSymbol>();
                namedTypeSymbol.Setup(it => it.ToDisplayString(It.Is<SymbolDisplayFormat>(x => x == null))).Returns(fullName);
                return namedTypeSymbol;
            }

            static void SetupBaseType(Mock<INamedTypeSymbol> t, Mock<INamedTypeSymbol> baseType)
                => t.SetupGet(it => it.BaseType).Returns(baseType.Object);
        }
    }
}