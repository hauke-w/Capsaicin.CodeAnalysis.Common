using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capsaicin.CodeAnalysis.Extensions
{
    [TestClass]
    public class SourceFromResourceSpecifierTests
    {
        [TestMethod]
        [DataRow("MyResource")]
        [DataRow("My.Resource")]
        public void Create1Test(string resourceAndHintName)
        {
            var actual = SourceFromResourceSpecifier.Create(resourceAndHintName);
            Assert.IsNotNull(actual);
            Assert.AreEqual(resourceAndHintName, actual.ResourceName);
            Assert.AreEqual(resourceAndHintName, actual.SourceHintName);
        }

        [TestMethod]
        [DataRow("MyResource", "Hint1")]
        [DataRow("My.Resource", "Hint2.cs")]
        public void Create2Test(string resourceName, string sourceHintName)
        {
            var actual = SourceFromResourceSpecifier.Create(resourceName, sourceHintName);
            Assert.IsNotNull(actual);
            Assert.AreEqual(resourceName, actual.ResourceName);
            Assert.AreEqual(sourceHintName, actual.SourceHintName);
        }

        [TestMethod]
        [DataRow("My.", "Resource", "My.Resource", "Resource")]
        [DataRow("my.X", "Something.cs", "my.XSomething.cs", "Something.cs")]
        public void CreateFromResourcePrefixAndSuffixTest(string resourcePrefix, string resourceSuffixAndHintName, string expectedResourceName, string expectedSourceHintName)
        {
            var actual = SourceFromResourceSpecifier.CreateFromResourcePrefixAndSuffix(resourcePrefix, resourceSuffixAndHintName);
            Assert.IsNotNull(actual);
            Assert.AreEqual(expectedResourceName, actual.ResourceName);
            Assert.AreEqual(expectedSourceHintName, actual.SourceHintName);
        }

        [TestMethod]
        [DataRow("My.Ns.Resource.txt")]
        public void ImplicitOperatorTest(string x)
        {
            SourceFromResourceSpecifier actual = x;
            Assert.IsNotNull(actual);
            Assert.AreEqual(x, actual.ResourceName);
            Assert.AreEqual(x, actual.SourceHintName);
        }
    }
}
