using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Capsaicin.CodeAnalysis.Generators
{
    /// <summary>
    /// Helper class for generating partial parent types for a nested type
    /// </summary>
    public class NestingTypesGenerator
    {
        /// <summary>
        /// The number of space characters (' ') that are used per indention level.
        /// </summary>
        public int IndentionStep { get; set; } = 4;

        /// <summary>
        /// Represents a method that adds sourcecode to <paramref name="sourceBuilder" /> with the specified intention.
        /// </summary>
        /// <param name="sourceBuilder"></param>
        /// <param name="indentionLevel"></param>
        public delegate void ContentGenerator(StringBuilder sourceBuilder, int indentionLevel);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol">A symbol that represents a member of type</param>
        /// <param name="sourceBuilder">The <see cref="StringBuilder"/> to which the generated code will be appended.</param>
        /// <param name="contentGenerator">A function that will generate the content for <paramref name="symbol"/>.</param>
        /// <param name="rootIndentionLevel">The indention level for the outer type</param>
        public void GenerateContainingPartialTypeRecursive(ISymbol symbol, StringBuilder sourceBuilder, ContentGenerator contentGenerator, int rootIndentionLevel)
        {
            var containingType = symbol.ContainingType;
            if (containingType is null)
            {
                contentGenerator(sourceBuilder, rootIndentionLevel);
            }
            else
            {
                GenerateContainingPartialTypeRecursive(containingType, sourceBuilder, GeneratePartialType, rootIndentionLevel);
            }

            void GeneratePartialType(StringBuilder sourceBuilder, int indentionLevel)
            {
                var padding = new string(' ', indentionLevel * IndentionStep);

                var typeKind = containingType.TypeKind switch
                {
                    TypeKind.Struct => "struct",
                    TypeKind.Interface => "interface",
                    TypeKind.Class when containingType.IsRecord => "record",
                    TypeKind.Class => "class",
                    _ => throw new NotSupportedException($"Cannot generate partial type hierarchy, type kind '{containingType.TypeKind}' is not supported.")
                };

                sourceBuilder.Append(padding);
                sourceBuilder.Append("partial ");
                sourceBuilder.Append(typeKind);
                sourceBuilder.Append(" ");
                sourceBuilder.AppendLine(containingType.Name);

                sourceBuilder.Append(padding);
                sourceBuilder.AppendLine("{");
                contentGenerator(sourceBuilder, indentionLevel + 1);
                sourceBuilder.Append(padding);
                sourceBuilder.AppendLine("}");
            }
        }
    }
}
