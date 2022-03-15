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
        [Obsolete("Compare symbols using SymbolEqualityComparer instead")]
        public static bool EqualsFullName(this INamedTypeSymbol namedTypeSymbol, string fullTypeName)
            => namedTypeSymbol.ToDisplayString() == fullTypeName;

        /// <summary>
        /// Returns whether the <paramref name="namedTypeSymbol"/> has a base type with the specified <paramref name="fullTypeName"/>.
        /// </summary>
        /// <param name="namedTypeSymbol"></param>
        /// <param name="fullTypeName"></param>
        /// <returns></returns>
        [Obsolete("Use overload HasBaseTypeOf(INamedTypeSymbol namedTypeSymbol, INamedTypeSymbol baseTypeCandidate) or compare symbols using SymbolEqualityComparer instead")]
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

        /// <summary>
        /// Returns whether <paramref name="baseType"/> is direct or indirect <see cref="ITypeSymbol.BaseType">base type</see> of <paramref name="type"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="baseType">The potential base type</param>
        /// <returns></returns>
        public static bool HasBaseTypeOf(this ITypeSymbol type, ITypeSymbol baseType)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (baseType is null)
            {
                throw new ArgumentNullException(nameof(baseType));
            }

            var currentBaseType = type.BaseType;
            while (currentBaseType is not null)
            {
                if (SymbolEqualityComparer.Default.Equals(currentBaseType, baseType))
                {
                    return true;
                }
                currentBaseType = currentBaseType.BaseType;
            }
            return false;
        }

        /// <summary>
        /// Returns whether <paramref name="type"/> and <paramref name="other"/> are equal
        /// or whether <paramref name="other"/> is direct or indirect <see cref="ITypeSymbol.BaseType">base type</see> of <paramref name="type"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool EqualsOrHasBaseTypeOf(this ITypeSymbol type, ITypeSymbol other)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return SymbolEqualityComparer.Default.Equals(type, other)
                || type.HasBaseTypeOf(other);
        }
    }
}
