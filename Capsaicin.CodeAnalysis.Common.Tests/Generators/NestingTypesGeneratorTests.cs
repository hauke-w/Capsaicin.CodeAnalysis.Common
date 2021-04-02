using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capsaicin.CodeAnalysis.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Moq;
using Capsaicin.UnitTesting;

namespace Capsaicin.CodeAnalysis.Generators
{
    [TestClass]
    public partial class NestingTypesGeneratorTests
    {
        [TestMethod]
        public void ConstructorTest()
        {
            var actual = new NestingTypesGenerator();
            Assert.AreEqual(4, actual.IndentionStep);
        }

        #region Test data
        private static ISymbol TypeHierachyMock1 => GetTypeHierachyMock(("Class1", TypeKind.Class, false));
        private const string Class1Code = @"partial class Class1
{
    // content1
}
";

        private static ISymbol TypeHierachyMock2 => GetTypeHierachyMock(("IInterface2", TypeKind.Interface, false), ("Struct2", TypeKind.Struct, false), ("Record2", TypeKind.Class, true));

        private const string Interface2Code = @"    partial interface IInterface2
    {
        partial struct Struct2
        {
            partial record Record2
            {
                public int Property { get; init; }
            }
        }
    }
";

        private static ISymbol TypeHierachyMock3 => GetTypeHierachyMock(("Struct3", TypeKind.Struct, false));
        // 3 spaces per indention level!
        private const string Struct3Code = @"      partial struct Struct3
      {
         public int Property { get; }
      }
";

        private static ISymbol TypeHierachyMock4 => GetTypeHierachyMock();
        private const string Class4Code = "   public class Class4 { }\r\n"; // indented with 3 spaces!

        private static ISymbol GetTypeHierachyMock(params (string Name, TypeKind TypeKind, bool IsRecord)[] containingTypes)
        {
            Mock<INamedTypeSymbol>? containingTypeMock = null;
            foreach (var (name, typeKind, isRecord) in containingTypes)
            {
                var childMock = new Mock<INamedTypeSymbol>(MockBehavior.Strict);
                childMock.SetupGet(it => it.Name).Returns(name);
                childMock.SetupGet(it => it.TypeKind).Returns(typeKind);
                childMock.SetupGet(it => it.IsRecord).Returns(isRecord);
                childMock.SetupGet(it => it.ContainingType).Returns(containingTypeMock?.Object!);
                containingTypeMock = childMock;
            }
            var symbolMock = new Mock<ISymbol>(MockBehavior.Strict);
            symbolMock.SetupGet(it => it.ContainingType).Returns(containingTypeMock?.Object!);
            return symbolMock.Object;
        }
        #endregion

        [TestMethod]
        [ExpressionDataRow(Class1Code, "TypeHierachyMock1", "// content1", 0, 4)]
        [ExpressionDataRow(Interface2Code, "TypeHierachyMock2", "public int Property { get; init; }", 1, 4)]
        [ExpressionDataRow(Struct3Code, "TypeHierachyMock3", "public int Property { get; }", 2, 3)]
        [ExpressionDataRow(Class4Code, "TypeHierachyMock4", "public class Class4 { }", 1, 3)]
        public partial void GenerateContainingPartialTypeRecursiveTest(string expected, [FromCSharpExpression] ISymbol symbol, string content, int rootIndentionLevel, int indentionStep)
        {
            var testObject = new NestingTypesGenerator()
            {
                IndentionStep = indentionStep
            };
            StringBuilder sourceBuilder = new();
            NestingTypesGenerator.ContentGenerator contentGenerator = GenerateContent;
            testObject.GenerateContainingPartialTypeRecursive(symbol, sourceBuilder, contentGenerator, rootIndentionLevel);
            var actual = sourceBuilder.ToString();
            Assert.AreEqual(expected, actual);

            void GenerateContent(StringBuilder sb, int indentionLevel)
                => sb.AppendIndented(indentionLevel, content, indentionStep: indentionStep).AppendLine();
        }
    }
}