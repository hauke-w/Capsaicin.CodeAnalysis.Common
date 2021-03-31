using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Capsaicin.CodeAnalysis.Generators
{
    /// <summary>
    /// Extension methods for <see cref="StringBuilder"/>.
    /// </summary>
    /// <remarks>
    /// This adds some methods missing in .Net standard 2.0
    /// </remarks>
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Appends all <paramref name="values"/> with <paramref name="separator"/> between each value.
        /// </summary>
        /// <param name="stringBuilder">The StringBuilder to which <paramref name="values"/> will be appended.</param>
        /// <param name="separator">The string that is appended to <paramref name="stringBuilder"/> between each value in <paramref name="values"/>.</param>
        /// <param name="values">The values to be appended. If null, nothing will be appended.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="values"/> is null.</exception>
        public static void AppendJoin(this StringBuilder stringBuilder, string? separator, params string?[] values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            if (values.Length >= 1 )
            {
                stringBuilder.Append(values[0]);
                for (int i = 1; i < values.Length; i++)
                {
                    stringBuilder.Append(separator);
                    stringBuilder.Append(values[i]);
                }
            }
        }

        private static readonly Regex LineBreakRegex = new Regex(@"\r?\n");
        /// <summary>
        /// Appends a string to <paramref name="stringBuilder"/> with the specified <paramref name="indention"/>.
        /// If the <paramref name="indention"/> is null, no indention will be applied.
        /// If value is null, nothing will be appended to <paramref name="stringBuilder"/>.
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <param name="indention"></param>
        /// <param name="value"></param>
        /// <param name="indentEmptyLines">Specifies whether empty lines must be indented (true) or not (false).</param>
        public static void AppendIndented(this StringBuilder stringBuilder, string? indention, string? value, bool indentEmptyLines = false)
        {
            if (value is not null)
            {
                if (indention is not { Length: > 0 })
                {
                    stringBuilder.Append(value);
                }
                else if (value.Length > 0)
                {
                    var lines = LineBreakRegex.Split(value);
                    string line;
                    int nLinesWithBreak = lines.Length - 1;
                    for (int i = 0; i < nLinesWithBreak; i++)
                    {
                        line = lines[i];
                        if (line.Length > 0 || indentEmptyLines)
                        {
                            stringBuilder.Append(indention);
                        }
                        stringBuilder.AppendLine(line);
                    }

                    line = lines[nLinesWithBreak];
                    if (line.Length > 0 || indentEmptyLines)
                    {
                        stringBuilder.Append(indention);
                    }
                    stringBuilder.Append(line);
                }
                else if(indentEmptyLines)
                {
                    stringBuilder.Append(indention);
                }
            }
        }
    }
}
