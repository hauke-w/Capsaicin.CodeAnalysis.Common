using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capsaicin.CodeAnalysis.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Microsoft.CodeAnalysis;

namespace Capsaicin.CodeAnalysis.Extensions
{
    [TestClass]
    public class TypeSymbolExtensionsTests
    {
        [TestMethod]
        [DataRow(NullableAnnotation.None, false)]
        [DataRow(NullableAnnotation.NotAnnotated, false)]
        [DataRow(NullableAnnotation.Annotated, true)]
        public void AllowsNullTest(NullableAnnotation nullableAnnotation, bool expected)
        {
            var typeSymbolMock = new Mock<ITypeSymbol>(MockBehavior.Strict);
            typeSymbolMock.SetupGet(it => it.NullableAnnotation).Returns(nullableAnnotation);
            var actual = TypeSymbolExtensions.AllowsNull(typeSymbolMock.Object);
            Assert.AreEqual(expected, actual);
        }
    }
}