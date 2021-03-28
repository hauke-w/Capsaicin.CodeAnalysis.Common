using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capsaicin.CodeAnalysis.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Moq;
using System.Collections.Immutable;

namespace Capsaicin.CodeAnalysis.Extensions
{
    [TestClass()]
    public class AttributeDataExtensionsTests
    {
        [TestMethod()]
        public void EqualsAttributeClassTest()
        {
            var attributeClass = new Mock<INamedTypeSymbol>();
            attributeClass.Setup(it => it.ToDisplayString(It.Is<SymbolDisplayFormat>(x => x == null))).Returns("My.SomeAttribute");
            AttributeData attributeData = new AttributeDataMock(attributeClass.Object);
            var actual = AttributeDataExtensions.EqualsAttributeClass(attributeData, "My.SomeAttribute");
            Assert.AreEqual(true, actual);

            actual = AttributeDataExtensions.EqualsAttributeClass(attributeData, "My2.SomeAttribute");
            Assert.AreEqual(false, actual);

            actual = AttributeDataExtensions.EqualsAttributeClass(attributeData, "My.SomeAttribute2");
            Assert.AreEqual(false, actual);

            AttributeData attributeData2 = new AttributeDataMock(null);
            actual = AttributeDataExtensions.EqualsAttributeClass(attributeData2, "My.SomeAttribute");
            Assert.AreEqual(false, actual);
        }

        private class AttributeDataMock : AttributeData
        {
            public AttributeDataMock(INamedTypeSymbol? commonAttributeClass)
            {
                CommonAttributeClass = commonAttributeClass;
            }
            protected override INamedTypeSymbol? CommonAttributeClass { get; }

            protected override IMethodSymbol? CommonAttributeConstructor => throw new NotImplementedException();

            protected override SyntaxReference? CommonApplicationSyntaxReference => throw new NotImplementedException();

            protected override ImmutableArray<TypedConstant> CommonConstructorArguments => throw new NotImplementedException();

            protected override ImmutableArray<KeyValuePair<string, TypedConstant>> CommonNamedArguments => throw new NotImplementedException();
        }
    }
}