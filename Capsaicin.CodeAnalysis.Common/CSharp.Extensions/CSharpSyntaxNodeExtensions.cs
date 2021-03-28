using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Capsaicin.UnitTesting.Generators
{
    /// <summary>
    /// Extension methods for <see cref="CSharpSyntaxNode"/>.
    /// </summary>
    public static class CSharpSyntaxNodeExtensions
    {
        /// <summary>
        /// Gets the using expressions from the compilation unit containing the specified <paramref name="syntaxNode"/>.
        /// </summary>
        /// <param name="syntaxNode"></param>
        /// <returns></returns>
        public static HashSet<string> GetUsingsFromContainingCompilationUnit(this CSharpSyntaxNode syntaxNode)
        {
            var compilationUnit = FindCompilationUnit(syntaxNode);
            var usings = compilationUnit.Usings
                .Select(it => it.ToString());
            return new HashSet<string>(usings);
        }

        /// <summary>
        /// Returns the <see cref="CompilationUnitSyntax"/> containing the specified <paramref name="syntaxNode"/>.
        /// </summary>
        /// <param name="syntaxNode"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Thrown if the containing CompilationUnitSyntax was not found.</exception>
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
