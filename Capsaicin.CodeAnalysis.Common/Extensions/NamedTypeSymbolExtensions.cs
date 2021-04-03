using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capsaicin.CodeAnalysis.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="INamedTypeSymbol"/>
    /// </summary>
    public static class NamedTypeSymbolExtensions
    {
        /// <summary>
        /// Returns whether the full name of the specified <paramref name="namedTypeSymbol"/> equals the <paramref name="fullTypeName"/> parameter.
        /// </summary>
        /// <param name="namedTypeSymbol"></param>
        /// <param name="fullTypeName"></param>
        /// <returns></returns>
        public static bool EqualsFullName(this INamedTypeSymbol namedTypeSymbol, string fullTypeName)
            => namedTypeSymbol.ToDisplayString() == fullTypeName;

        /// <summary>
        /// Returns whether the <paramref name="namedTypeSymbol"/> has a base type with the specified <paramref name="fullTypeName"/>.
        /// </summary>
        /// <param name="namedTypeSymbol"></param>
        /// <param name="fullTypeName"></param>
        /// <returns></returns>
        public static bool HasBaseTypeOf(this INamedTypeSymbol namedTypeSymbol, string fullTypeName)
        {
            var baseType = namedTypeSymbol.BaseType;
            while (baseType is not null)
            {
                var fullName = baseType.ToDisplayString();
                if (fullName == fullTypeName)
                {
                    return true;
                }
                baseType = baseType.BaseType;
            }
            return false;
        }
    }
}
