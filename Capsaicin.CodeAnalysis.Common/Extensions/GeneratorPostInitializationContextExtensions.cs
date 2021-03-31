using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Capsaicin.CodeAnalysis.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="GeneratorPostInitializationContext"/>.
    /// </summary>
    public static class GeneratorPostInitializationContextExtensions
    {
        /// <summary>
        /// Copies an embedded resource to the output of a source generator
        /// </summary>
        /// <param name="context">The context to which the source text is added.</param>
        /// <param name="sourceGenerator">The source generator from which the assembly containing the embedded resource is derived (both must be in the same assembly).</param>
        /// <param name="sourceFromResourceSpecifier">Specifies the input resource name and the hint name of the output source text.</param>
        public static void GenerateSourceFromResource(this GeneratorPostInitializationContext context, ISourceGenerator sourceGenerator, SourceFromResourceSpecifier sourceFromResourceSpecifier)
        {
            var resourceAssembly = sourceGenerator.GetType().Assembly;
            GenerateSourceFromResource(context, resourceAssembly, sourceFromResourceSpecifier);
        }

        /// <summary>
        /// Copies an embedded resource to the output of a source generator
        /// </summary>
        /// <param name="context">The context to which the source text is added.</param>
        /// <param name="resourceAssembly">The assembly that contains the embedded resource.</param>
        /// <param name="sourceFromResourceSpecifier">Specifies the input resource name and the hint name of the output source text.</param>
        public static void GenerateSourceFromResource(this GeneratorPostInitializationContext context, Assembly resourceAssembly, SourceFromResourceSpecifier sourceFromResourceSpecifier)
        {
            var (resourceName, hintName) = sourceFromResourceSpecifier;
            using var resourceStream = resourceAssembly.GetManifestResourceStream(resourceName);
            using var resourceStreamReader = new StreamReader(resourceStream);
            var sourceCode = resourceStreamReader.ReadToEnd();
            context.AddSource(hintName, SourceText.From(sourceCode, Encoding.UTF8));
        }
    }
}
