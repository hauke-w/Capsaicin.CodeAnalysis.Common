using Microsoft.CodeAnalysis;

namespace Capsaicin.CodeAnalysis.Extensions
{
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
        public static bool EqualsAttributeClass(this AttributeData attributeData, string attributeFullName)
            => attributeData.AttributeClass?.ToDisplayString() == attributeFullName;
    }
}
