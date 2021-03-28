using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capsaicin.CodeAnalysis.Extensions
{
    /// <summary>
    /// Extensions methods for <see cref="ITypeSymbol"/>.
    /// </summary>
    public static class TypeSymbolExtensions
    {
        /// <summary>
        /// Returns whether <c>null</c> value can be assigned to variables of the specified type.
        /// </summary>
        /// <param name="typeSymbol"></param>
        /// <returns></returns>
        public static bool AllowsNull(this ITypeSymbol typeSymbol)
            => typeSymbol.NullableAnnotation == NullableAnnotation.Annotated;
    }
}
