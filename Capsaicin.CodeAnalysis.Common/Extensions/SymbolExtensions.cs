using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;

namespace Capsaicin.CodeAnalysis.Extensions
{
    /// <summary>
    /// Extensions methods related to <see cref="ISymbol"/>
    /// </summary>
    public static class SymbolExtensions
    {
        public static Stack<INamedTypeSymbol> GetContainingTypes(this ISymbol symbol)
        {
            var stack = new Stack<INamedTypeSymbol>();

            INamedTypeSymbol? current = symbol.ContainingType;
            while (current is not null)
            {
                stack.Push(current);
                current = current.ContainingType;
            }
            return stack;
        }

        public static IEnumerable<INamedTypeSymbol> EnumerateContainingTypesOutwards(this ISymbol symbol)
        {
            INamedTypeSymbol? current = symbol.ContainingType;
            while (current is not null)
            {
                yield return current;
                current = current.ContainingType;
            }
        }

        public static bool HasAttribute(this ISymbol symbol, string attributeFullName)
            => symbol.GetAttributes().Any(a => a.EqualsAttributeClass(attributeFullName));
    }
}
