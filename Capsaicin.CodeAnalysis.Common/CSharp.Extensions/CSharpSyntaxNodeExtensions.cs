using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Capsaicin.UnitTesting.Generators
{
    public static class CSharpSyntaxNodeExtensions
    {
        public static HashSet<string> GetUsingsFromContainingCompilationUnit(this CSharpSyntaxNode syntaxNode)
        {
            var compilationUnit = FindCompilationUnit(syntaxNode);
            var usings = compilationUnit.Usings
                .Select(it => it.ToString());
            return new HashSet<string>(usings);
        }

        public static CompilationUnitSyntax FindCompilationUnit(this CSharpSyntaxNode? syntaxNode)
        {
            SyntaxNode? current = syntaxNode;
            while (current is not null)
            {
                if (current is CompilationUnitSyntax compilationUnit)
                {
                    return compilationUnit;
                }
                current = current.Parent;
            }
            throw new InvalidOperationException("Compilation unit not found");
        }
    }
}
