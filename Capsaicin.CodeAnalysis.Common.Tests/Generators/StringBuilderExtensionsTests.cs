using Microsoft.VisualStudio.TestTools.UnitTesting;
using Capsaicin.CodeAnalysis.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsaicin.CodeAnalysis.Generators
{
    [TestClass]
    public class StringBuilderExtensionsTests
    {
        [TestMethod]
        [DataRow("a, b, C", ", ", "a", "b", "C")]
        [DataRow("Hello test!", " ", "Hello", "test!")]
        [DataRow("Just one value", ".", "Just one value")]
        [DataRow("123", "", "1", "2", "3")]
        [DataRow("", "x")]
        [DataRow(" A--b--c - d ", "-", " A", "", "b", null, "c ", " d ")]
        [DataRow("ab", null, "a", "b")]
        public void AppendJoinTest(string expected, string? separator, params string?[] values)
        {
            var stringBuilder = new StringBuilder();
            StringBuilderExtensions.AppendJoin(stringBuilder, separator, values);

            var actual = stringBuilder.ToString();
            Assert.AreEqual(expected, actual);

            // verify result is same as in .net core implementation of StringBuilder.AppendJoin(...)
            var stringBuilder2 = new StringBuilder();
            var expected2 = stringBuilder2.AppendJoin(separator, values).ToString();
            Assert.AreEqual(expected2, expected);
        }

        [TestMethod]
        public void AppendJoin_ArgumentNullException_Test()
        {
            var stringBuilder = new StringBuilder();
            Assert.ThrowsException<ArgumentNullException>(()=> StringBuilderExtensions.AppendJoin(stringBuilder, " - ", null!));
        }

        [TestMethod]
        [DataRow("", "   ", null, true)]
        [DataRow("", "   ", null, false)]
        [DataRow("   ", "   ", "", true)]
        [DataRow("", " x ", "", false)]
        [DataRow(" line1\r\n\r\n line3", " ", "line1\n\nline3", false)]
        [DataRow("-line1\r\n-\r\n-line3", "-", "line1\n\r\nline3", true)]
        [DataRow("    line1", "    ", "line1", true)]
        [DataRow("_\r\n_line1", "_", "\nline1", true)]
        [DataRow("\nline1", null, "\nline1", true)]
        [DataRow("\nline1", "", "\nline1", true)]
        public void AppendIndented1Test(string expected, string? indention, string? value, bool indentEmptyLines)
        {
            var stringBuilder = new StringBuilder();
            var actual = StringBuilderExtensions.AppendIndented(stringBuilder, indention, value, indentEmptyLines);
            Assert.AreSame(stringBuilder, actual);
            var actualResult = stringBuilder.ToString();
            Assert.AreEqual(expected, actualResult);
        }

        [TestMethod]
        [DataRow("", 1, null, true, 4)]
        [DataRow("", 1, null, false, 5)]
        [DataRow("  ", 1, "", true, 2)]
        [DataRow("", 2, "", false, 4)]
        [DataRow(" line1\r\n\r\n line3", 1, "line1\n\nline3", false, 1)]
        [DataRow("      line1\r\n      \r\n      line3", 2, "line1\n\r\nline3", true, 3)]
        [DataRow("    line1", 1, "line1", true, 4)]
        [DataRow("\nline1", 0, "\nline1", true, 4)]
        [DataRow("   \r\n   line1", 3, "\nline1", true, 1)]
        [DataRow("\n   line1", 1, "\n   line1", true, 0)]
        public void AppendIndented2Test(string expected, int indentionLevel, string? value, bool indentEmptyLines, int indentionStep)
        {
            var stringBuilder = new StringBuilder();
            var actual = StringBuilderExtensions.AppendIndented(stringBuilder, indentionLevel, value, indentEmptyLines, indentionStep);
            Assert.AreSame(stringBuilder, actual);
            var actualResult = stringBuilder.ToString();
            Assert.AreEqual(expected, actualResult);
        }
    }
}