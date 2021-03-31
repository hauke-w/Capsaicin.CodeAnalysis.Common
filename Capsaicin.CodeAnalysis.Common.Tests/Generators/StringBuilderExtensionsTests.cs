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
        public void AppendIndentedTest(string expected, string? indention, string? value, bool indentEmptyLines)
        {
            var stringBuilder = new StringBuilder();
            StringBuilderExtensions.AppendIndented(stringBuilder, indention, value, indentEmptyLines);
            var actual = stringBuilder.ToString();
            Assert.AreEqual(expected, actual);
        }
    }
}