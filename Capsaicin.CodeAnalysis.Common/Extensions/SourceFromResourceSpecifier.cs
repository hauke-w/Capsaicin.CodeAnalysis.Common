namespace Capsaicin.CodeAnalysis.Extensions
{
    /// <summary>
    /// Data structure that specifies a <see cref="ResourceName"/> and <see cref="SourceHintName"/> pair.
    /// <see cref="ResourceName"/> specifies the a resource that will be copied to a source generators output.
    /// <see cref="SourceHintName"/> is an identifier that can be used to reference the generated source text and must be unique within the generator.
    /// </summary>
    public record SourceFromResourceSpecifier(string ResourceName, string SourceHintName)
    {
        /// <summary>
        /// Creates a <see cref="SourceFromResourceSpecifier"/> where the <see cref="ResourceName"/> is the concatenation
        /// of <paramref name="resourcePrefix"/> and <paramref name="resourceSuffixAndHintName"/>. <see cref="SourceHintName"/> is set to <paramref name="resourceSuffixAndHintName"/>.
        /// </summary>
        /// <param name="resourcePrefix">A prefix of the resource id.</param>
        /// <param name="resourceSuffixAndHintName">Suffix of the resource id and will be used as <see cref="SourceHintName"/>.</param>
        /// <returns></returns>
        public static SourceFromResourceSpecifier CreateFromResourcePrefixAndSuffix(string resourcePrefix, string resourceSuffixAndHintName)
            => new SourceFromResourceSpecifier(resourcePrefix + resourceSuffixAndHintName, resourceSuffixAndHintName);

        /// <summary>
        /// Creates a <see cref="SourceFromResourceSpecifier"/> where <see cref="ResourceName"/> and <see cref="SourceHintName"/>
        /// equal the <paramref name="resourceAndHintName"/> parameter.
        /// </summary>
        /// <param name="resourceAndHintName"></param>
        /// <returns></returns>
        public static SourceFromResourceSpecifier Create(string resourceAndHintName)
            => new SourceFromResourceSpecifier(resourceAndHintName, resourceAndHintName);

        /// <summary>
        /// Creates a <see cref="SourceFromResourceSpecifier"/> where <see cref="ResourceName"/> is set to the <paramref name="resourceName"/> parameter
        /// and <see cref="SourceHintName"/> is set to the <paramref name="resourceName"/> parameter.
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="sourceHintName"></param>
        /// <returns></returns>
        public static SourceFromResourceSpecifier Create(string resourceName, string sourceHintName)
            => new SourceFromResourceSpecifier(resourceName, sourceHintName);

        /// <summary>
        /// Creates a <see cref="SourceFromResourceSpecifier"/> where <see cref="ResourceName"/> and <see cref="SourceHintName"/>
        /// equal the <paramref name="resourceAndHintName"/> parameter.
        /// </summary>
        /// <param name="resourceAndHintName"></param>
        public static implicit operator SourceFromResourceSpecifier(string resourceAndHintName)
            => new SourceFromResourceSpecifier(resourceAndHintName, resourceAndHintName);
    }
}
