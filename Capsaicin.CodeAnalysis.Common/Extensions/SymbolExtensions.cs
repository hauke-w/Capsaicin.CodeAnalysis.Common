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
        /// <summary>
        /// Gets the stack of parent <see cref="INamedTypeSymbol"/>s for the specified <paramref name="symbol"/>.
        /// The parent of type is determinde by the <see cref="ISymbol.ContainingType"/> property.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns>A stack with the root containing type on top and the direct containing type at the bottom.</returns>
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

        /// <summary>
        /// Enumerates the <see cref="INamedTypeSymbol"/>s that are parent of the specified <paramref name="symbol"/>.
        /// The parent of type is determinde by the <see cref="ISymbol.ContainingType"/> property.
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns>An enumerable that first returns the direct parent and the root as last.</returns>
        public static IEnumerable<INamedTypeSymbol> EnumerateContainingTypesOutwards(this ISymbol symbol)
        {
            INamedTypeSymbol? current = symbol.ContainingType;
            while (current is not null)
            {
                yield return current;
                current = current.ContainingType;
            }
        }

        /// <summary>
        /// Returns whether the specified <paramref name="symbol"/> has a attribute whose class' name equals the specified <paramref name="attributeFullName"/>.
        /// </summary>
        /// <param name="symbol">The symbol that may have attributes.</param>
        /// <param name="attributeFullName"></param>
        /// <returns><c>true</c> if the full name of any attribute's full class name equals <paramref name="attributeFullName"/>.</returns>
        public static bool HasAttribute(this ISymbol symbol, string attributeFullName)
            => symbol.GetAttributes().Any(a => a.EqualsAttributeClass(attributeFullName));

        /// <summary>
        /// Enumerates the attributes of the specified type.
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="attributeFullName">The full name of the attribute class that is searched.</param>
        /// <param name="includeSpecializing">If <c>true</c>, sub classes of the attribute type are included.</param>
        /// <returns></returns>
        public static IEnumerable<AttributeData> GetAttributes(this ISymbol symbol, string attributeFullName, bool includeSpecializing = false)
        {
            return symbol.GetAttributes().Where(HasAttributeClass);

            bool HasAttributeClass(AttributeData a)
            {
                var attributeClass = a.AttributeClass;
                return (attributeClass is not null && attributeClass.EqualsFullName(attributeFullName))
                    || (attributeClass is not null && includeSpecializing && attributeClass.HasBaseTypeOf(attributeFullName));
            }
        }
    }
}
