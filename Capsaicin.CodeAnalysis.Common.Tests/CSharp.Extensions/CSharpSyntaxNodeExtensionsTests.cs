using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capsaicin.UnitTesting.Generators;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Capsaicin.UnitTesting.Generators
{
    [TestClass]
    public class CSharpSyntaxNodeExtensionsTests
    {
        [TestMethod]
        public void GetUsingsFromContainingCompilationUnitTest()
        {
            var propertyCode = "public string Property1 { get; set; }";
            var code = $@"using My.Namespace;
using My.Other.Namespace;
using My.Namespace; // deliberate duplicate!
using My.NameSpace; // different casing!
using Alias=My.Something;

namespace
{{
    class Class1
    {{
        {propertyCode}
    }}
}}";

            var compilationUnit = (CompilationUnitSyntax)SyntaxFactory.ParseSyntaxTree(code).GetRoot();
            int start = code.IndexOf(propertyCode);
            int end = start + propertyCode.Length;
            var syntaxNode = (CSharpSyntaxNode)compilationUnit.FindNode(TextSpan.FromBounds(start, end));
            var expected = new List<string>
            {
                "using My.Namespace;",
                "using My.Other.Namespace;",
                "using My.NameSpace;",
                "using Alias=My.Something;"
            };
            var actual = CSharpSyntaxNodeExtensions.GetUsingsFromContainingCompilationUnit(syntaxNode);
            Assert.IsNotNull(actual);
            CollectionAssert.AreEquivalent(expected, actual.ToList());
        }

        [TestMethod]
        public void FindCompilationUnitTest()
        {
            var code = @"using System;
namespace
{
    class Class1
    {
        public string Property1 { get; set; }
    }
}";
            var compilationUnit = (CompilationUnitSyntax)SyntaxFactory.ParseSyntaxTree(code).GetRoot();
            int start = code.IndexOf("get");
            int end = start + "get".Length;
            var syntaxNode = (CSharpSyntaxNode)compilationUnit.FindNode(TextSpan.FromBounds(start, end));
            var actual = CSharpSyntaxNodeExtensions.FindCompilationUnit(syntaxNode);
            Assert.IsNotNull(actual);
            Assert.AreSame(compilationUnit, actual);
        }

        [TestMethod]
        public void FindCompilationUnit_InvalidOperationException_Test()
        {
            var syntaxNode = SyntaxFactory.ClassDeclaration("class1");
            var exception = Assert.ThrowsException<InvalidOperationException>(() => CSharpSyntaxNodeExtensions.FindCompilationUnit(syntaxNode));
            Assert.IsFalse(string.IsNullOrWhiteSpace(exception.Message));
        }
    }
}