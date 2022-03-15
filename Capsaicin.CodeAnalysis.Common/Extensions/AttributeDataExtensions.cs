using Microsoft.CodeAnalysis;
using System;

namespace Capsaicin.CodeAnalysis.Extensions;

/// <summary>
/// Extensions methods for <see cref="AttributeData"/>
/// </summary>
public static class AttributeDataExtensions
{
    /// <summary>
    /// Returns whether the attributes full name equals the specified name.
    /// </summary>
    /// <param name="attributeData"></param>
    /// <param name="attributeFullName"></param>
    /// <returns></returns>
    [Obsolete]
    public static bool EqualsAttributeClass(this AttributeData attributeData, string attributeFullName)
        => attributeData.AttributeClass?.EqualsFullName(attributeFullName) == true;

    public static bool EqualsOrIsSpecializingAttributeClass(this AttributeData attribute, ITypeSymbol attributeClass)
        => attribute.AttributeClass?.EqualsOrHasBaseTypeOf(attributeClass) == true;

    public static bool EqualsAttributeClass(this AttributeData attribute, ITypeSymbol attributeClass)
        => attribute.AttributeClass is not null && SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, attributeClass);
}
