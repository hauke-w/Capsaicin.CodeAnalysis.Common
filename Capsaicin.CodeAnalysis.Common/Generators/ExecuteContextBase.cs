using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capsaicin.CodeAnalysis.Generators
{
    /// <summary>
    /// Base class for a context of code generation.
    /// This is intended to be used within a source generator.
    /// </summary>
    public abstract class ExecuteContextBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="generatorExecutionContext"></param>
        protected ExecuteContextBase(GeneratorExecutionContext generatorExecutionContext)
        {
            GeneratorExecutionContext = generatorExecutionContext;
        }

        private readonly GeneratorExecutionContext GeneratorExecutionContext;

        /// <summary>
        /// Adds a diagnostic to the compilation. The first location of the <paramref name="forSymbol"/>
        /// is used as location of the diagnostic. The name of the symbol is used as message argument.
        /// </summary>
        /// <param name="diagnosticDescriptor">A <see cref="DiagnosticDescriptor"/> describing the diagnostic.</param>
        /// <param name="forSymbol">The symbol that is the context of the diagnostic.</param>
        protected void ReportDiagnostic(DiagnosticDescriptor diagnosticDescriptor, ISymbol forSymbol)
            => ReportDiagnostic(diagnosticDescriptor, forSymbol.Locations[0], forSymbol.Name);

        /// <summary>
        /// Adds a diagnostic to the compilation.
        /// </summary>
        /// <param name="diagnosticDescriptor">A <see cref="DiagnosticDescriptor"/> describing the diagnostic.</param>
        /// <param name="location">An optional primary location of the diagnostic.</param>
        /// <param name="messageArgs">Arguments to the message of the diagnostic.</param>
        protected void ReportDiagnostic(DiagnosticDescriptor diagnosticDescriptor, Location? location, params object?[] messageArgs)
        {
            var diagnostic = Diagnostic.Create(diagnosticDescriptor, location, messageArgs);
            GeneratorExecutionContext.ReportDiagnostic(diagnostic);
        }
    }
}
